using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Tests.BusinessRules
{
    [TestClass]
    public class GenerateRootValidationRulesActionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GameRootFolderDontExists()
        {
            //Test data
            const string dirFolder = "customDirFolder";

            //Set mocks 
            var fileSystemDalMock = MockRepository.GenerateMock<IFileSystemDal>();
            fileSystemDalMock.Expect(x => x.DirectoryExists(Arg<string>.Is.Equal(dirFolder))).Return(false);

            //Run Tests
            var action = new GenerateRootValidationRulesAction(fileSystemDalMock);
            action.Execute(dirFolder).ToList();
        }

        [TestMethod]
        public void GetAllDirsRules()
        {
            //Test data
            const string rootDirFolder = "customDirFolder";
            var dirList = new List<DirectoryInfo>();

            //Set stubs
            var dir1 = MockRepository.GenerateStub<DirectoryInfo>();
            dir1.Stub(x => x.Name).Return("subfolder1");
            dir1.Stub(x => x.FullName).Return("fullPathSubfolder1");
            dirList.Add(dir1);

            var dir2 = MockRepository.GenerateStub<DirectoryInfo>();
            dir2.Stub(x => x.Name).Return("subfolder2");
            dir2.Stub(x => x.FullName).Return("fullPathSubfolder2");
            dirList.Add(dir2);

            //Set mocks 
            var fileSystemDalMock = MockRepository.GenerateMock<IFileSystemDal>();
            fileSystemDalMock.Expect(x => x.DirectoryExists(Arg<string>.Is.Anything)).Return(true);
            fileSystemDalMock.Expect(x => x.DirectoryGetChildren(Arg<string>.Is.Equal(rootDirFolder))).Return(dirList);
            fileSystemDalMock.Expect(x => x.DirectoryGetChildren(Arg<string>.Is.NotEqual(rootDirFolder))).Return(new DirectoryInfo[0]);
            fileSystemDalMock.Expect(x => x.GetFiles(Arg<string>.Is.Anything)).Return(new FileInfo[0]);

            //Run Tests
            var action = new GenerateRootValidationRulesAction(fileSystemDalMock);
            var result = action.Execute(rootDirFolder).ToList();

            //Validate
            fileSystemDalMock.VerifyAllExpectations();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(RootValidationTypeEnum.FolderValidation, result[0].Type);
            Assert.AreEqual(dir1.Name, result[0].WitnessName);
            Assert.AreEqual(RootValidationTypeEnum.FolderValidation, result[1].Type);
            Assert.AreEqual(dir2.Name, result[1].WitnessName);
        }

        [TestMethod]
        public void GetAllExesRules()
        {
            //Test data
            const string rootDirFolder = "customDirFolder";
            var dirList = new List<DirectoryInfo>();

            //Set stubs
            var dir1 = MockRepository.GenerateStub<DirectoryInfo>();
            dir1.Stub(x => x.Name).Return("subfolder1");
            dir1.Stub(x => x.FullName).Return("fullPathSubfolder1");
            dirList.Add(dir1);

            var exe1 = MockRepository.GenerateStub<FileInfo>();
            exe1.Stub(x => x.Name).Return("exe1.exe");
            exe1.Stub(x => x.FullName).Return(@"customDirFolder\exe1.exe");

            var exe2 = MockRepository.GenerateStub<FileInfo>();
            exe2.Stub(x => x.Name).Return("exe2.exe");
            exe2.Stub(x => x.FullName).Return(@"customDirFolder\subfolder1\exe2.exe");

            //Set mocks 
            var fileSystemDalMock = MockRepository.GenerateMock<IFileSystemDal>();
            fileSystemDalMock.Expect(x => x.DirectoryExists(Arg<string>.Is.Anything)).Return(true);
            fileSystemDalMock.Expect(x => x.DirectoryGetChildren(Arg<string>.Is.Equal(rootDirFolder))).Return(dirList);
            fileSystemDalMock.Expect(x => x.DirectoryGetChildren(Arg<string>.Is.Anything)).Return(new DirectoryInfo[0]);
            fileSystemDalMock.Expect(x => x.GetFiles(Arg<string>.Is.Equal(rootDirFolder))).Return(new [] { exe1 });
            fileSystemDalMock.Expect(x => x.GetFiles(Arg<string>.Is.Equal(dir1.FullName))).Return(new[] { exe2 });

            //Run Tests
            var action = new GenerateRootValidationRulesAction(fileSystemDalMock);
            var result = action.Execute(rootDirFolder).ToList();

            //Validate
            fileSystemDalMock.VerifyAllExpectations();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(RootValidationTypeEnum.FolderValidation, result[0].Type);
            Assert.AreEqual(dir1.Name, result[0].WitnessName);
            Assert.AreEqual(RootValidationTypeEnum.FileValidation, result[1].Type);
            Assert.AreEqual(exe1.Name, result[1].WitnessName);
            Assert.AreEqual(RootValidationTypeEnum.FileValidation, result[2].Type);
            Assert.AreEqual(dir1.Name + "\\" + exe2.Name, result[2].WitnessName);
        }
    }
}
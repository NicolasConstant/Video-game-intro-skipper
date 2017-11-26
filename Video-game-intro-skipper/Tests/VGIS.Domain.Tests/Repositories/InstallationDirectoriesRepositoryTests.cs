using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.Tests.Repositories
{
    [TestClass]
    public class InstallationDirectoriesRepositoryTests
    {
        [TestMethod]
        public void GetAllInstallationFolders_Default()
        {
            const string pathToSettingFile = "myCustomPath";
            var defaultDirectories = "[\"dir1\", \"dir2\"]";

            #region Stubs and Mocks
            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToSettingFile))).Return(defaultDirectories);
            #endregion

            var repo = new InstallationDirectoriesRepository(pathToSettingFile, null, dalMock);
            var installationFolders = repo.GetAllInstallationFolders().ToList();
            
            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(2, installationFolders.Count);
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir1"));
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir2"));
            #endregion
        }

        [TestMethod]
        public void GetAllInstallationFolders_WtCustom()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var defaultDirectories = "[\"dir1\", \"dir2\"]";
            var customDirectories = "[\"dir3\", \"dir4\"]";

            #region Stubs and Mocks
            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToDefaultSettingFile))).Return(defaultDirectories);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(customDirectories);
            #endregion

            var repo = new InstallationDirectoriesRepository(pathToDefaultSettingFile, pathToCustomSettingFile, dalMock);
            var installationFolders = repo.GetAllInstallationFolders().ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(4, installationFolders.Count);
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir1"));
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir2"));
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir3"));
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir4"));
            #endregion
        }

        [TestMethod]
        public void AddNewCustomFolder()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var customDirectories = "[\"dir1\", \"dir2\"]";
            const string newPathToAdd = "dir3";

            #region Stubs and Mocks
            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(customDirectories);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal(pathToCustomSettingFile),
                Arg<string>.Matches(s => s.Contains("dir1") && s.Contains("dir2") && s.Contains("dir3"))));
            dalMock.Expect(x => x.DirectoryExists(newPathToAdd)).Return(true);
            #endregion

            var repo = new InstallationDirectoriesRepository(pathToDefaultSettingFile, pathToCustomSettingFile, dalMock);
            repo.AddNewCustomInstallFolder(newPathToAdd);

            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }

        [TestMethod]
        public void AddNewCustomFolder_PathDontExists()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var customDirectories = "[\"dir1\", \"dir2\"]";
            const string newPathToAdd = "dir3";

            #region Stubs and Mocks
            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(customDirectories);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal(pathToCustomSettingFile),Arg<string>.Is.Anything)).Repeat.Never(); 
            dalMock.Expect(x => x.DirectoryExists(newPathToAdd)).Return(false);
            #endregion

            var repo = new InstallationDirectoriesRepository(pathToDefaultSettingFile, pathToCustomSettingFile, dalMock);
            repo.AddNewCustomInstallFolder(newPathToAdd);

            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }
    }
}

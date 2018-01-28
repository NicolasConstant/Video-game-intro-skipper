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
using VGIS.Domain.Settings;

namespace VGIS.Domain.Tests.Repositories
{
    [TestClass]
    public class InstallationDirectoriesRepositoryTests
    {
        [TestMethod]
        public void GetAllInstallationFolders_UserFileNotCreated()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var defaultDirectories = "[\"dir1\",\"dir2\"]";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();
            globalSettingsStub.DefaultInstallFolderConfigFile = pathToDefaultSettingFile;
            globalSettingsStub.CustomInstallFolderConfigFile = pathToCustomSettingFile;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.FileExists(pathToCustomSettingFile)).Return(false);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal(pathToCustomSettingFile), Arg<string>.Is.Equal(defaultDirectories)));
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToDefaultSettingFile))).Return(defaultDirectories);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(defaultDirectories);
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            var installationFolders = repo.GetAllInstallationFolders().ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(2, installationFolders.Count);
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir1"));
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir2"));
            #endregion
        }

        [TestMethod]
        public void GetAllInstallationFolders_UserFileCreated()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var customDirectories = "[\"dir1\",\"dir2\"]";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();
            globalSettingsStub.DefaultInstallFolderConfigFile = pathToDefaultSettingFile;
            globalSettingsStub.CustomInstallFolderConfigFile = pathToCustomSettingFile;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.FileExists(pathToCustomSettingFile)).Return(true);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToDefaultSettingFile))).Repeat.Never();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(customDirectories);
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            var installationFolders = repo.GetAllInstallationFolders().ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(2, installationFolders.Count);
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir1"));
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir2"));
            #endregion
        }

        [TestMethod]
        public void GetAllInstallationFolders_EmptyCustomFile()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var defaultDirectories = "[\"dir1\", \"dir2\"]";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();
            globalSettingsStub.DefaultInstallFolderConfigFile = pathToDefaultSettingFile;
            globalSettingsStub.CustomInstallFolderConfigFile = pathToCustomSettingFile;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.FileExists(pathToCustomSettingFile)).Return(true);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToDefaultSettingFile))).Repeat.Never();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return("");
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            var installationFolders = repo.GetAllInstallationFolders().ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(0, installationFolders.Count);
            #endregion
        }

        [TestMethod]
        public void AddNewFolder_PathExists_CustomFileAlreadyCreated()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var customDirectories = "[\"dir1\", \"dir2\"]";
            const string newPathToAdd = "dir3";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();
            globalSettingsStub.DefaultInstallFolderConfigFile = pathToDefaultSettingFile;
            globalSettingsStub.CustomInstallFolderConfigFile = pathToCustomSettingFile;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(customDirectories);
            dalMock.Expect(x => x.FileExists(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(true);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal(pathToCustomSettingFile),
                Arg<string>.Matches(s => s.Contains("dir1") && s.Contains("dir2") && s.Contains("dir3"))));
            dalMock.Expect(x => x.DirectoryExists(newPathToAdd)).Return(true);
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            repo.AddNewInstallFolder(newPathToAdd);

            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }

        [TestMethod]
        public void AddNewFolder_PathDontExists_CustomFileAlreadyCreated()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var customDirectories = "[\"dir1\", \"dir2\"]";
            const string newPathToAdd = "dir3";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();
            globalSettingsStub.DefaultInstallFolderConfigFile = pathToDefaultSettingFile;
            globalSettingsStub.CustomInstallFolderConfigFile = pathToCustomSettingFile;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(customDirectories);
            dalMock.Expect(x => x.FileExists(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(true);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal(pathToCustomSettingFile),Arg<string>.Is.Anything)).Repeat.Never(); 
            dalMock.Expect(x => x.DirectoryExists(newPathToAdd)).Return(false);
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            repo.AddNewInstallFolder(newPathToAdd);

            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }

        [TestMethod]
        public void RemoveFolder()
        {
            const string pathToCustomSettingFile = "myCustomPath";
            var customDirectories = "[\"dir1\",\"dir2\"]";
            const string pathToRemove = "dir2";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();
            globalSettingsStub.CustomInstallFolderConfigFile = pathToCustomSettingFile;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(customDirectories);
            dalMock.Expect(x => x.FileExists(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(true);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal(pathToCustomSettingFile), Arg<string>.Is.Equal("[\"dir1\"]")));
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            repo.RemoveInstallFolder(pathToRemove);

            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }

        [TestMethod]
        public void ResetFolders()
        {
            const string pathToDefaultSettingFile = "myDefaultPath";
            const string pathToCustomSettingFile = "myCustomPath";
            var defaultDirectories = "[\"dir1\",\"dir2\",\"dir3\"]";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();
            globalSettingsStub.DefaultInstallFolderConfigFile = pathToDefaultSettingFile;
            globalSettingsStub.CustomInstallFolderConfigFile = pathToCustomSettingFile;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(pathToDefaultSettingFile))).Return(defaultDirectories);
            dalMock.Expect(x => x.FileExists(Arg<string>.Is.Equal(pathToCustomSettingFile))).Return(true);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal(pathToCustomSettingFile), Arg<string>.Is.Equal(defaultDirectories)));
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            repo.ResetInstallationFolders();

            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }

        [TestMethod]
        public void GetSubFolder_DirExists()
        {
            const string parentFolder = "c:/parentFolder";
            var subDirs = new List<DirectoryInfo>
            {
                new DirectoryInfo(parentFolder+"/subfolder1"),
                new DirectoryInfo(parentFolder+"/subfolder2"),
            };

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.DirectoryExists(Arg<string>.Is.Equal(parentFolder))).Return(true);
            dalMock.Expect(x => x.DirectoryGetChildren(Arg<string>.Is.Equal(parentFolder))).Return(subDirs);
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            var result = repo.GetSubFolders(parentFolder).ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();

            Assert.IsTrue(result.Count() == 2);
            Assert.AreEqual(subDirs.First().Name, result.First());
            #endregion

        }

        [TestMethod]
        public void GetSubFolder_DirDontExists()
        {
            const string parentFolder = "parentFolder";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateMock<GlobalSettings>();

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.DirectoryExists(Arg<string>.Is.Equal(parentFolder))).Return(false);
            #endregion

            var repo = new InstallationDirectoriesRepository(globalSettingsStub, dalMock);
            var subfolders = repo.GetSubFolders(parentFolder);

            #region Validate 
            dalMock.VerifyAllExpectations();

            Assert.IsTrue(!subfolders.Any());
            #endregion

        }
    }
}

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

            var repo = new InstallationDirectoriesRepository(pathToSettingFile, dalMock);
            var installationFolders = repo.GetAllInstallationFolders().ToList();
            
            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(2, installationFolders.Count);
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir1"));
            Assert.IsNotNull(installationFolders.Where(x => x.Name == "dir2"));
            #endregion
        }

    }
}

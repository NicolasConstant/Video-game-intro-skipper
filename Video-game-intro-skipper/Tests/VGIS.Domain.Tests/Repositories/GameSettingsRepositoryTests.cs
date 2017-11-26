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
    public class GameSettingsRepositoryTests
    {

        [TestMethod]
        public void GetAllGameSettings()
        {
            const string customPath = "myCustomPath";

            #region Stubs and Mocks
            var file1 = MockRepository.GenerateStub<FileInfo>();
            file1.Expect(x => x.FullName).Return("fullName1");

            var file2 = MockRepository.GenerateStub<FileInfo>();
            file2.Expect(x => x.FullName).Return("fullName2");

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.GetFiles(Arg<string>.Is.Equal(customPath))).Return(new[] { file1, file2 });
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(file1.FullName))).Return(File1);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(file2.FullName))).Return(File2);
            #endregion

            var repo = new GameSettingsRepository(customPath, dalMock);
            var settings = repo.GetAllGameSettings().ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(2, settings.Count);
            Assert.IsNotNull(settings.Where(x => x.Name == "Dead by Daylight"));
            Assert.IsNotNull(settings.Where(x => x.Name == "PLAYERUNKNOWN'S BATTLEGROUND"));
            #endregion
        }

        private const string File1 = "{\"Name\": \"Dead by Daylight\", \"PublisherName\": \"Starbreeze Publishing AB\", \"SettingVersion\": null, \"PotentialRootFolderNames\": [ \"Dead by Daylight\" ], \"ValidationRules\": [{\"Type\": 2,\"WitnessName\": \"DeadByDaylight.exe\"}],\"DisablingIntroductionActions\": [{\"Type\": 2,\"InitialName\": \"DeadByDaylight\\\\Content\\\\Movies\\\\LoadingScreen.mp4\"}],\"IllustrationPlatform\": 1,\"IllustrationId\": \"381210\"}";

        private const string File2 = "{\"Name\": \"PLAYERUNKNOWN'S BATTLEGROUND\",\"PublisherName \": \"Bluehole, Inc\",\"SettingVersion\": null,\"PotentialRootFolderNames\": [ \"PUBG\" ],\"ValidationRules\": [{\"Type\": 2,\"WitnessName\": \"TslGame\\\\Binaries\\\\Win64\\\\TslGame.exe\"}],\"DisablingIntroductionActions\": [{\"Type\": 2,\"InitialName\": \"\\\\TslGame\\\\Content\\\\Movies\\\\LoadingScreen.mp4\"}],\"IllustrationPlatform\": 1,\"IllustrationId\": \"578080\"}";
        
    }
    
}

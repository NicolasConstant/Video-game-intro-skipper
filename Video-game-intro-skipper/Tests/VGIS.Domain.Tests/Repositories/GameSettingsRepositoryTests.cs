using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Rhino.Mocks;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Repositories;
using VGIS.Domain.Settings;

namespace VGIS.Domain.Tests.Repositories
{
    [TestClass]
    public class GameSettingsRepositoryTests
    {
        [TestMethod]
        public void GetAllGameSettings_Default()
        {
            const string defaultPath = "myDefaultPath";
            const string customPath = "myCustomPath";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateStub<GlobalSettings>();
            globalSettingsStub.DefaultGamesSettingsFolder = defaultPath;
            globalSettingsStub.CustomGamesSettingsFolder = customPath;

            var file1 = MockRepository.GenerateStub<FileInfo>();
            file1.Expect(x => x.FullName).Return("fullName1");

            var file2 = MockRepository.GenerateStub<FileInfo>();
            file2.Expect(x => x.FullName).Return("fullName2");

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.GetFiles(Arg<string>.Is.Equal(defaultPath))).Return(new[] { file1, file2 });
            dalMock.Expect(x => x.GetFiles(Arg<string>.Is.Equal(customPath))).Return(new FileInfo[0]);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(file1.FullName))).Return(File1);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(file2.FullName))).Return(File2);
            #endregion

            var repo = new GameSettingsRepository(globalSettingsStub, dalMock);
            var settings = repo.GetAllGameSettings().ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(2, settings.Count);
            Assert.IsNotNull(settings.Where(x => x.Name == "Dead by Daylight"));
            Assert.IsNotNull(settings.Where(x => x.Name == "PLAYERUNKNOWN'S BATTLEGROUND"));
            #endregion
        }

        [TestMethod]
        public void GetAllGameSettings_DefaultAndCustom()
        {
            const string defaultPath = "myDefaultPath";
            const string customPath = "myCustomPath";

            #region Stubs and Mocks
            var globalSettingsStub = MockRepository.GenerateStub<GlobalSettings>();
            globalSettingsStub.DefaultGamesSettingsFolder = defaultPath;
            globalSettingsStub.CustomGamesSettingsFolder = customPath;

            var file1 = MockRepository.GenerateStub<FileInfo>();
            file1.Expect(x => x.FullName).Return("fullName1");

            var file2 = MockRepository.GenerateStub<FileInfo>();
            file2.Expect(x => x.FullName).Return("fullName2");
            
            var file3 = MockRepository.GenerateStub<FileInfo>();
            file3.Expect(x => x.FullName).Return("fullName3");

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.GetFiles(Arg<string>.Is.Equal(defaultPath))).Return(new[] { file1, file2 });
            dalMock.Expect(x => x.GetFiles(Arg<string>.Is.Equal(customPath))).Return(new []{ file3 });
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(file1.FullName))).Return(File1);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(file2.FullName))).Return(File2);
            dalMock.Expect(x => x.ReadAllText(Arg<string>.Is.Equal(file3.FullName))).Return(File3);
            #endregion

            var repo = new GameSettingsRepository(globalSettingsStub, dalMock);
            var settings = repo.GetAllGameSettings().ToList();

            #region Validate 
            dalMock.VerifyAllExpectations();
            Assert.AreEqual(3, settings.Count);
            Assert.IsNotNull(settings.Where(x => x.Name == "Dead by Daylight"));
            Assert.IsNotNull(settings.Where(x => x.Name == "PLAYERUNKNOWN'S BATTLEGROUND"));
            Assert.IsNotNull(settings.Where(x => x.Name == "Ghost Recon: Wildlands"));
            #endregion
        }

        [TestMethod]
        public void AddNewGameSettings_FileDontExist()
        {
            const string customPath = @"myCustomPath\";

            #region Stubs and Mocks
            var gameSettingStub = new GameSetting()
            {
                Name = "My new GAme",
                DisablingIntroductionActions = new List<DisableIntroductionAction>
                {
                    new DisableIntroductionAction()
                    {
                        InitialName = "pathToName",
                        Type = DisableActionTypeEnum.FileRename
                    }
                }
            };
            var serializedGameSettings = JsonConvert.SerializeObject(gameSettingStub);

            var globalSettingsStub = MockRepository.GenerateStub<GlobalSettings>();
            globalSettingsStub.CustomGamesSettingsFolder = customPath;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.DirectoryExists(Arg<string>.Is.Equal(customPath))).Return(true);
            dalMock.Expect(x => x.FileExists($"{customPath}{gameSettingStub.Name}.json")).Return(false);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal($"{customPath}{gameSettingStub.Name}.json"), Arg<string>.Is.Equal(serializedGameSettings)));
            #endregion

            var repo = new GameSettingsRepository(globalSettingsStub, dalMock);
            repo.SaveNewGameSettings(gameSettingStub);
            
            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }

        [TestMethod]
        public void AddNewGameSettings_FileAlreadyExist()
        {
            const string customPath = @"myCustomPath\";

            #region Stubs and Mocks
            var gameSettingStub = new GameSetting()
            {
                Name = "My new GAme",
                DisablingIntroductionActions = new List<DisableIntroductionAction>
                {
                    new DisableIntroductionAction()
                    {
                        InitialName = "pathToName",
                        Type = DisableActionTypeEnum.FileRename
                    }
                }
            };
            var serializedGameSettings = JsonConvert.SerializeObject(gameSettingStub);

            var globalSettingsStub = MockRepository.GenerateStub<GlobalSettings>();
            globalSettingsStub.CustomGamesSettingsFolder = customPath;

            var dalMock = MockRepository.GenerateMock<IFileSystemDal>();
            dalMock.Expect(x => x.DirectoryExists(Arg<string>.Is.Equal(customPath))).Return(true);
            dalMock.Expect(x => x.FileExists($"{customPath}{gameSettingStub.Name}.json")).Return(true);
            dalMock.Expect(x => x.FileExists($"{customPath}{gameSettingStub.Name}_1.json")).Return(true);
            dalMock.Expect(x => x.FileExists($"{customPath}{gameSettingStub.Name}_2.json")).Return(false);
            dalMock.Expect(x => x.FileWriteAllText(Arg<string>.Is.Equal($"{customPath}{gameSettingStub.Name}_2.json"), Arg<string>.Is.Equal(serializedGameSettings)));
            #endregion

            var repo = new GameSettingsRepository(globalSettingsStub, dalMock);
            repo.SaveNewGameSettings(gameSettingStub);

            #region Validate 
            dalMock.VerifyAllExpectations();
            #endregion
        }

        private const string File1 = "{\"Name\": \"Dead by Daylight\", \"PublisherName\": \"Starbreeze Publishing AB\", \"SettingVersion\": null, \"PotentialRootFolderNames\": [ \"Dead by Daylight\" ], \"ValidationRules\": [{\"Type\": 2,\"WitnessName\": \"DeadByDaylight.exe\"}],\"DisablingIntroductionActions\": [{\"Type\": 2,\"InitialName\": \"DeadByDaylight\\\\Content\\\\Movies\\\\LoadingScreen.mp4\"}],\"IllustrationPlatform\": 1,\"IllustrationId\": \"381210\"}";

        private const string File2 = "{\"Name\": \"PLAYERUNKNOWN'S BATTLEGROUND\",\"PublisherName \": \"Bluehole, Inc\",\"SettingVersion\": null,\"PotentialRootFolderNames\": [ \"PUBG\" ],\"ValidationRules\": [{\"Type\": 2,\"WitnessName\": \"TslGame\\\\Binaries\\\\Win64\\\\TslGame.exe\"}],\"DisablingIntroductionActions\": [{\"Type\": 2,\"InitialName\": \"\\\\TslGame\\\\Content\\\\Movies\\\\LoadingScreen.mp4\"}],\"IllustrationPlatform\": 1,\"IllustrationId\": \"578080\"}";

        private const string File3 = "{\"Name\": \"Ghost Recon: Wildlands\",\"PublisherName\": \"Ubisoft\",\"SettingVersion\": null,\"PotentialRootFolderNames\": [ \"Tom Clancy's Ghost Recon Wildlands\" ],\"ValidationRules\": [{\"Type\": 2,\"WitnessName\": \"GRW.exe\"}],\"DisablingIntroductionActions\": [{\"Type\": 2,\"InitialName\": \"\\\\videos\\\\Nvidia.bk2\"},{\"Type\": 2,\"InitialName\": \"\\\\videos\\\\Ubisoft_Logo.bk2\"},{\"Type\": 2,\"InitialName\": \"\\\\videos\\\\VIDEO_EXPERIENCE.bk2\"},{\"Type\": 2,\"InitialName\": \"\\\\videos\\\\VIDEO_GLOBA_000.bk2\"},{\"Type\": 2,\"InitialName\": \"\\\\videos\\\\VIDEO_INTRO_GAM.bk2\"},{\"Type\": 2,\"InitialName\": \"\\\\videos\\\\TRC\\\\*\\\\WarningSaving.bk2\"},{\"Type\": 2,\"InitialName\": \"\\\\videos\\\\TRC\\\\*\\\\Epilepsy.bk2\"}],\"IllustrationPlatform\": 1,\"IllustrationId\": \"460930\"}";}
    
}

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Settings;

namespace VGIS.Domain.Repositories
{
    public interface IGameSettingsRepository
    {
        IEnumerable<GameSetting> GetAllGameSettings();
        void SaveNewGameSettings(GameSetting newGame);
        GameSetting CreateNewGameSetting(string gameName, string publisherName, string developerName, string platformFolder, string gameRootFolder, List<DisableIntroductionAction> disablingIntroductionActions, List<RootValidationRule> validationRules, IllustrationPlatformEnum illustrationPlatform, string illustrationId);
    }

    public class GameSettingsRepository : IGameSettingsRepository
    {
        private readonly string _defaultGameSettingsFilesPath;
        private readonly string _customGameSettingsFilesPath;
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public GameSettingsRepository(GlobalSettings settings, IFileSystemDal fileSystemDal)
        {
            _defaultGameSettingsFilesPath = settings.DefaultGamesSettingsFolder;
            _customGameSettingsFilesPath = settings.CustomGamesSettingsFolder;

            _fileSystemDal = fileSystemDal;

            if (!_fileSystemDal.DirectoryExists(_customGameSettingsFilesPath))
                _fileSystemDal.DirectoryCreate(_customGameSettingsFilesPath);
        }
        #endregion

        public IEnumerable<GameSetting> GetAllGameSettings()
        {
            var allDetectedGameSettings = new List<FileInfo>();
            allDetectedGameSettings.AddRange(_fileSystemDal.GetFiles(_defaultGameSettingsFilesPath));
            allDetectedGameSettings.AddRange(_fileSystemDal.GetFiles(_customGameSettingsFilesPath));

            foreach (var file in allDetectedGameSettings)
            {
                GameSetting data = null;
                try
                {
                    var jsonFileData = _fileSystemDal.ReadAllText(file.FullName);
                    data = JsonConvert.DeserializeObject<GameSetting>(jsonFileData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (data != null) yield return data;
            }
        }

        public void SaveNewGameSettings(GameSetting newGame)
        {
            var fileNamePattern = $"{_customGameSettingsFilesPath}{newGame.Name.Trim()}{{0}}.json";
            var fileName = GetAvailableFileNameInFolder(fileNamePattern, _customGameSettingsFilesPath);

            var json = JsonConvert.SerializeObject(newGame);
            _fileSystemDal.FileWriteAllText(fileName, json);
        }

        public GameSetting CreateNewGameSetting(string gameName, string publisherName, string developerName, string platformFolder, string gameRootFolder, List<DisableIntroductionAction> disablingIntroductionActions, List<RootValidationRule> validationRules, IllustrationPlatformEnum illustrationPlatform, string illustrationId)
        {
            // Input validation
            if(string.IsNullOrWhiteSpace(gameName)) throw new ArgumentException("gameName");
            if (!_fileSystemDal.DirectoryExists(platformFolder)) throw new ArgumentException("platformFolder");
            if (!_fileSystemDal.DirectoryExists(Path.Combine(platformFolder, gameRootFolder))) throw new ArgumentException("gameRootFolder");

            foreach (var action in disablingIntroductionActions)
            {
                var pathToCheck = Path.Combine(platformFolder.Trim('\\'), gameRootFolder.Trim('\\'), action.InitialName.Trim('\\'));
                switch (action.Type)
                {
                    case DisableActionTypeEnum.FileRename:
                        if (!_fileSystemDal.FileExists(pathToCheck)) throw new ArgumentException($"{pathToCheck} don't exists");
                        break;
                    case DisableActionTypeEnum.FolderRename:
                        if (!_fileSystemDal.DirectoryExists(pathToCheck)) throw new ArgumentException($"{pathToCheck} don't exists");
                        break;
                    default:
                        throw new NotImplementedException($"action {action.Type} not supported");
                }
            }
            
            //Entity creation
            var result = new GameSetting
            {
                Name = gameName,
                PublisherName = publisherName,
                DeveloperName = developerName,
                IllustrationPlatform = illustrationPlatform,
                IllustrationId = illustrationId,
                SettingVersion = new Version(0,0,1),
                PotentialRootFolderNames = new List<string>() { gameRootFolder },
                DisablingIntroductionActions = disablingIntroductionActions,
                ValidationRules = validationRules
            };
            return result;
        }

        private string GetAvailableFileNameInFolder(string pattern, string folderPath)
        {
            var iter = 0;
            var candidate = string.Format(pattern, "");

            while (_fileSystemDal.FileExists(candidate))
            {
                iter++;
                candidate = string.Format(pattern, $"_{iter}");
            }

            return candidate;
        }
    }
}
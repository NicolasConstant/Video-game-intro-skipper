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
    public class GameSettingsRepository
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

        public void AddNewGameSettings(GameSetting newGame)
        {
            var fileNamePattern = $"{_customGameSettingsFilesPath}{newGame.Name.Trim()}{{0}}.json";
            var fileName = GetAvailableFileNameInFolder(fileNamePattern, _customGameSettingsFilesPath);

            var json = JsonConvert.SerializeObject(newGame);
            _fileSystemDal.FileWriteAllText(fileName, json);
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
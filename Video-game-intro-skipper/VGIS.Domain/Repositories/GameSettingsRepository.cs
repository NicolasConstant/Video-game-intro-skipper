using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Repositories
{
    public class GameSettingsRepository
    {
        private readonly string _gameSettingsFilesPath;

        #region Ctor
        public GameSettingsRepository(string gameSettingsFilesPath)
        {
            _gameSettingsFilesPath = gameSettingsFilesPath;
        }
        #endregion

        public IEnumerable<GameSetting> GetAllGameSettings()
        {
            var dir = new DirectoryInfo(_gameSettingsFilesPath);
            var settingsFiles = dir.GetFiles();
            foreach (var file in settingsFiles)
            {
                GameSetting data = null;
                try
                {
                    var jsonFileData = File.ReadAllText(file.FullName);
                    data = JsonConvert.DeserializeObject<GameSetting>(jsonFileData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if(data != null) yield return data;
            }
        }
    }
}
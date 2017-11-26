﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Repositories
{
    public class GameSettingsRepository
    {
        private readonly string _gameSettingsFilesPath;
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public GameSettingsRepository(string gameSettingsFilesPath, IFileSystemDal fileSystemDal)
        {
            _gameSettingsFilesPath = gameSettingsFilesPath;
            _fileSystemDal = fileSystemDal;
        }
        #endregion

        public IEnumerable<GameSetting> GetAllGameSettings()
        {
            var settingsFiles = _fileSystemDal.GetFiles(_gameSettingsFilesPath);
            foreach (var file in settingsFiles)
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
                if(data != null) yield return data;
            }
        }
    }
}
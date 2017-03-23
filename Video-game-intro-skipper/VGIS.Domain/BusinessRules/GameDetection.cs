using System;
using System.Collections.Generic;
using System.IO;
using VGIS.Domain.Domain;

namespace VGIS.Domain.BusinessRules
{
    public class GameDetection
    {
        private readonly GameSetting _gameSetting;
        private readonly IEnumerable<DirectoryInfo> _installationDirectories;

        #region Ctor
        public GameDetection(GameSetting gameSetting, IEnumerable<DirectoryInfo> installationDirectories)
        {
            _gameSetting = gameSetting;
            _installationDirectories = installationDirectories;
        }
        #endregion

        public GameDetectionResult Run()
        {
            throw new NotImplementedException();
        }
    }

    public class GameDetectionResult
    {
        public int GameId { get; set; } //TODO 
        public string GameName { get; set; }
        public bool Detected { get; set; }
        public bool IsIntroductionDisabled { get; set; }
        public DirectoryInfo InstallationPath { get; set; }
    }
}
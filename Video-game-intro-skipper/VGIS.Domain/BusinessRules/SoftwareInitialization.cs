using System.Collections.Generic;
using System.Linq;
using VGIS.Domain.Domain;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.BusinessRules
{
    public class SoftwareInitialization
    {
        private readonly GameSettingsRepository _settingsRepository;
        private readonly InstallationDirectoriesRepository _installationDirectoryRepository;

        #region Ctor
        public SoftwareInitialization(GameSettingsRepository settingsRepository, InstallationDirectoriesRepository installationDirectoryRepository)
        {
            _settingsRepository = settingsRepository;
            _installationDirectoryRepository = installationDirectoryRepository;
        }
        #endregion

        public Dictionary<GameSetting, GameDetectionResult> Run()
        {
            //Load all game settings
            var gameSettings = _settingsRepository.GetAllGameSettings();

            //Load all repository to look at
            var installationRepositories = _installationDirectoryRepository.GetAllInstallationFolders().ToList();

            //Browse and detect installed games
            var gameDetectionResults = new Dictionary<GameSetting, GameDetectionResult>();
            foreach (var gameSetting in gameSettings)
            {
                var gameDetection = new GameDetection(gameSetting, installationRepositories);
                var gameDetectionResult = gameDetection.Run();

                gameDetectionResults.Add(gameSetting, gameDetectionResult);
            }

            //TODO Optimize with user settings, ...
            //TODO Return progress information for GUI

            return gameDetectionResults;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using VGIS.Domain.Domain;
using VGIS.Domain.Factories;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.BusinessRules
{
    internal class DetectAllGamesStatus
    {
        private readonly GameSettingsRepository _settingsRepository;
        private readonly InstallationDirectoriesRepository _installationDirectoryRepository;
        private readonly IGameFactory _gameFact;

        #region Ctor
        public DetectAllGamesStatus(GameSettingsRepository settingsRepository, InstallationDirectoriesRepository installationDirectoryRepository, IGameFactory gameFact)
        {
            _settingsRepository = settingsRepository;
            _installationDirectoryRepository = installationDirectoryRepository;
            _gameFact = gameFact;
        }
        #endregion

        public IEnumerable<Game> Execute()
        {
            //Load all game settings
            var gameSettings = _settingsRepository.GetAllGameSettings();

            //Load all repository to look at
            var installationRepositories = _installationDirectoryRepository.GetAllInstallationFolders().ToList();

            //Browse and detect installed games
            foreach (var gameSetting in gameSettings)
            {
                var gameDetection = new DetectGameStatus(gameSetting, installationRepositories);
                var gameDetectionResult = gameDetection.Execute();

                yield return _gameFact.GetGame(gameSetting, gameDetectionResult);
            }
        }
    }
}
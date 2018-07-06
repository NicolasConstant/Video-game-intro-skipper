using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Factories;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.Services
{
    public class GameService
    {
        private readonly GameFactory _gameFactory;
        private readonly GameSettingsRepository _gameSettingsRepository;
        private readonly InstallationDirectoriesRepository _installationDirRepository;
        private readonly IGameFactory _gameFact;
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public GameService(GameSettingsRepository gameSettingsRepository, InstallationDirectoriesRepository installationDirRepository, IGameFactory gameFact, IFileSystemDal fileSystemDal, GameFactory gameFactory)
        {
            _gameSettingsRepository = gameSettingsRepository;
            _installationDirRepository = installationDirRepository;
            _gameFact = gameFact;
            _fileSystemDal = fileSystemDal;
            _gameFactory = gameFactory;
        }
        #endregion

        public IEnumerable<Game> GetAllGames(bool returnOnlyDetectedGames)
        {
            var detectAllGamesStatus = new DetectAllGamesStatus(_gameSettingsRepository, _installationDirRepository, _gameFact, returnOnlyDetectedGames);
            return detectAllGamesStatus.Execute();
        }

        public Game GetGameFromSettings(GameSetting setting)
        {
            var allInstallDirs = _installationDirRepository.GetAllInstallationFolders();
            var detectStatusAction = new DetectGameStatus(setting, allInstallDirs);
            var gameDetectionResult = detectStatusAction.Execute();

            return _gameFactory.GetGame(setting, gameDetectionResult);
        }

        public GameSetting GetGameSetting(string gameName, string publisherName, string developerName, string platformFolder, string gameRootFolder, List<DisableIntroductionAction> disablingIntroductionActions,  IllustrationPlatformEnum platformType, string illustrationUrl)
        {
            //Determine if illustration is valid
            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var isValid = action.Execute();

            //Determine illustration Code
            var illustration = IllustrationPlatformEnum.Unknown;
            var illustrationCode = string.Empty;
            if (isValid)
            {
                var extractIllustrationCodeAction = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
                illustrationCode = extractIllustrationCodeAction.Execute();
                illustration = platformType;
            } 

            //Determine rootValidationRule
            var generateRootValidationRulesAction = new GenerateRootValidationRulesAction(_fileSystemDal);
            var validationRules = generateRootValidationRulesAction.Execute(Path.Combine(platformFolder, gameRootFolder)).ToList();

            //Create new game settings
            var generateNewGameSetting = new GenerateNewGameSettingAction(_gameSettingsRepository);
            return generateNewGameSetting.Execute(gameName, publisherName, developerName, platformFolder,
                gameRootFolder, disablingIntroductionActions, validationRules, illustration, illustrationCode);
        }

        /// <summary>
        /// Save the setting to a new settings file
        /// </summary>
        /// <param name="newGame">settings</param>
        /// <returns>Path to the file</returns>
        public string SaveNewGame(GameSetting newGame)
        {
            var addNewGameSettings = new AddNewGameSettings(_gameSettingsRepository, newGame);
            return addNewGameSettings.Execute();
        }
    }
}
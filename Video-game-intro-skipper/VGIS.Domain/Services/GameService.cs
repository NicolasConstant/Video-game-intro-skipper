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
        private readonly GameSettingsRepository _gameSettingsRepository;
        private readonly InstallationDirectoriesRepository _installationDirRepository;
        private readonly IGameFactory _gameFact;
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public GameService(GameSettingsRepository gameSettingsRepository, InstallationDirectoriesRepository installationDirRepository, IGameFactory gameFact, IFileSystemDal fileSystemDal)
        {
            _gameSettingsRepository = gameSettingsRepository;
            _installationDirRepository = installationDirRepository;
            _gameFact = gameFact;
            _fileSystemDal = fileSystemDal;
        }
        #endregion

        public IEnumerable<Game> GetAllGames()
        {
            var detectAllGamesStatus = new DetectAllGamesStatus(_gameSettingsRepository, _installationDirRepository, _gameFact);
            return detectAllGamesStatus.Execute();
        }

        public GameSetting GetGameSetting(string gameName, string publisherName, string developerName, string platformFolder, string gameRootFolder, List<DisableIntroductionAction> disablingIntroductionActions,  IllustrationPlatformEnum illustrationPlatform, string illustrationId)
        {
            //Determine rootValidationRule
            var generateRootValidationRulesAction = new GenerateRootValidationRulesAction(_fileSystemDal);
            var validationRules = generateRootValidationRulesAction.Execute(Path.Combine(platformFolder, gameRootFolder)).ToList();

            //Create new game settings
            var generateNewGameSetting = new GenerateNewGameSettingAction(_gameSettingsRepository);
            return generateNewGameSetting.Execute(gameName, publisherName, developerName, platformFolder,
                gameRootFolder, disablingIntroductionActions, validationRules, illustrationPlatform, illustrationId);
        }

        public void SaveNewGame(GameSetting newGame)
        {
            var addNewGameSettings = new AddNewGameSettings(_gameSettingsRepository, newGame);
            addNewGameSettings.Execute();
        }
    }
}
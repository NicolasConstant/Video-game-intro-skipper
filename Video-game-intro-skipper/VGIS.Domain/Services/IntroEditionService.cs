using System.Collections.Generic;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Domain;
using VGIS.Domain.Repositories;
using VGIS.Domain.Tools;

namespace VGIS.Domain.Services
{
    public class IntroEditionService
    {
        private readonly GameSettingsRepository _gameSettingsRepository;
        private readonly InstallationDirectoriesRepository _installationDirRepository;
        private readonly FileAndFolderRenamer _fileAndFolderRenamer;
        private readonly PathPatternTranslator _pathPatternTranslator;

        #region Ctor
        public IntroEditionService(GameSettingsRepository gameSettingsRepository, InstallationDirectoriesRepository installationDirRepository, FileAndFolderRenamer fileAndFolderRenamer, PathPatternTranslator pathPatternTranslator)
        {
            _gameSettingsRepository = gameSettingsRepository;
            _installationDirRepository = installationDirRepository;
            _fileAndFolderRenamer = fileAndFolderRenamer;
            _pathPatternTranslator = pathPatternTranslator;
        }
        #endregion

        public IEnumerable<Game> GetAllGames()
        {
            var detectAllGamesStatus = new DetectAllGamesStatus(_gameSettingsRepository, _installationDirRepository);
            return detectAllGamesStatus.Execute();
        }

        public void SaveNewGame(GameSetting newGame)
        {
            var addNewGameSettings = new AddNewGameSettings(_gameSettingsRepository, newGame);
            addNewGameSettings.Execute();
        }

        public void DisableIntro(Game targetedGame)
        {
            var disableIntro = new ApplyDisableIntroAction(targetedGame.Settings, targetedGame.DetectionResult, _fileAndFolderRenamer, _pathPatternTranslator);
            disableIntro.Execute();
        }

        public void ReenableIntro(Game targetedGame)
        {
            var reenableIntro = new ApplyReenableIntroAction(targetedGame.Settings, targetedGame.DetectionResult, _fileAndFolderRenamer, _pathPatternTranslator);
            reenableIntro.Execute();
        }
    }
}
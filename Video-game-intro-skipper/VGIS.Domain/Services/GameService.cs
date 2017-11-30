using System.Collections.Generic;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Domain;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.Services
{
    public class GameService
    {
        private readonly GameSettingsRepository _gameSettingsRepository;
        private readonly InstallationDirectoriesRepository _installationDirRepository;

        #region Ctor
        public GameService(GameSettingsRepository gameSettingsRepository, InstallationDirectoriesRepository installationDirRepository)
        {
            _gameSettingsRepository = gameSettingsRepository;
            _installationDirRepository = installationDirRepository;
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
    }
}
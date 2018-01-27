using System.Collections.Generic;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Domain;
using VGIS.Domain.Factories;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.Services
{
    public class GameService
    {
        private readonly GameSettingsRepository _gameSettingsRepository;
        private readonly InstallationDirectoriesRepository _installationDirRepository;
        private readonly IGameFactory _gameFact;

        #region Ctor
        public GameService(GameSettingsRepository gameSettingsRepository, InstallationDirectoriesRepository installationDirRepository, IGameFactory gameFact)
        {
            _gameSettingsRepository = gameSettingsRepository;
            _installationDirRepository = installationDirRepository;
            _gameFact = gameFact;
        }
        #endregion

        public IEnumerable<Game> GetAllGames()
        {
            var detectAllGamesStatus = new DetectAllGamesStatus(_gameSettingsRepository, _installationDirRepository, _gameFact);
            return detectAllGamesStatus.Execute();
        }

        public void SaveNewGame(GameSetting newGame)
        {
            var addNewGameSettings = new AddNewGameSettings(_gameSettingsRepository, newGame);
            addNewGameSettings.Execute();
        }
    }
}
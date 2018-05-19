using VGIS.Domain.BusinessRules.Bases;
using VGIS.Domain.Domain;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.BusinessRules
{
    public class AddNewGameSettings 
    {
        private readonly GameSettingsRepository _gameSettingsRepository;
        private readonly GameSetting _newGame;

        #region Ctor
        public AddNewGameSettings(GameSettingsRepository gameSettingsRepository, GameSetting newGame)
        {
            _gameSettingsRepository = gameSettingsRepository;
            _newGame = newGame;
        }
        #endregion

        public void Execute()
        {
            //TODO add crowdsourcing here

            _gameSettingsRepository.SaveNewGameSettings(_newGame);
        }
    }
}
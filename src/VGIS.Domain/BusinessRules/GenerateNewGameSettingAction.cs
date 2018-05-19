using System.Collections.Generic;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.BusinessRules
{
    public class GenerateNewGameSettingAction
    {
        private readonly IGameSettingsRepository _gameSettingsRepository;

        #region Ctor
        public GenerateNewGameSettingAction(IGameSettingsRepository gameSettingsRepository)
        {
            _gameSettingsRepository = gameSettingsRepository;
        }
        #endregion

        public GameSetting Execute(string gameName, string publisherName, string developerName, string platformFolder, string gameRootFolder, List<DisableIntroductionAction> disablingIntroductionActions, List<RootValidationRule> validationRules, IllustrationPlatformEnum illustrationPlatform, string illustrationId)
        {
            return _gameSettingsRepository.CreateNewGameSetting(gameName, publisherName, developerName, platformFolder, gameRootFolder, disablingIntroductionActions, validationRules,  illustrationPlatform,  illustrationId);
        }
    }
}
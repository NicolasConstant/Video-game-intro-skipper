using VGIS.Domain.BusinessRules;
using VGIS.Domain.Domain;

namespace VGIS.Domain.Factories
{
    public interface IGameFactory
    {
        Game GetGame(GameSetting settings, GameDetectionResult detectionResult);
    }

    public class GameFactory : IGameFactory
    {
        public Game GetGame(GameSetting settings, GameDetectionResult detectionResult)
        {
            //TODO: Create local cache of illustrations

            //Generate illustration
            var generateIllustrationAction = new ConstructIllustrationAction(settings.IllustrationPlatform, settings.IllustrationId);
            var illustration = generateIllustrationAction.Execute();

            //Create Game object
            var game = new Game(settings, detectionResult, illustration);
            return game;
        }
    }
}
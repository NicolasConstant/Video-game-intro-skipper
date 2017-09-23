using VGIS.Domain.Domain;

namespace VGIS.GUI.ViewModels
{
    public class GameViewModel
    {
        private readonly Game _game;

        #region Ctor
        public GameViewModel(Game game)
        {
            _game = game;
        }
        #endregion

        public string IllustrationUrl => _game.IllustrationUrl;
        public float Opacity => _game.IsDetected ? 1f : 0.2f;
    }
}
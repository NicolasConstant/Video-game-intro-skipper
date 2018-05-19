using System;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Domain
{
    public class Game
    {
        private readonly GameSetting _settings;
        private readonly GameDetectionResult _detectionResult;

        #region Ctor
        public Game(GameSetting settings, GameDetectionResult detectionResult, string illustrationUrl)
        {
            _settings = settings;
            _detectionResult = detectionResult;
            IllustrationUrl = illustrationUrl;
        }
        #endregion

        public GameSetting Settings => _settings;
        public GameDetectionResult DetectionResult => _detectionResult;
        
        public string Name => _settings.Name;
        public string Publisher => _settings.PublisherName;
        public string IllustrationUrl { get; }
        public bool IsDetected => _detectionResult.Detected;
        public IntroductionStateEnum State => _detectionResult.IntroductionState;
    }
}
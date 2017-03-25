using System;
using VGIS.Domain.Domain;

namespace VGIS.Domain.BusinessRules
{
    public class ApplyReenableIntroAction
    {
        private readonly GameSetting _settings;
        private readonly GameDetectionResult _detectionResult;

        #region Ctor
        public ApplyReenableIntroAction(GameSetting settings, GameDetectionResult detectionResult)
        {
            _settings = settings;
            _detectionResult = detectionResult;
        }
        #endregion

        public bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
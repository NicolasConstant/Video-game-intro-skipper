using System;
using VGIS.Domain.Domain;

namespace VGIS.Domain.BusinessRules
{
    public class ApplyDisableIntroAction
    {
        private readonly GameSetting _settings;
        private readonly GameDetectionResult _detectionResult;

        #region Ctor
        public ApplyDisableIntroAction(GameSetting settings, GameDetectionResult detectionResult)
        {
            _settings = settings;
            _detectionResult = detectionResult;
        }
        #endregion

        public bool Run()
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Services
{
    public class IllustrationValidationService
    {

        #region Ctor
        public IllustrationValidationService()
        {

        }
        #endregion

        public bool IsIllustrationValid(IllustrationPlatformEnum platformType, string illustrationUrl)
        {
            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            return action.Execute();
        }
    }
}
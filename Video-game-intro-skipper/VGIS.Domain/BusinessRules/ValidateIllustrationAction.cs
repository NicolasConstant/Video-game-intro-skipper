using System;
using System.Text.RegularExpressions;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    public class ValidateIllustrationAction
    {
        private readonly string _illustrationUrl;
        private readonly IllustrationPlatformEnum _platformType;

        #region Ctor
        public ValidateIllustrationAction(IllustrationPlatformEnum platformType, string illustrationUrl)
        {
            _illustrationUrl = illustrationUrl;
            _platformType = platformType;
        }
        #endregion

        public bool Execute()
        {
            switch (_platformType)
            {
                case IllustrationPlatformEnum.Gog:
                    throw new NotImplementedException();
                case IllustrationPlatformEnum.BattleNet:
                    throw new NotImplementedException();
                case IllustrationPlatformEnum.Origin:
                    throw new NotImplementedException();
                case IllustrationPlatformEnum.Steam:
                    return ValidateSteam(_illustrationUrl);
                case IllustrationPlatformEnum.Uplay:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();

            }


            throw new NotImplementedException();
        }

        private bool ValidateSteam(string illustrationUrl)
        {
            const string pattern = @"steamstatic.com/steam/apps/([0-9]+)/header.jpg";
            var reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(illustrationUrl);            
        }
    }
}
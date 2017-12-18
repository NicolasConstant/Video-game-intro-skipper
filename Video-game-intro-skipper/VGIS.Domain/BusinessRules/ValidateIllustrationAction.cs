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
                    return ValidateGog(_illustrationUrl);
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

        private bool ValidateGog(string illustrationUrl)
        {
            const string pattern = @"images-([0-9]+).gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_([a-zA-Z0-9]+(_)*)*.jpg";
            var reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(illustrationUrl);
        }

        private bool ValidateSteam(string illustrationUrl)
        {
            const string pattern = @"steamstatic.com/steam/apps/([0-9]+)/header.jpg";
            var reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(illustrationUrl);            
        }
    }
}
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
            if (string.IsNullOrWhiteSpace(_illustrationUrl)) return false;

            switch (_platformType)
            {
                case IllustrationPlatformEnum.Gog:
                    return ValidateGog(_illustrationUrl);
                case IllustrationPlatformEnum.BattleNet:
                    return ValidateBattleNet(_illustrationUrl);
                case IllustrationPlatformEnum.Origin:
                    return ValidateOrigin(_illustrationUrl);
                case IllustrationPlatformEnum.Steam:
                    return ValidateSteam(_illustrationUrl);
                case IllustrationPlatformEnum.Uplay:
                    return ValidateUplay(_illustrationUrl);
                case IllustrationPlatformEnum.Unknown:
                    return false;
                default:
                    throw new NotImplementedException();

            }
        }

        private bool ValidateBattleNet(string illustrationUrl)
        {
            const string pattern = @"akamaihd.net/cms/page_media/[a-zA-Z0-9]+.jpg";
            return ValidateRegex(pattern, illustrationUrl);
        }

        private bool ValidateOrigin(string illustrationUrl)
        {
            const string pattern = @"originassets.[a-zA-Z]+.net/origin-com-store-final-assets-prod/[0-9]+/[xX0-9\.]+/[0-9]+_LB_[xX0-9]+_[a-zA-Z]+_[a-zA-Z]+_[a-zA-Z0-9\%]+_[0-9\-]+_[a-zA-Z0-9]+.jpg";
            return ValidateRegex(pattern, illustrationUrl);
        }

        private bool ValidateUplay(string illustrationUrl)
        {
            const string pattern = @"store.ubi.com/dw/image/v2/ABBS_PRD/on/demandware.static/-/Sites-masterCatalog/default/dw266cd145/images/large/[a-zA-Z0-9]+.jpg";
            return ValidateRegex(pattern, illustrationUrl);
        }

        private bool ValidateGog(string illustrationUrl)
        {
            const string pattern = @"images-([0-9]+).gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_([a-zA-Z0-9]+(_)*)*.jpg";
            return ValidateRegex(pattern, illustrationUrl);
        }

        private bool ValidateSteam(string illustrationUrl)
        {
            const string pattern = @"steamstatic.com/steam/apps/([0-9]+)/header.jpg";
            return ValidateRegex(pattern, illustrationUrl);
        }

        private bool ValidateRegex(string pattern, string illustration)
        {
            var reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(illustration);
        }
    }
}
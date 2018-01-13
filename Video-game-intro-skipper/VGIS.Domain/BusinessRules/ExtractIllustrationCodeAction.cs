using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    public class ExtractIllustrationCodeAction
    {
        private readonly IllustrationPlatformEnum _platformType;
        private readonly string _illustrationUrl;

        #region Ctor
        public ExtractIllustrationCodeAction(IllustrationPlatformEnum platformType, string illustrationUrl)
        {
            _platformType = platformType;
            _illustrationUrl = illustrationUrl;
        }
        #endregion

        public string Execute()
        {
            switch (_platformType)
            {
                case IllustrationPlatformEnum.Gog:
                    return ExtractGog(_illustrationUrl);
                case IllustrationPlatformEnum.BattleNet:
                    return ExtractBattleNet(_illustrationUrl);
                case IllustrationPlatformEnum.Origin:
                    return ExtractOrigin(_illustrationUrl);
                case IllustrationPlatformEnum.Steam:
                    return ExtractSteam(_illustrationUrl);
                case IllustrationPlatformEnum.Uplay:
                    return ExtractUplay(_illustrationUrl);
                default:
                    throw new NotImplementedException();

            }
        }

        private string ExtractGog(string illustrationUrl)
        {
            const string pre = ".gog.com/";
            const string post = "_product";

            //Extract code
            var illustrationCode = ExtractIllustrationCode(illustrationUrl, pre, post);

            // Validate data
            const string pattern = @"^([a-zA-Z0-9]+)$";
            if (!ValidateRegex(pattern, illustrationCode))
                throw new Exception("extracted illustration code not valid");

            return illustrationCode;
        }

        private string ExtractBattleNet(string illustrationUrl)
        {
            throw new NotImplementedException();
        }

        private string ExtractOrigin(string illustrationUrl)
        {
            throw new NotImplementedException();
        }

        private string ExtractSteam(string illustrationUrl)
        {
            const string pre = "steamstatic.com/steam/apps/";
            const string post = "/header.jpg";

            //Extract code
            var illustrationCode = ExtractIllustrationCode(illustrationUrl, pre, post);

            // Validate data
            const string pattern = @"^([0-9]+)$";
            if (!ValidateRegex(pattern, illustrationCode))
                throw new Exception("extracted illustration code not valid");

            return illustrationCode;
        }

        private string ExtractUplay(string illustrationUrl)
        {
            const string pre = "demandware.static/-/Sites-masterCatalog/default/";
            const string post = ".jpg";

            //Extract global code
            var illustrationCodes = ExtractIllustrationCode(illustrationUrl, pre, post);

            //Extract two codes
            var splitedCodes = illustrationCodes.Split('/');
            var firstCode = splitedCodes[0];
            var secondCode = splitedCodes[3];

            // Validate data
            const string pattern = @"^([a-zA-Z0-9]+)$";
            if (!ValidateRegex(pattern, firstCode) || !ValidateRegex(pattern, secondCode))
                throw new Exception("extracted illustration code not valid");

            return $"{firstCode}-{secondCode}";
        }

        private bool ValidateRegex(string pattern, string illustration)
        {
            var reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(illustration);
        }

        private string ExtractIllustrationCode(string illustrationUrl, string pre, string post)
        {
            // Remove preposition
            var prePosition = illustrationUrl.LastIndexOf(pre, StringComparison.InvariantCultureIgnoreCase);
            var illustrationUrlWtPre = illustrationUrl.Substring(prePosition + pre.Length);

            // Remove postposition
            var postPosition = illustrationUrlWtPre.IndexOf(post, StringComparison.InvariantCultureIgnoreCase);
            var illustrationCode = illustrationUrlWtPre.Substring(0, postPosition);
            return illustrationCode;
        }
    }
}

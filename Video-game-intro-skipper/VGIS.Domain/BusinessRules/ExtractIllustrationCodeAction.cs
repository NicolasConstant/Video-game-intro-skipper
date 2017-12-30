﻿using System;
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
            throw new NotImplementedException();
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

            // Remove preposition
            var prePosition = illustrationUrl.LastIndexOf(pre, StringComparison.InvariantCultureIgnoreCase);
            var illustrationUrlWtPre = illustrationUrl.Substring(prePosition + pre.Length);

            // Remove postposition
            var postPosition = illustrationUrlWtPre.IndexOf(post, StringComparison.InvariantCultureIgnoreCase);
            var illustrationCode = illustrationUrlWtPre.Substring(0, postPosition);

            // Validate data
            const string pattern = @"^([0-9]+)$";
            if (!ValidateRegex(pattern, illustrationCode))
                throw new ArgumentException("illustration url not valid");

            return illustrationCode;
        }

        private string ExtractUplay(string illustrationUrl)
        {
            throw new NotImplementedException();
        }

        private bool ValidateRegex(string pattern, string illustration)
        {
            var reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(illustration);
        }
    }
}

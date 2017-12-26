using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }

        private string ExtractUplay(string illustrationUrl)
        {
            throw new NotImplementedException();
        }
    }
}

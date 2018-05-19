using System;
using VGIS.Domain.Enums;
using VGIS.Domain.Tools;

namespace VGIS.Domain.BusinessRules
{
    public class ConstructIllustrationAction
    {
        private readonly string _illustrationUrl;
        private readonly IllustrationPlatformEnum _platformType;

        #region Ctor
        public ConstructIllustrationAction(IllustrationPlatformEnum platformType, string illustrationUrl)
        {
            _illustrationUrl = illustrationUrl;
            _platformType = platformType;
        }
        #endregion

        public string Execute()
        {
            switch (_platformType)
            {
                case IllustrationPlatformEnum.Gog:
                    return GenerateGogIllustration(_illustrationUrl);
                case IllustrationPlatformEnum.BattleNet:
                    return GenerateBattleNetIllustration(_illustrationUrl);
                case IllustrationPlatformEnum.Origin:
                    return GenerateOriginIllustration(_illustrationUrl);
                case IllustrationPlatformEnum.Steam:
                    return GenerateSteamIllustration(_illustrationUrl);
                case IllustrationPlatformEnum.Uplay:
                    return GenerateUplayIllustration(_illustrationUrl);
                case IllustrationPlatformEnum.Unknown:
                    return string.Empty;
                default:
                    throw new NotImplementedException();

            }
        }

        private string GenerateGogIllustration(string illustrationUrl)
        {
            return "https://" + $"images-1.gog.com/{illustrationUrl}_product_quartet_250_2x.jpg";
        }

        private string GenerateBattleNetIllustration(string illustrationUrl)
        {
            return "https://" + $"bnetcmsus-a.akamaihd.net/cms/page_media/{illustrationUrl}.jpg";
        }

        private string GenerateOriginIllustration(string illustrationUrl)
        {
            return "https://" + $"originassets.akamaized.net/origin-com-store-final-assets-prod/{illustrationUrl}.jpg";
        }

        private string GenerateSteamIllustration(string illustrationUrl)
        {
            var epochNow = EpochHandler.GenerateEpochNow();
            return "http://" + $"cdn.edgecast.steamstatic.com/steam/apps/{illustrationUrl}/header.jpg?t={epochNow}";
        }

        private string GenerateUplayIllustration(string illustrationUrl)
        {
            var ids = illustrationUrl.Split('-');

            return "http://" + $"store.ubi.com/dw/image/v2/ABBS_PRD/on/demandware.static/-/Sites-masterCatalog/default/{ids[0]}/images/large/{ids[1]}.jpg";
        }
    }
}
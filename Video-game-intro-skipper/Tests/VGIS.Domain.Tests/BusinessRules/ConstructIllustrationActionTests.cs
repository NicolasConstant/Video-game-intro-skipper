using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Enums;
using VGIS.Domain.Tools;

namespace VGIS.Domain.Tests.BusinessRules
{
    [TestClass]
    public class ConstructIllustrationActionTests
    {
        [TestMethod]
        public void GenerateSteam_ValidPattern()
        {
            const string illustrationUrl = "110800";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ConstructIllustrationAction(platformType, illustrationUrl);
            var url = action.Execute();

            #region Validate
            var epoch = EpochHandler.GenerateEpochNow() - 60;
            Assert.AreEqual("http://cdn.edgecast.steamstatic.com/steam/apps/110800/header.jpg", url.Split(new []{ "?t=" }, StringSplitOptions.RemoveEmptyEntries).First());
            Assert.IsTrue(epoch < int.Parse(url.Split(new []{ "?t=" }, StringSplitOptions.RemoveEmptyEntries).Last()));
            #endregion
        }
        
        [TestMethod]
        public void ExtractGog_ValidPattern()
        {
            const string illustrationUrl = "b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ConstructIllustrationAction(platformType, illustrationUrl);
            var url = action.Execute();

            #region Validate
            Assert.AreEqual("https://images-1.gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_product_quartet_250_2x.jpg", url);
            #endregion
        }

        [TestMethod]
        public void ExtractUplay_ValidPattern()
        {
            const string illustrationUrl = "dw266cd145-584543894e01656a168b4567";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Uplay;

            var action = new ConstructIllustrationAction(platformType, illustrationUrl);
            var url = action.Execute();

            #region Validate
            Assert.AreEqual("http://store.ubi.com/dw/image/v2/ABBS_PRD/on/demandware.static/-/Sites-masterCatalog/default/dw266cd145/images/large/584543894e01656a168b4567.jpg", url);
            #endregion
        }

        [TestMethod]
        public void ExtractOrigin_ValidPattern()
        {
            const string illustrationUrl = "193632/231.0x326.0/1047228_LB_231x326_en_US_%5E_2017-05-26-22-43-31_4a0f2ef46a1183b885840fb8d0a7b7cc795b4a9f";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Origin;

            var action = new ConstructIllustrationAction(platformType, illustrationUrl);
            var url = action.Execute();

            #region Validate
            Assert.AreEqual("https://originassets.akamaized.net/origin-com-store-final-assets-prod/193632/231.0x326.0/1047228_LB_231x326_en_US_%5E_2017-05-26-22-43-31_4a0f2ef46a1183b885840fb8d0a7b7cc795b4a9f.jpg", url);
            #endregion
        }

        [TestMethod]
        public void ExtractBattleNet_ValidPattern()
        {
            const string illustrationUrl = "BZ5PE09UZVHF1506441173647";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.BattleNet;

            var action = new ConstructIllustrationAction(platformType, illustrationUrl);
            var url = action.Execute();

            #region Validate
            Assert.AreEqual("https://bnetcmsus-a.akamaihd.net/cms/page_media/BZ5PE09UZVHF1506441173647.jpg", url);
            #endregion
        }
    }
}
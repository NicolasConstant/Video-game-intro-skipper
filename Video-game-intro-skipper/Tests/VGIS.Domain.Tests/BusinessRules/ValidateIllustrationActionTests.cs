using System.Collections;
using System.Security.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Tests.BusinessRules
{
    [TestClass]
    public class ValidateIllustrationActionTests
    {

        [TestMethod]
        public void ValidateSteam_ValidPattern()
        {
            const string illustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/110800/header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }

        [TestMethod]
        public void ValidateSteam_InvalidPattern_Alphanumeric()
        {
            const string illustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/11df00/header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateSteam_InvalidPattern_HasNotValue()
        {
            const string illustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/apps//header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateGog_ValidPattern()
        {
            const string illustrationUrl = "https://images-1.gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_product_quartet_250_2x.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }

        [TestMethod]
        public void ValidateGog_HasNotValue()
        {
            const string illustrationUrl = "https://images-1.gog.com/_product_quartet_250_2x.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateGog_HasNotSubValue()
        {
            const string illustrationUrl = "https://images-1.gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }
        
        [TestMethod]
        public void ValidateUplay_ValidPattern()
        {
            const string illustrationUrl = "http://store.ubi.com/dw/image/v2/ABBS_PRD/on/demandware.static/-/Sites-masterCatalog/default/dw266cd145/images/large/584543894e01656a168b4567.jpg?sw=192&sh=245&sm=fit";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Uplay;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }

        [TestMethod]
        public void ValidateUplay_HasNotValue()
        {
            const string illustrationUrl = "http://store.ubi.com/dw/image/v2/ABBS_PRD/on/demandware.static/-/Sites-masterCatalog/default/dw266cd145/images/large/.jpg?sw=192&sh=245&sm=fit";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Uplay;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateOrigin_ValidPattern()
        {
            const string illustrationUrl = "https://originassets.akamaized.net/origin-com-store-final-assets-prod/193632/231.0x326.0/1047228_LB_231x326_en_US_%5E_2017-05-26-22-43-31_4a0f2ef46a1183b885840fb8d0a7b7cc795b4a9f.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Origin;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }

        [TestMethod]
        public void ValidateOrigin_HasNotValue()
        {
            const string illustrationUrl = "https://originassets.akamaized.net/origin-com-store-final-assets-prod/193632/231.0x326.0/1047228_LB_231x326_en_US_%5E_2017-05-26-22-43-.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Origin;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateBattleNet_ValidPattern()
        {
            const string illustrationUrl = "http://bnetproduct-a.akamaihd.net//f84/7d453e354c9df8ca335ad45da020704c-prod-card-tall.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.BattleNet;

            var action = new ValidateIllustrationAction(platformType, illustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }


    }
}
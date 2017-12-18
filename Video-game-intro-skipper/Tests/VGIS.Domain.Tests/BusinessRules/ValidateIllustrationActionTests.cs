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
            const string steamIllustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/110800/header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ValidateIllustrationAction(platformType, steamIllustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }

        [TestMethod]
        public void ValidateSteam_InvalidPattern_Alphanumeric()
        {
            const string steamIllustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/11df00/header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ValidateIllustrationAction(platformType, steamIllustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateSteam_InvalidPattern_HasNotValue()
        {
            const string steamIllustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/apps//header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ValidateIllustrationAction(platformType, steamIllustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateGog_ValidPattern()
        {
            const string steamIllustrationUrl = "https://images-1.gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_product_quartet_250_2x.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ValidateIllustrationAction(platformType, steamIllustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }

        [TestMethod]
        public void ValidateGog_HasNotValue()
        {
            const string steamIllustrationUrl = "https://images-1.gog.com/_product_quartet_250_2x.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ValidateIllustrationAction(platformType, steamIllustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(false, result);
            #endregion
        }

        [TestMethod]
        public void ValidateGog_HasNotSubValue()
        {
            const string steamIllustrationUrl = "https://images-1.gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ValidateIllustrationAction(platformType, steamIllustrationUrl);
            var result = action.Execute();

            #region Validate
            Assert.AreEqual(true, result);
            #endregion
        }

    }
}
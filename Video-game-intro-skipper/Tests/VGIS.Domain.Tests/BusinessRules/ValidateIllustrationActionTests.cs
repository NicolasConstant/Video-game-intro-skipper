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


    }
}
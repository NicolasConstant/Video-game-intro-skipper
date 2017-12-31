using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Tests.BusinessRules
{
    [TestClass]
    public class ExtractIllustrationCodeActionTests
    {
        [TestMethod]
        public void ExtractSteam_ValidPattern()
        {
            const string illustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/110800/header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            var code = action.Execute();

            #region Validate
            Assert.AreEqual("110800", code);
            #endregion
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExtractSteam_UnvalidPattern()
        {
            const string illustrationUrl = "http://cdn.edgecast.steamstatic.com/steam/aPPpps/110800/header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            action.Execute();
        }
    }
}

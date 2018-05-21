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
            const string illustrationUrl =
                "http://cdn.edgecast.steamstatic.com/steam/apps/110800/header.jpg?t=1482775022";
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
            const string illustrationUrl =
                "http://cdn.edgecast.steamstatic.com/steam/aPPpps/110800/header.jpg?t=1482775022";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Steam;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            action.Execute();
        }

        [TestMethod]
        public void ExtractGog_ValidPattern()
        {
            const string illustrationUrl =
                "https://images-1.gog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_product_quartet_250_2x.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;
            
            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            var code = action.Execute();

            #region Validate
            Assert.AreEqual("b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42", code);
            #endregion
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExtractGog_UnvalidPattern()
        {
            const string illustrationUrl =
                "https://images-1.gogog.com/b509eebef606ff5cebde31c74e31b01352e9c347e60afaefacff8924b1111b42_product_quartet_250_2x.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Gog;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            action.Execute();
        }

        [TestMethod]
        public void ExtractUplay_ValidPattern()
        {
            const string illustrationUrl = "http://store.ubi.com/dw/image/v2/ABBS_PRD/on/demandware.static/-/Sites-masterCatalog/default/dw266cd145/images/large/584543894e01656a168b4567.jpg?sw=192&sh=245&sm=fit";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Uplay;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            var code = action.Execute();

            #region Validate
            Assert.AreEqual("dw266cd145-584543894e01656a168b4567", code);
            #endregion
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExtractUplay_UnvalidPattern()
        {
            const string illustrationUrl = "http://store.ubi.com/dw/image/v2/ABBS_PRD/on/demandware.static/-/Sites-mastertalog/default/dw266cd145/images/large/584543894e01656a168b4567.jpg?sw=192&sh=245&sm=fit";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Uplay;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            action.Execute();
        }

        [TestMethod]
        public void ExtractOrigin_ValidPattern()
        {
            const string illustrationUrl = "https://originassets.akamaized.net/origin-com-store-final-assets-prod/193632/231.0x326.0/1047228_LB_231x326_en_US_%5E_2017-05-26-22-43-31_4a0f2ef46a1183b885840fb8d0a7b7cc795b4a9f.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Origin;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            var code = action.Execute();

            #region Validate
            Assert.AreEqual("193632/231.0x326.0/1047228_LB_231x326_en_US_%5E_2017-05-26-22-43-31_4a0f2ef46a1183b885840fb8d0a7b7cc795b4a9f", code);
            #endregion
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExtractOrigin_UnvalidPattern()
        {
            const string illustrationUrl = "https://originassets.akamaized.net/origin-com-store-final-ass00ets-prod/193632/231.0x326.0/1047228_LB_231x326_en_US_%5E_2017-05-26-22-43-31_4a0f2ef46a1183b885840fb8d0a7b7cc795b4a9f.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.Origin;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            action.Execute();
        }

        [TestMethod]
        public void ExtractBattleNet_ValidPattern()
        {
            const string illustrationUrl = "https://bnetcmsus-a.akamaihd.net/cms/page_media/BZ5PE09UZVHF1506441173647.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.BattleNet;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            var code = action.Execute();

            #region Validate
            Assert.AreEqual("BZ5PE09UZVHF1506441173647", code);
            #endregion
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExtractBattleNet_UnvalidPattern()
        {
            const string illustrationUrl = "https://bnetcmsus-a.akamaihd.net/cms/page_me0dia/BZ5PE09UZVHF1506441173647.jpg";
            const IllustrationPlatformEnum platformType = IllustrationPlatformEnum.BattleNet;

            var action = new ExtractIllustrationCodeAction(platformType, illustrationUrl);
            action.Execute();
        }
    }
}

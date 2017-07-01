using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Consts;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Tools;

namespace VGIS.Domain.Tests.Tools
{
    [TestClass]
    public class PathPatternTranslatorTests
    {
        [TestMethod]
        public void PatternWoNavigation()
        {
            //Set constants
            const string pattern = @"C:\Temp\videos\Nvidia.bk2";
            
            //Set Mocks
            var directoryBrowser = MockRepository.GenerateMock<IDirectoryBrowser>();

            //Run test
            var businessRule = new PathPatternTranslator(directoryBrowser);
            var result = businessRule.GetPathFromPattern(pattern).ToList();

            //Validate 
            directoryBrowser.VerifyAllExpectations();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(pattern, result.First());
        }
        
        [TestMethod]
        public void PatternWtNavigation()
        {
            //Set constants
            const string pattern = @"C:\Temp\videos\*\Nvidia.bk2";
            
            //Set Mocks
            var directoryBrowser = MockRepository.GenerateMock<IDirectoryBrowser>();
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos"))).Return(new[] { @"C:\Temp\videos\sub1", @"C:\Temp\videos\sub2" });
            
            //Run test
            var businessRule = new PathPatternTranslator(directoryBrowser);
            var result = businessRule.GetPathFromPattern(pattern).ToList();

            //Validate 
            directoryBrowser.VerifyAllExpectations();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub1\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub2\Nvidia.bk2"));
        }

        [TestMethod]
        public void PatternWtComplexNavigation()
        {
            //Set constants
            const string pattern = @"C:\Temp\videos\*\*\dummy\second\*\Nvidia.bk2";

            //Set Mocks
            var directoryBrowser = MockRepository.GenerateMock<IDirectoryBrowser>();
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos"))).Return(new[] { @"C:\Temp\videos\sub1", @"C:\Temp\videos\sub2" });
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos\sub1"))).Return(new[] { @"C:\Temp\videos\sub1\data1", @"C:\Temp\videos\sub1\data2" });
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos\sub2"))).Return(new[] { @"C:\Temp\videos\sub2\data3", @"C:\Temp\videos\sub2\data4" });
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos\sub1\data1\dummy\second"))).Return(new[] { @"C:\Temp\videos\sub1\data1\dummy\second\third1", @"C:\Temp\videos\sub1\data1\dummy\second\third2" });
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos\sub1\data2\dummy\second"))).Return(new[] { @"C:\Temp\videos\sub1\data2\dummy\second\third3", @"C:\Temp\videos\sub1\data2\dummy\second\third4" });
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos\sub2\data3\dummy\second"))).Return(new[] { @"C:\Temp\videos\sub2\data3\dummy\second\third5", @"C:\Temp\videos\sub2\data3\dummy\second\third6" });
            directoryBrowser.Expect(x => x.GetSubDirectories(Arg<string>.Matches(y => y == @"C:\Temp\videos\sub2\data4\dummy\second"))).Return(new[] { @"C:\Temp\videos\sub2\data4\dummy\second\third7", @"C:\Temp\videos\sub2\data4\dummy\second\third8" });

            //Run test
            var businessRule = new PathPatternTranslator(directoryBrowser);
            var result = businessRule.GetPathFromPattern(pattern).ToList();

            //Validate 
            directoryBrowser.VerifyAllExpectations();
            Assert.AreEqual(8, result.Count);
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub1\data1\dummy\second\third1\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub1\data1\dummy\second\third2\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub1\data2\dummy\second\third3\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub1\data2\dummy\second\third4\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub2\data3\dummy\second\third5\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub2\data3\dummy\second\third6\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub2\data4\dummy\second\third7\Nvidia.bk2"));
            Assert.IsTrue(result.Contains(@"C:\Temp\videos\sub2\data4\dummy\second\third8\Nvidia.bk2"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongPattern()
        {
            //Set constants
            const string pattern = @"C:\Temp\v*id**eos\Nvidia.bk2";

            //Set Mocks
            var directoryBrowser = MockRepository.GenerateMock<IDirectoryBrowser>();

            //Run test
            var businessRule = new PathPatternTranslator(directoryBrowser);
            businessRule.GetPathFromPattern(pattern);
        }
    }
}
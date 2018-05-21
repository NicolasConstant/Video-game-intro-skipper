using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Consts;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Tools;

namespace VGIS.Domain.Tests.BusinessRules
{
    [TestClass]
    public class ApplyDisableIntroActionTests
    {
        [TestMethod]
        public void SingleFileToRename()
        {
            //Set constants
            const string installPath = @"C:\Temp";
            const string fileToRename = @"\videos\Nvidia.bk2";

            //Set dependancies
            var settings = new GameSetting()
            {
                DisablingIntroductionActions = new List<DisableIntroductionAction>()
                {
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = fileToRename,
                    }
                }
            };

            var gameDetectionResult = new GameDetectionResult("wat")
            {
                InstallationPath = new DirectoryInfo(installPath),
                Detected = true,
                IntroductionState = IntroductionStateEnum.Enabled
            };

            //Set Mocks
            var fileAndFolderRenamerMock = MockRepository.GenerateMock<IFileAndFolderRenamer>();
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal($"{installPath}{fileToRename}"), Arg<string>.Is.Equal($"{installPath}{fileToRename}{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));

            var pathPatternTranslatorMock = MockRepository.GenerateMock<IPathPatternTranslator>();
            pathPatternTranslatorMock.Expect(x => x.GetPathFromPattern($"{installPath}{fileToRename}")).Return(new []{ $"{installPath}{fileToRename}" });

            //Run test
            var businessRule = new ApplyDisableIntroAction(settings, gameDetectionResult, fileAndFolderRenamerMock, pathPatternTranslatorMock);
            businessRule.Execute();

            //Validate 
            fileAndFolderRenamerMock.VerifyAllExpectations();
            pathPatternTranslatorMock.VerifyAllExpectations();
        }

        [TestMethod]
        public void MultipleFilesToRename()
        {
            //Set constants
            const string installPath = @"C:\Temp";
            const string fileToRename1 = @"\videos\Nvidia.bk2";
            const string fileToRename2 = @"\videos\SubFolder\Intro.bk2";

            //Set dependancies
            var settings = new GameSetting()
            {
                DisablingIntroductionActions = new List<DisableIntroductionAction>()
                {
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = fileToRename1,
                    },
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = fileToRename2,
                    }
                }
            };
            var gameDetectionResult = new GameDetectionResult("wat")
            {
                InstallationPath = new DirectoryInfo(installPath),
                Detected = true,
                IntroductionState = IntroductionStateEnum.Enabled
            };

            //Set Mocks
            var fileAndFolderRenamerMock = MockRepository.GenerateMock<IFileAndFolderRenamer>();
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal($"{installPath}{fileToRename1}"), Arg<string>.Is.Equal($"{installPath}{fileToRename1}{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal($"{installPath}{fileToRename2}"), Arg<string>.Is.Equal($"{installPath}{fileToRename2}{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));

            var pathPatternTranslatorMock = MockRepository.GenerateMock<IPathPatternTranslator>();
            pathPatternTranslatorMock.Expect(x => x.GetPathFromPattern($"{installPath}{fileToRename1}")).Return(new[] { $"{installPath}{fileToRename1}" });
            pathPatternTranslatorMock.Expect(x => x.GetPathFromPattern($"{installPath}{fileToRename2}")).Return(new[] { $"{installPath}{fileToRename2}" });

            //Run test
            var businessRule = new ApplyDisableIntroAction(settings, gameDetectionResult, fileAndFolderRenamerMock, pathPatternTranslatorMock);
            businessRule.Execute();

            //Validate 
            fileAndFolderRenamerMock.VerifyAllExpectations();
        }

        [TestMethod]
        public void SingleFileToRenameWtNavigation()
        {
            //Set constants
            const string installPath = @"C:\Temp";
            const string fileToRename = @"\videos\*\Nvidia.bk2";

            //Set dependancies
            var settings = new GameSetting()
            {
                DisablingIntroductionActions = new List<DisableIntroductionAction>()
                {
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = fileToRename,
                    }
                }
            };

            var gameDetectionResult = new GameDetectionResult("wat")
            {
                InstallationPath = new DirectoryInfo(installPath),
                Detected = true,
                IntroductionState = IntroductionStateEnum.Enabled
            };

            //Set Mocks
            var pathPatternTranslatorMock = MockRepository.GenerateMock<IPathPatternTranslator>();
            pathPatternTranslatorMock.Expect(x => x.GetPathFromPattern($"{installPath}{fileToRename}")).Return(new[] { @"C:\Temp\videos\sub1\Nvidia.bk2", @"C:\Temp\videos\sub2\Nvidia.bk2" });

            var fileAndFolderRenamerMock = MockRepository.GenerateMock<IFileAndFolderRenamer>();
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal(@"C:\Temp\videos\sub1\Nvidia.bk2"), Arg<string>.Is.Equal($@"C:\Temp\videos\sub1\Nvidia.bk2{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal(@"C:\Temp\videos\sub2\Nvidia.bk2"), Arg<string>.Is.Equal($@"C:\Temp\videos\sub2\Nvidia.bk2{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));

            //Run test
            var businessRule = new ApplyDisableIntroAction(settings, gameDetectionResult, fileAndFolderRenamerMock, pathPatternTranslatorMock);
            businessRule.Execute();

            //Validate 
            pathPatternTranslatorMock.VerifyAllExpectations();
            fileAndFolderRenamerMock.VerifyAllExpectations();
        }

        [TestMethod]
        public void SingleFileToRenameWtComplexNavigation()
        {
            //Set constants
            const string installPath = @"C:\Temp";
            const string fileToRename = @"\videos\*\data\second\*\Nvidia.bk2";

            //Set dependancies
            var settings = new GameSetting()
            {
                DisablingIntroductionActions = new List<DisableIntroductionAction>()
                {
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = fileToRename,
                    }
                }
            };

            var gameDetectionResult = new GameDetectionResult("wat")
            {
                InstallationPath = new DirectoryInfo(installPath),
                Detected = true,
                IntroductionState = IntroductionStateEnum.Enabled
            };

            //Set Mocks
            var pathPatternTranslatorMock = MockRepository.GenerateMock<IPathPatternTranslator>();
            pathPatternTranslatorMock.Expect(x => x.GetPathFromPattern($"{installPath}{fileToRename}")).Return(new[]
            {
                @"C:\Temp\videos\sub1\data\second\third1\Nvidia.bk2",
                @"C:\Temp\videos\sub1\data\second\third2\Nvidia.bk2",
                @"C:\Temp\videos\sub2\data\second\third3\Nvidia.bk2",
                @"C:\Temp\videos\sub2\data\second\third4\Nvidia.bk2"
            });

            var fileAndFolderRenamerMock = MockRepository.GenerateMock<IFileAndFolderRenamer>();
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal(@"C:\Temp\videos\sub1\data\second\third1\Nvidia.bk2"), Arg<string>.Is.Equal($@"C:\Temp\videos\sub1\data\second\third1\Nvidia.bk2{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal(@"C:\Temp\videos\sub1\data\second\third2\Nvidia.bk2"), Arg<string>.Is.Equal($@"C:\Temp\videos\sub1\data\second\third2\Nvidia.bk2{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal(@"C:\Temp\videos\sub2\data\second\third3\Nvidia.bk2"), Arg<string>.Is.Equal($@"C:\Temp\videos\sub2\data\second\third3\Nvidia.bk2{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));
            fileAndFolderRenamerMock.Expect(x => x.RenameFile(Arg<string>.Is.Equal(@"C:\Temp\videos\sub2\data\second\third4\Nvidia.bk2"), Arg<string>.Is.Equal($@"C:\Temp\videos\sub2\data\second\third4\Nvidia.bk2{GlobalNamesStruct.RenameSuffix}"), Arg<bool>.Is.Anything));

            //Run test
            var businessRule = new ApplyDisableIntroAction(settings, gameDetectionResult, fileAndFolderRenamerMock, pathPatternTranslatorMock);
            businessRule.Execute();

            //Validate 
            pathPatternTranslatorMock.VerifyAllExpectations();
            fileAndFolderRenamerMock.VerifyAllExpectations();
        }
    }
}

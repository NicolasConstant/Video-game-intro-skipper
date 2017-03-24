using System;
using System.Collections.Generic;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Repositories
{
    public class GameSettingsRepository
    {
        public IEnumerable<GameSetting> GetAllGameSettings()
        {
            yield return new GameSetting()
            {
                Name = "Ghost Recon: Wildlands",
                PublisherName = "Ubisoft",
                PotentialRootFolderNames = new List<string>
                {
                    "Tom Clancy's Ghost Recon Wildlands",
                },
                ValidationRules = new List<RootValidationRule>
                {
                    new RootValidationRule
                    {
                        Type = RootValidationTypeEnum.FileValidation,
                        WitnessName = "GRW.exe"
                    }
                },
                DisablingIntroductionActions = new List<DisableIntroductionAction>()
                {
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = "\\videos\\Nvidia.bk2",
                    },
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = "\\videos\\Ubisoft_Logo.bk2",
                    },
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = "\\videos\\VIDEO_EXPERIENCE.bk2",
                    },
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = "\\videos\\VIDEO_GLOBA_000.bk2",
                    },
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = "\\videos\\VIDEO_INTRO_GAM.bk2",
                    },
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = "\\videos\\TRC\\004_English\\WarningSaving.bk2",
                    },
                    new DisableIntroductionAction()
                    {
                        Type = DisableActionTypeEnum.FileRename,
                        InitialName = "\\videos\\TRC\\004_English\\Epilepsy.bk2",
                    },
                }
            };
        }
    }
}
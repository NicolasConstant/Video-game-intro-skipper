using System;
using System.Collections.Generic;
using VGIS.Domain.Enums;

namespace VGIS.Domain.Domain
{
    public class GameSetting
    {
        public string Name { get; set; }
        public string PublisherName { get; set; }
        public Version SettingVersion { get; set; }
        public List<string> PotentialRootFolderNames { get; set; }
        public List<RootValidationRule> ValidationRules { get; set; }
        public List<DisableIntroductionAction> DisablingIntroductionActions { get; set; }
        public IllustrationPlatformEnum IllustrationPlatform { get; set; }
        public string IllustrationId { get; set; }
    }
}
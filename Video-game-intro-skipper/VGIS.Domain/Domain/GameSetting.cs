using System;
using System.Collections.Generic;

namespace VGIS.Domain.Domain
{
    public class GameSetting
    {
        public string Name { get; set; }
        public string PublisherName { get; set; }
        public Version SettingVersion { get; set; }
        public List<string> RootFolderNames { get; set; }
        public List<RootValidationRule> ValidationRules { get; set; }
        public List<DisableIntroductionAction> DisablingIntroductionActions { get; set; }
    }
}
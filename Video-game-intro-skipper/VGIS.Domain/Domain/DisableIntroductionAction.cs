using VGIS.Domain.Enums;

namespace VGIS.Domain.Domain
{
    public class DisableIntroductionAction
    {
        public DisableActionTypeEnum Type { get; set; }
        public string InitialName { get; set; }
        public string TargetName { get; set; }
    }
}
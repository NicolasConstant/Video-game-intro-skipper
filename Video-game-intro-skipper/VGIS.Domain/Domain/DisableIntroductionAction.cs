namespace VGIS.Domain.Domain
{
    public class DisableIntroductionAction
    {
        public DisableActionType Type { get; set; }
    }

    public enum DisableActionType
    {
        FolderRename,
        FileRename, 
        ShortcutEdition,
    }
}
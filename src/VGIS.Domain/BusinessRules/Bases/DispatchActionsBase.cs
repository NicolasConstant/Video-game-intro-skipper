using System;
using System.IO;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Tools;

namespace VGIS.Domain.BusinessRules.Bases
{
    public abstract class DispatchActionsBase
    {
        private readonly GameSetting _settings;

        #region Ctor
        protected DispatchActionsBase(GameSetting settings)
        {
            _settings = settings;
        }
        #endregion

        protected bool ExecuteAndDispatch()
        {
            var allActionSucceeded = true;

            foreach (var action in _settings.DisablingIntroductionActions)
            {
                var actionSucceeded = DispatchActionsPerType(action);
                if (!actionSucceeded) allActionSucceeded = false;
            }

            return allActionSucceeded;
        }

        private bool DispatchActionsPerType(DisableIntroductionAction action)
        {
            switch (action.Type)
            {
                case DisableActionTypeEnum.FileRename:
                    return ProcessRenameFile(action);
                case DisableActionTypeEnum.FolderRename:
                    return ProcessRenameFolder(action);
                case DisableActionTypeEnum.ShortcutEdition:
                    return ProcessEditShortcut(action);
                default:
                    throw new Exception($"DisableActionTypeEnum {action.Type} not found");
            }
        }

        protected abstract bool ProcessRenameFile(DisableIntroductionAction action);

        protected abstract bool ProcessRenameFolder(DisableIntroductionAction action);
        
        protected abstract bool ProcessEditShortcut(DisableIntroductionAction action);

        protected static string GetCleanPath(string path)
        {
            return path.Replace(@"\\", @"\");
        }
    }
}
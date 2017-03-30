using System;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

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
                    return RenameFile(action);
                case DisableActionTypeEnum.FolderRename:
                    return RenameFolder(action);
                case DisableActionTypeEnum.ShortcutEdition:
                    return EditShortcut(action);
                default:
                    throw new Exception($"DisableActionTypeEnum {action.Type} not found");
            }
        }

        protected abstract bool RenameFile(DisableIntroductionAction action);

        protected abstract bool RenameFolder(DisableIntroductionAction action);
        
        protected abstract bool EditShortcut(DisableIntroductionAction action);
    }
}
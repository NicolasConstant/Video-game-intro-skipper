using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    public class DetectIntroductionState
    {
        private List<DisableIntroductionAction> _disablingIntroductionActions;
        private DirectoryInfo _installationPath;

        #region Ctor
        public DetectIntroductionState()
        {

        }

        public DetectIntroductionState(DirectoryInfo installationPath, List<DisableIntroductionAction> disablingIntroductionActions)
        {
            _installationPath = installationPath;
            _disablingIntroductionActions = disablingIntroductionActions;
        }
        #endregion

        public IntroductionStateEnum Execute()
        {
            var actionStateList = new List<IntroductionStateEnum>();

            foreach (var action in _disablingIntroductionActions)
            {
                var actionState = GetStateFromAction(action);
                actionStateList.Add(actionState);
            }

            // Check if status enabled
            if (actionStateList.Any(x => x == IntroductionStateEnum.Enabled)
                && actionStateList.All(x => x != IntroductionStateEnum.Disabled))
                return IntroductionStateEnum.Enabled;

            // Check if status disabled
            if (actionStateList.Any(x => x == IntroductionStateEnum.Disabled)
                && actionStateList.All(x => x != IntroductionStateEnum.Enabled))
                return IntroductionStateEnum.Disabled;

            // Status is unknown
            return IntroductionStateEnum.Unknown;
        }

        private IntroductionStateEnum GetStateFromAction(DisableIntroductionAction action)
        {
            switch (action.Type)
            {
                case DisableActionTypeEnum.FileRename:
                    return CheckFileRenameState(action);
                case DisableActionTypeEnum.FolderRename:
                    return CheckFolderRenameState(action);
                case DisableActionTypeEnum.ShortcutEdition:
                    return CheckShortcutEdition(action);
                default:
                    throw new Exception($"DisableActionTypeEnum {action.Type} not known");
            }
        }

        private IntroductionStateEnum CheckFileRenameState(DisableIntroductionAction action)
        {
            // Check if file exists
            var expectedInitialFileName = action.InitialName;
            var fileIsInInitialState = File.Exists(_installationPath.FullName + "\\" + expectedInitialFileName);

            // Return status
            return fileIsInInitialState ? IntroductionStateEnum.Enabled : IntroductionStateEnum.Disabled;
        }

        private IntroductionStateEnum CheckFolderRenameState(DisableIntroductionAction action)
        {
            // Check if folder exists
            var expectedInitialDirectoryName = action.InitialName;
            var directoryIsInInitialState = Directory.Exists(_installationPath.FullName + "\\" + expectedInitialDirectoryName);

            // Return status
            return directoryIsInInitialState ? IntroductionStateEnum.Enabled : IntroductionStateEnum.Disabled;
        }

        private IntroductionStateEnum CheckShortcutEdition(DisableIntroductionAction action)
        {
            throw new NotImplementedException();
        }
    }
}
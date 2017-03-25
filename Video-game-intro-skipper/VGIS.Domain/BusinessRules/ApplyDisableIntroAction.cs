using System;
using System.IO;
using VGIS.Domain.Consts;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    public class ApplyDisableIntroAction
    {
        private readonly GameSetting _settings;
        private readonly GameDetectionResult _detectionResult;

        #region Ctor
        public ApplyDisableIntroAction(GameSetting settings, GameDetectionResult detectionResult)
        {
            _settings = settings;
            _detectionResult = detectionResult;
        }
        #endregion

        public bool Execute()
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

        private bool RenameFile(DisableIntroductionAction action)
        {
            try
            {
                var fileFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}";
                var destinationFileFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}{GlobalNamesStruct.RenameSuffix}";

                if(File.Exists(destinationFileFullPath)) File.Delete(destinationFileFullPath);

                File.Move(fileFullPath, destinationFileFullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool RenameFolder(DisableIntroductionAction action)
        {
            try
            {
                var directoryFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}";
                var destinationDirectoryFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}{GlobalNamesStruct.RenameSuffix}";

                if (Directory.Exists(destinationDirectoryFullPath)) File.Delete(destinationDirectoryFullPath);

                Directory.Move(directoryFullPath, destinationDirectoryFullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool EditShortcut(DisableIntroductionAction action)
        {
            throw new NotImplementedException();
        }
    }
}
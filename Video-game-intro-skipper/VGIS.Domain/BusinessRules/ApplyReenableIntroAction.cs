using System;
using System.IO;
using VGIS.Domain.BusinessRules.Bases;
using VGIS.Domain.Consts;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    public class ApplyReenableIntroAction : DispatchActionsBase
    {
        private readonly GameDetectionResult _detectionResult;

        #region Ctor
        public ApplyReenableIntroAction(GameSetting settings, GameDetectionResult detectionResult)
            : base(settings)
        {
            _detectionResult = detectionResult;
        }
        #endregion

        public bool Execute()
        {
            return ExecuteAndDispatch();
        }

        protected override bool ProcessRenameFile(DisableIntroductionAction action)
        {
            try
            {
                var fileFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}{GlobalNamesStruct.RenameSuffix}";
                var destinationFileFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}";

                if (File.Exists(destinationFileFullPath)) File.Delete(destinationFileFullPath);

                File.Move(fileFullPath, destinationFileFullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override bool ProcessRenameFolder(DisableIntroductionAction action)
        {
            try
            {
                var directoryFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}{GlobalNamesStruct.RenameSuffix}"; 
                var destinationDirectoryFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}";

                if (Directory.Exists(destinationDirectoryFullPath)) File.Delete(destinationDirectoryFullPath);

                Directory.Move(directoryFullPath, destinationDirectoryFullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override bool ProcessEditShortcut(DisableIntroductionAction action)
        {
            throw new NotImplementedException();
        }
    }
}
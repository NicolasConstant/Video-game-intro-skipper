using System;
using System.Collections.Generic;
using System.IO;
using VGIS.Domain.BusinessRules.Bases;
using VGIS.Domain.Consts;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Tools;

namespace VGIS.Domain.BusinessRules
{
    public class ApplyDisableIntroAction : DispatchActionsBase
    {
        private readonly GameDetectionResult _detectionResult;
        private readonly IFileAndFolderRenamer _fileRenamer;
        private readonly IPathPatternTranslator _pathPatternTranslator;

        #region Ctor
        public ApplyDisableIntroAction(GameSetting settings, GameDetectionResult detectionResult, IFileAndFolderRenamer fileRenamer, IPathPatternTranslator pathPatternTranslator)
            : base(settings)
        {
            _detectionResult = detectionResult;
            _fileRenamer = fileRenamer;
            _pathPatternTranslator = pathPatternTranslator;
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
                var pattern = GetCleanPath($"{_detectionResult.InstallationPath}\\{action.InitialName}");
                var pathsToRename = _pathPatternTranslator.GetPathFromPattern(pattern);
                foreach (var path in pathsToRename)
                    _fileRenamer.RenameFile(path, $@"{path}{GlobalNamesStruct.RenameSuffix}");

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

        protected override bool ProcessEditShortcut(DisableIntroductionAction action)
        {
            throw new NotImplementedException();
        }
    }
}
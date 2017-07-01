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
                

                ////If the path doesn't contain '*' symbole, apply renaming directly
                //if (!originFileFullPath.Contains(SpecialChar.AnyDirectoryName.ToString()))
                //{
                //    var destinationFileFullPath = GetCleanPath($"{_detectionResult.InstallationPath}\\{action.InitialName}{GlobalNamesStruct.RenameSuffix}");
                //    _fileRenamer.RenameFile(originFileFullPath, destinationFileFullPath);
                //}
                ////Apply renaming by navigainge througt all '*' alternatives
                //else
                //{
                //    var pathSections = originFileFullPath.Split('\\');
                //    var allPaths = new List<string>();
                //    for (var i = 1; i < pathSections.Length; i++)
                //    {
                //        var subPath = pathSections[i];
                //        //Init
                //        if (i == 1)
                //        {
                //            allPaths.Add($@"{pathSections[0]}\{pathSections[1]}");
                //        }
                //        //Is a subfolder
                //        else if (!subPath.Contains(SpecialChar.AnyDirectoryName.ToString()))
                //        {
                //            for (var j = 0; j < allPaths.Count; j++)
                //            {
                //                var completePath = allPaths[j];
                //                var completeSubPath = $"{completePath}\\{subPath}";
                //                allPaths[j] = completeSubPath;
                //            }
                //        }
                //        //Is a navigation order
                //        else if (subPath.Trim() == SpecialChar.AnyDirectoryName.ToString().Trim())
                //        {
                //            var allPathResults = new List<string>();
                //            for (var j = 0; j < allPaths.Count; j++)
                //            {
                //                var completePath = allPaths[j];
                //                var allSubPaths = _directoryBrowser.GetSubDirectories(completePath);
                //                foreach (var allSubPath in allSubPaths)
                //                    allPathResults.Add(allSubPath);
                //            }
                //            allPaths = allPathResults;
                //        }
                //        //Is unsupported
                //        else
                //        {
                //            throw new ArgumentException($"{originFileFullPath} is using a unsupported pattern");
                //        }
                //    }



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
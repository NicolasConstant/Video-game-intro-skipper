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

        #region Ctor
        public ApplyDisableIntroAction(GameSetting settings, GameDetectionResult detectionResult, IFileAndFolderRenamer fileRenamer)
            :base(settings)
        {
            _detectionResult = detectionResult;
            _fileRenamer = fileRenamer;
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
                var originFileFullPath = GetCleanPath($"{_detectionResult.InstallationPath}\\{action.InitialName}");

                //If the path doesn't contain '*' symbole, apply renaming directly
                if (!originFileFullPath.Contains(SpecialChar.AnyDirectoryName.ToString()))
                {
                    var destinationFileFullPath = GetCleanPath($"{_detectionResult.InstallationPath}\\{action.InitialName}{GlobalNamesStruct.RenameSuffix}");
                    _fileRenamer.RenameFile(originFileFullPath, destinationFileFullPath);
                }
                //Apply renaming by navigainge througt all '*' alternatives
                else 
                {
                    var pathSections = originFileFullPath.Split(SpecialChar.AnyDirectoryName);
                    var allPaths = new List<DirectoryInfo>();
                    for (var i = 0; i < pathSections.Length; i++)
                    {
                        var subPath = pathSections[i];
                        //Init
                        if (i == 0)
                        {
                            allPaths.Add(new DirectoryInfo(subPath));
                        }
                        //Is a subfolder
                        else if(!subPath.Contains(SpecialChar.AnyDirectoryName.ToString()))
                        {
                            for (var j = 0; j < allPaths.Count; j++ )
                            {
                                var completePath = allPaths[j];
                                var completeSubPath = $"{completePath.FullName}\\{subPath}";
                                if (Directory.Exists(completeSubPath))
                                {
                                    var subPathDirectoryInfo = new DirectoryInfo(completeSubPath);
                                    allPaths[j] = subPathDirectoryInfo;
                                }
                            }
                        }
                        //Is a navigation order
                        else if (subPath.Trim() == SpecialChar.AnyDirectoryName.ToString().Trim())
                        {
                            var allPathResults = new List<DirectoryInfo>();
                            for (var j = 0; j < allPaths.Count; j++)
                            {
                                var completePath = allPaths[j];
                                var allSubPaths = completePath.GetDirectories();
                                allPathResults.AddRange(allSubPaths);
                            }
                            allPaths = allPathResults;
                        }
                        //Is unsupported
                        else
                        {
                            throw new ArgumentException($"{originFileFullPath} is using a unsupported pattern");
                        }
                    }

                    //foreach (var directoryInfo in allPaths)
                    //{
                    //    var destinationFileFullPath = $"{_detectionResult.InstallationPath}\\{action.InitialName}{GlobalNamesStruct.RenameSuffix}";
                    //    _fileRenamer.RenameFile(directoryInfo.FullName );
                    //}
                }
                
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    public class GenerateRootValidationRulesAction
    {
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public GenerateRootValidationRulesAction(IFileSystemDal fileSystemDal)
        {
            _fileSystemDal = fileSystemDal;
        }
        #endregion

        public IEnumerable<RootValidationRule> Execute(string gameRootFolder)
        {
            if(!_fileSystemDal.DirectoryExists(gameRootFolder)) throw new ArgumentException("gameRootFolder");

            //Get First Level Folders
            var subFolders = _fileSystemDal.DirectoryGetChildren(gameRootFolder);
            foreach (var directoryInfo in subFolders)
            {
                yield return new RootValidationRule()
                {
                    Type = RootValidationTypeEnum.FolderValidation,
                    WitnessName = directoryInfo.Name
                };
            }

            //Get Exes
            var exeFounds = GetExeInFolderAndSubFolders(gameRootFolder);
            foreach (var fileInfo in exeFounds)
            {
                yield return new RootValidationRule()
                {
                    Type = RootValidationTypeEnum.FileValidation,
                    WitnessName = fileInfo.Name
                };
            }
        }

        private IEnumerable<FileInfo> GetExeInFolderAndSubFolders(string gameRootFolder)
        {
            //Return exes 
            var filesFound = _fileSystemDal.GetFiles(gameRootFolder);
            foreach (var fileInfo in filesFound)
            {
                var ext = Path.GetExtension(fileInfo.Name);
                if (ext == ".exe") yield return fileInfo;
            }

            //Browse
            var subFolders = _fileSystemDal.DirectoryGetChildren(gameRootFolder);
            foreach (var subFolder in subFolders)
            {
                var results = GetExeInFolderAndSubFolders(subFolder.FullName);
                foreach (var result in results)
                    yield return result;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;

namespace VGIS.Domain.Repositories
{
    public class InstallationDirectoriesRepository
    {
        private readonly string _defaultInstallFolderSettingsFile;
        private readonly string[] _defaultInstallFolder;
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public InstallationDirectoriesRepository(string defaultInstallFolderSettingsFile, IFileSystemDal fileSystemDal)
        {
            _defaultInstallFolderSettingsFile = defaultInstallFolderSettingsFile;
            _fileSystemDal = fileSystemDal;

            var defaultInstallFolderJson = _fileSystemDal.ReadAllText(_defaultInstallFolderSettingsFile);
            _defaultInstallFolder = JsonConvert.DeserializeObject<string[]>(defaultInstallFolderJson);
        }
        #endregion

        public IEnumerable<DirectoryInfo> GetAllInstallationFolders()
        {
            foreach (var s in _defaultInstallFolder)
                yield return new DirectoryInfo(s);
        }
    }
}
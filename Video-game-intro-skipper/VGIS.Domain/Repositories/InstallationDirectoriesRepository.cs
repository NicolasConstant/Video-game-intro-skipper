using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;

namespace VGIS.Domain.Repositories
{
    public class InstallationDirectoriesRepository
    {
        private readonly string _defaultInstallFolderSettingsFile;
        private readonly string _customInstallFolderSettingsFile;
        //private readonly string[] _defaultInstallFolder;
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public InstallationDirectoriesRepository(string defaultInstallFolderSettingsFile, string customInstallFolderSettingsFile, IFileSystemDal fileSystemDal)
        {
            _defaultInstallFolderSettingsFile = defaultInstallFolderSettingsFile;
            _customInstallFolderSettingsFile = customInstallFolderSettingsFile;
            _fileSystemDal = fileSystemDal;
            
            if (!_fileSystemDal.FileExists(_customInstallFolderSettingsFile))
                _fileSystemDal.FileCreate(_customInstallFolderSettingsFile);
        }
        #endregion

        public IEnumerable<DirectoryInfo> GetAllInstallationFolders()
        {
            var allFolders = new List<string>();
            allFolders.AddRange(ReadAndReturnDirectoriesFromConfigFile(_defaultInstallFolderSettingsFile));
            allFolders.AddRange(ReadAndReturnDirectoriesFromConfigFile(_customInstallFolderSettingsFile));

            foreach (var s in allFolders)
                yield return new DirectoryInfo(s);
        }

        public void AddNewCustomInstallFolder(string path)
        {
            var allCurentCustomInstallFolder = ReadAndReturnDirectoriesFromConfigFile(_customInstallFolderSettingsFile).ToList();

            if (_fileSystemDal.DirectoryExists(path) && !allCurentCustomInstallFolder.Contains(path))
            {
                allCurentCustomInstallFolder.Add(path);
                var foldersJson = JsonConvert.SerializeObject(allCurentCustomInstallFolder);
                _fileSystemDal.FileWriteAllText(_customInstallFolderSettingsFile, foldersJson);
            }
        }

        private IEnumerable<string> ReadAndReturnDirectoriesFromConfigFile(string configFile)
        {
            try
            {
                var foldersJson = _fileSystemDal.ReadAllText(configFile);
                return JsonConvert.DeserializeObject<string[]>(foldersJson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new string[0];
        }
    }
}
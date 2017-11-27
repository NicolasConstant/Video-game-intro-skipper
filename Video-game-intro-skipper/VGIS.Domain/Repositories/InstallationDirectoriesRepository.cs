using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;
using VGIS.Domain.Settings;

namespace VGIS.Domain.Repositories
{
    public class InstallationDirectoriesRepository
    {
        private readonly string _defaultInstallFolderSettingsFile;
        private readonly string _customInstallFolderSettingsFile;
        private readonly IFileSystemDal _fileSystemDal;

        #region Ctor
        public InstallationDirectoriesRepository(GlobalSettings settings, IFileSystemDal fileSystemDal)
        {
            _defaultInstallFolderSettingsFile = settings.DefaultInstallFolderConfigFile;
            _customInstallFolderSettingsFile = settings.CustomInstallFolderConfigFile;
            _fileSystemDal = fileSystemDal;

            if (!_fileSystemDal.FileExists(_customInstallFolderSettingsFile))
                LoadAndCreateUserSettings();
        }
        #endregion

        private void LoadAndCreateUserSettings()
        {
            //Get common settings
            var defaultInstallFolders = ReadAndReturnDirectoriesFromConfigFile(_defaultInstallFolderSettingsFile).ToList();

            //Serialize to file
            var defaultInstallFoldersJson = JsonConvert.SerializeObject(defaultInstallFolders);
            _fileSystemDal.FileWriteAllText(_customInstallFolderSettingsFile, defaultInstallFoldersJson);
        }

        public IEnumerable<DirectoryInfo> GetAllInstallationFolders()
        {
            var allFolders = new List<string>();
            allFolders.AddRange(ReadAndReturnDirectoriesFromConfigFile(_customInstallFolderSettingsFile));

            foreach (var s in allFolders)
                yield return new DirectoryInfo(s);
        }

        public void AddNewInstallFolder(string path)
        {
            var allCurentCustomInstallFolder = ReadAndReturnDirectoriesFromConfigFile(_customInstallFolderSettingsFile).ToList();

            if (_fileSystemDal.DirectoryExists(path) && !allCurentCustomInstallFolder.Contains(path))
            {
                allCurentCustomInstallFolder.Add(path);
                var foldersJson = JsonConvert.SerializeObject(allCurentCustomInstallFolder);
                _fileSystemDal.FileWriteAllText(_customInstallFolderSettingsFile, foldersJson);
            }
        }

        public void RemoveInstallFolder(string path)
        {
            var allCurentCustomInstallFolder = ReadAndReturnDirectoriesFromConfigFile(_customInstallFolderSettingsFile).ToList();

            if (allCurentCustomInstallFolder.Contains(path))
            {
                allCurentCustomInstallFolder.Remove(path);
                var foldersJson = JsonConvert.SerializeObject(allCurentCustomInstallFolder);
                _fileSystemDal.FileWriteAllText(_customInstallFolderSettingsFile, foldersJson);
            }
        }

        public void ResetInstallationFolders()
        {
            if (_fileSystemDal.FileExists(_customInstallFolderSettingsFile))
                _fileSystemDal.FileDelete(_customInstallFolderSettingsFile);

            LoadAndCreateUserSettings();
        }

        private IEnumerable<string> ReadAndReturnDirectoriesFromConfigFile(string configFile)
        {
            try
            {
                var foldersJson = _fileSystemDal.ReadAllText(configFile);
                return JsonConvert.DeserializeObject<string[]>(foldersJson) ?? new string[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new string[0];
        }
    }
}
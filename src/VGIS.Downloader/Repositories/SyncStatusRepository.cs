using System;
using System.IO;
using Newtonsoft.Json;

namespace VGIS.Downloader.Repositories
{
    public class SyncStatusRepository
    {
        private readonly string _syncFilFullPath;

        #region Ctor
        public SyncStatusRepository()
        {
            var syncDir = $@"{Directory.GetCurrentDirectory()}\SyncStatus\";
            if (!Directory.Exists(syncDir)) Directory.CreateDirectory(syncDir);
            _syncFilFullPath = $"{syncDir}status.json";
            if (!File.Exists(_syncFilFullPath)) File.Create(_syncFilFullPath);
        }
        #endregion
        
        public DateTime GetSyncStatus()
        {
            var content = File.ReadAllText(_syncFilFullPath);
            if (string.IsNullOrWhiteSpace(content)) return default(DateTime);

            var dateTime = JsonConvert.DeserializeObject<DateTime>(content);
            return dateTime;
        }

        public void SaveSyncStatus(DateTime syncDateTime)
        {
            var jsonObject = JsonConvert.SerializeObject(syncDateTime);
            File.WriteAllText(_syncFilFullPath, jsonObject);
        }
    }
}
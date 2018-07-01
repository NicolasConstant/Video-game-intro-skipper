using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VGIS.Azure;
using VGIS.Downloader.Settings;

namespace VGIS.Downloader.BusinessRules
{
    public class RetrieveAllGameSettingsSinceLastSync
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly TableStorageService _tableStorageService;
        private readonly string _downloadDir;

        #region Ctor
        public RetrieveAllGameSettingsSinceLastSync(BlobStorageService blobStorageService, TableStorageService tableStorageService)
        {
            _blobStorageService = blobStorageService;
            _tableStorageService = tableStorageService;
            _downloadDir = $@"{Directory.GetCurrentDirectory()}\DownloadedGameSettings\";
        }
        #endregion

        public async Task<DateTime> ExecuteAsync(DateTime lastSync)
        {
            var maxDate = DateTime.UtcNow;
            
            var entries = await _tableStorageService.GetEntries(lastSync, maxDate);
            if (!entries.Any()) return maxDate;
            
            foreach (var settingsEntity in entries)
            {
                Console.WriteLine($"Downloading: {settingsEntity.RowKey}");

                //Prepare destination folder
                var currentSyncDir = $@"{_downloadDir}{maxDate.ToLocalTime():yyyy-MM-dd}\{settingsEntity.PartitionKey}\";
                if (!Directory.Exists(currentSyncDir)) Directory.CreateDirectory(currentSyncDir);

                //Download file
                await _blobStorageService.DownloadFile(currentSyncDir, settingsEntity.SettingFileName);

                //Rename file
                var splitedFileName = settingsEntity.SettingFileName.Split('.');
                var expectedName = Path.Combine(currentSyncDir, settingsEntity.SettingFileName);
                var destinationName = Path.Combine(currentSyncDir, $"{splitedFileName[1]}.{splitedFileName[2]}");
                File.Move(expectedName, destinationName);
            }

            return maxDate;
        }
    }
}
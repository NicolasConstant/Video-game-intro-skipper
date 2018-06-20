using System;
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

        #region Ctor
        public RetrieveAllGameSettingsSinceLastSync(BlobStorageService blobStorageService, TableStorageService tableStorageService)
        {
            _blobStorageService = blobStorageService;
            _tableStorageService = tableStorageService;
        }
        #endregion

        public async Task<DateTime> ExecuteAsync(DateTime lastSync)
        {
            var maxDate = DateTime.UtcNow;
            var entries = await _tableStorageService.GetEntries(lastSync, maxDate);
            foreach (var settingsEntity in entries)
            {
                await _blobStorageService.DownloadFile(@"C:\Temp\", settingsEntity.SettingFileName);
            }


            return maxDate;
        }
    }
}
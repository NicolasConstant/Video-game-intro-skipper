using System.Data.SqlTypes;
using System.Threading.Tasks;
using VGIS.Azure;
using VGIS.Downloader.BusinessRules;
using VGIS.Downloader.Settings;

namespace VGIS.Downloader.Domain
{
    public class DownloaderLogic
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly TableStorageService _tableStorageService;

        #region Ctor
        public DownloaderLogic(BlobStorageService blobStorageService, TableStorageService tableStorageService)
        {
            _blobStorageService = blobStorageService;
            _tableStorageService = tableStorageService;
        }
        #endregion

        public async Task RunAsync()
        {
            var getLastSync = new GetLastSynchronisation();
            var lastSync = getLastSync.Execute();
            
            var retrieveAllGameSettings = new RetrieveAllGameSettingsSinceLastSync(_blobStorageService, _tableStorageService);
            var processedIter = await retrieveAllGameSettings.ExecuteAsync(lastSync);

            var saveSyncStatus = new SaveSyncStatus();
            saveSyncStatus.Execute();
        }
    }
}
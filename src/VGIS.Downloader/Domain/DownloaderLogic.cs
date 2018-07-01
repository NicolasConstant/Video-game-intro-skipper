using System.Data.SqlTypes;
using System.Threading.Tasks;
using VGIS.Azure;
using VGIS.Downloader.BusinessRules;
using VGIS.Downloader.Repositories;
using VGIS.Downloader.Settings;

namespace VGIS.Downloader.Domain
{
    public class DownloaderLogic
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly TableStorageService _tableStorageService;
        private readonly SyncStatusRepository _syncStatusRepository;

        #region Ctor
        public DownloaderLogic(BlobStorageService blobStorageService, TableStorageService tableStorageService, SyncStatusRepository syncStatusRepository)
        {
            _blobStorageService = blobStorageService;
            _tableStorageService = tableStorageService;
            _syncStatusRepository = syncStatusRepository;
        }
        #endregion

        public async Task RunAsync()
        {
            var getLastSync = new GetLastSynchronisation(_syncStatusRepository);
            var lastSync = getLastSync.Execute();
            
            var retrieveAllGameSettings = new RetrieveAllGameSettingsSinceLastSync(_blobStorageService, _tableStorageService);
            var currentSync = await retrieveAllGameSettings.ExecuteAsync(lastSync);

            var saveSyncStatus = new SaveSyncStatus(_syncStatusRepository);
            saveSyncStatus.Execute(currentSync);
        }
    }
}
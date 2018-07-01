using System;
using VGIS.Downloader.Repositories;

namespace VGIS.Downloader.BusinessRules
{
    public class SaveSyncStatus
    {
        private readonly SyncStatusRepository _syncStatusRepository;

        #region Ctor
        public SaveSyncStatus(SyncStatusRepository syncStatusRepository)
        {
            _syncStatusRepository = syncStatusRepository;
        }
        #endregion

        public void Execute(DateTime syncDateTime)
        {
            _syncStatusRepository.SaveSyncStatus(syncDateTime);
        }
    }
}
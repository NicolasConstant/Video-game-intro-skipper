using System;
using VGIS.Downloader.Repositories;

namespace VGIS.Downloader.BusinessRules
{
    public class GetLastSynchronisation
    {
        private readonly SyncStatusRepository _syncStatusRepository;

        #region Ctor
        public GetLastSynchronisation(SyncStatusRepository syncStatusRepository)
        {
            _syncStatusRepository = syncStatusRepository;
        }
        #endregion

        public DateTime Execute()
        {
            var lastSavedSync = _syncStatusRepository.GetSyncStatus();

            return lastSavedSync == default(DateTime) ? DateTime.UtcNow.AddYears(-20) : lastSavedSync;
        }
    }
}
using System;

namespace VGIS.Downloader.BusinessRules
{
    public class GetLastSynchronisation
    {
        public DateTime Execute()
        {
            var d = DateTime.UtcNow.AddMonths(-12);
            return d;
        }
    }
}
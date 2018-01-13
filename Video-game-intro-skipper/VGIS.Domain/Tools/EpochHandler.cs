using System;

namespace VGIS.Domain.Tools
{
    public static class EpochHandler
    {
        public static int GenerateEpochNow()
        {
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (int)t.TotalSeconds;
        }
    }
}
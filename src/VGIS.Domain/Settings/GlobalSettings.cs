using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGIS.Domain.Settings
{
    public class GlobalSettings
    {
        public string DefaultGamesSettingsFolder { get; set; }
        public string CustomGamesSettingsFolder { get; set; }
        public string DefaultInstallFolderConfigFile { get; set; }
        public string CustomInstallFolderConfigFile { get; set; }
    }
}

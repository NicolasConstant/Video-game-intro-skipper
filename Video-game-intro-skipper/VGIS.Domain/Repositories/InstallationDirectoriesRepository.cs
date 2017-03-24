using System;
using System.Collections.Generic;
using System.IO;

namespace VGIS.Domain.Repositories
{
    public class InstallationDirectoriesRepository
    {
        public IEnumerable<DirectoryInfo> GetAllInstallationFolders()
        {
            yield return new DirectoryInfo(@"C:\Program Files (x86)\Steam\steamapps\common");
            yield return new DirectoryInfo(@"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games");
        }
    }
}
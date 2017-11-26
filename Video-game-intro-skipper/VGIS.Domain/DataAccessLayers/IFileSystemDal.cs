using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace VGIS.Domain.DataAccessLayers
{
    public interface IFileSystemDal
    {
        IEnumerable<FileInfo> GetFiles(string path);
        string ReadAllText(string fileFullName);
    }
}
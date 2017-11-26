using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGIS.Domain.DataAccessLayers
{
    public class FileSystemDal : IFileSystemDal
    {
        public IEnumerable<FileInfo> GetFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles();
            return files;
        }

        public string ReadAllText(string fileFullName)
        {
            return File.ReadAllText(fileFullName);
        }
    }
}

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

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void FileCreate(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var dir = fileInfo.DirectoryName;

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.Create(filePath);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void FileWriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public void FileDelete(string filePath)
        {
            File.Delete(filePath);
        }
    }
}

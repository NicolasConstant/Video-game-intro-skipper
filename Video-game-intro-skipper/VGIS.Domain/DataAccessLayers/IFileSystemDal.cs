using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace VGIS.Domain.DataAccessLayers
{
    public interface IFileSystemDal
    {
        IEnumerable<FileInfo> GetFiles(string path);
        string ReadAllText(string fileFullName);
        bool FileExists(string filePath);
        void FileCreate(string filePath);
        bool DirectoryExists(string path);
        void FileWriteAllText(string path, string content);
        void FileDelete(string filePath);
    }
}
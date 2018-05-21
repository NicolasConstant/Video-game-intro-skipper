using System.IO;

namespace VGIS.Crowdsourcing.Tools
{
    public interface IFileHandler
    {
        string GetFileName(string pathToSettingsFile);
        byte[] GetFileBytes(string pathToSettingsFile);
    }

    public class FileHandler : IFileHandler
    {
        public string GetFileName(string pathToSettingsFile)
        {
            var fileInfo = new FileInfo(pathToSettingsFile);
            return fileInfo.Name;
        }

        public byte[] GetFileBytes(string pathToSettingsFile)
        {
            return File.ReadAllBytes(pathToSettingsFile);
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VGIS.Domain.Tools
{
    public interface IDirectoryBrowser
    {
        bool DirectoryExists(string completeSubPath);
        IEnumerable<string> GetSubDirectories(string completePath);
    }

    public class DirectoryBrowser : IDirectoryBrowser
    {
        public bool DirectoryExists(string completeSubPath)
        {
            return Directory.Exists(completeSubPath);
        }

        public IEnumerable<string> GetSubDirectories(string completePath)
        {
            return new DirectoryInfo(completePath).GetDirectories().Select(x => x.FullName);
        }
    }
}
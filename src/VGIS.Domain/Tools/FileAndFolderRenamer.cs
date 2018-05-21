using System.IO;

namespace VGIS.Domain.Tools
{
    public interface IFileAndFolderRenamer
    {
        void RenameFile(string sourceFileName, string destFileFullPath, bool overrideFile = true);
    }

    public class FileAndFolderRenamer : IFileAndFolderRenamer
    {
        public void RenameFile(string sourceFileName, string destFileFullPath, bool overrideFile = true)
        {
            if (File.Exists(destFileFullPath) && overrideFile) File.Delete(destFileFullPath);
            File.Move(sourceFileName, destFileFullPath);
        }
    }
}
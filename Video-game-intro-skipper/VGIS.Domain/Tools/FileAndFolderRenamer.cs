using System.IO;

namespace VGIS.Domain.Tools
{
    public interface IFileAndFolderRenamer
    {
        void RenameFile(string sourceFileName, string destFileFullPath);
    }

    public class FileAndFolderRenamer : IFileAndFolderRenamer
    {
        public void RenameFile(string sourceFileName, string destFileFullPath)
        {
            if (File.Exists(destFileFullPath)) File.Delete(destFileFullPath);
            File.Move(sourceFileName, destFileFullPath);
        }
    }
}
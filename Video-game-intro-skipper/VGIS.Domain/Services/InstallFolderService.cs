using System.Linq;
using VGIS.Domain.Repositories;

namespace VGIS.Domain.Services
{
    public class InstallFolderService
    {
        private readonly InstallationDirectoriesRepository _repository;

        public InstallFolderService(InstallationDirectoriesRepository repository)
        {
            _repository = repository;
        }

        public string[] GetAllInstallFolder()
        {
            return _repository.GetAllInstallationFolders().Select(x => x.FullName).ToArray();
        }

        public void AddInstallationFolder(string path)
        {
            _repository.AddNewInstallFolder(path);
        }

        public void RemoveInstallationFolder(string path)
        {
            _repository.RemoveInstallFolder(path);
        }

        public void ResetInstallationFolders()
        {
            _repository.ResetInstallationFolders();
        }
    }
}
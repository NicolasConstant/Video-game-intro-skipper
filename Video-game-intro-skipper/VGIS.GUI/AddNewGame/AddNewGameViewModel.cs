using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using VGIS.Domain.Services;

namespace VGIS.GUI.AddNewGame
{
    public class AddNewGameViewModel : BindableBase
    {
        private readonly InstallFolderService _installFolderService;
        private readonly GameService _gameService;

        private ObservableCollection<string> _installFolders = new ObservableCollection<string>();
        private ObservableCollection<string> _potentialGameFolders = new ObservableCollection<string>();
        private string _selectedInstallFolder;
        private string _selectedGameFolder;


        public ObservableCollection<string> InstallFolders
        {
            get => _installFolders;
            set => SetProperty(ref _installFolders, value);
        }

        public ObservableCollection<string> PotentialGameFolders
        {
            get => _potentialGameFolders;
            set => SetProperty(ref _potentialGameFolders, value);
        }

        public string SelectedInstallFolder
        {
            get => _selectedInstallFolder;
            set
            {
                BrowseAndDetectPotentialGameFolders(value);
                SetProperty(ref _selectedInstallFolder, value);
            }
        }

        public string SelectedGameFolder
        {
            get => _selectedGameFolder;
            set => SetProperty(ref _selectedGameFolder, value);
        }

        #region Ctor
        public AddNewGameViewModel(InstallFolderService installFolderService, GameService gameService)
        {
            _installFolderService = installFolderService;
            _gameService = gameService;

            InstallFolders.AddRange(_installFolderService.GetAllInstallFolder());
            SelectedInstallFolder = InstallFolders?.FirstOrDefault();
        }
        #endregion

        private void BrowseAndDetectPotentialGameFolders(string parentFolder)
        {
            PotentialGameFolders.Clear();
            PotentialGameFolders.AddRange(_installFolderService.GetSubFolders(parentFolder));
            SelectedGameFolder = PotentialGameFolders?.FirstOrDefault();
        }
    }
}
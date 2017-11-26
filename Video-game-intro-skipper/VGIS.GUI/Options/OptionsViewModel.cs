using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;

namespace VGIS.GUI.Options
{
    public class OptionsViewModel : BindableBase
    {
        private ObservableCollection<string> _installFolders = new ObservableCollection<string>();
        private string _selectedFolder;

        public ObservableCollection<string> InstallFolders
        {
            get => _installFolders;
            set => SetProperty(ref _installFolders, value);
        }

        public string SelectedFolder
        {
            get => _selectedFolder;
            set => SetProperty(ref _selectedFolder, value);
        }

        #region Ctor
        public OptionsViewModel()
        {
            //TODO get install folder from repository
            var installFolders = new[] { "path1", "path2", "path3" };

            InstallFolders = new ObservableCollection<string>();
            InstallFolders.AddRange(installFolders);

            ResetInstallFoldersCommand = new DelegateCommand(ResetInstallFolders);
            RemoveInstallFolderCommand = new DelegateCommand(RemoveInstallFolder);
            AddInstallFolderCommand = new DelegateCommand(AddInstallFolder);
            CloseCommand = new DelegateCommand(Close);
        }
        #endregion

        public ICommand ResetInstallFoldersCommand { get; set; }

        public ICommand RemoveInstallFolderCommand { get; set; }

        public ICommand AddInstallFolderCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public event Action CloseEvent;
        public event Action FocusEvent;

        private void ResetInstallFolders()
        {
            //TODO get install folder from repository
            var installFolders = new[] { "path1", "path2", "path3" };

            InstallFolders.Clear();
            InstallFolders.AddRange(installFolders);
        }

        private void RemoveInstallFolder()
        {
            if (string.IsNullOrWhiteSpace(SelectedFolder) || !InstallFolders.Contains(SelectedFolder)) return;

            //TODO remove 

            InstallFolders.Remove(SelectedFolder);
        }

        private void AddInstallFolder()
        {
            var dialog = new CommonOpenFileDialog {IsFolderPicker = true};
            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok && dialog.IsFolderPicker)
            {
                var folder = dialog.FileName;

                //TODO add to local storage

                if(!InstallFolders.Contains(folder)) InstallFolders.Add(folder);
            }

            FocusEvent?.Invoke();
        }

        private void Close()
        {
            CloseEvent?.Invoke();
        }

    }
}
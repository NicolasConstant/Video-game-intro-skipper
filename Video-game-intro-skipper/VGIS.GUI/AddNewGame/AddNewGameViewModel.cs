using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Services;

namespace VGIS.GUI.AddNewGame
{
    public class AddNewGameViewModel : BindableBase
    {
        private readonly Dictionary<string, IllustrationPlatformEnum> _illustrationDisplayableValue = new Dictionary<string, IllustrationPlatformEnum>();
        private readonly InstallFolderService _installFolderService;
        private readonly GameService _gameService;
        private readonly IllustrationValidationService _illustrationValidationService;
        private readonly IntroductionActivationService _introductionActivationService;

        private ObservableCollection<string> _installFolders = new ObservableCollection<string>();
        private ObservableCollection<string> _potentialGameFolders = new ObservableCollection<string>();
        private ObservableCollection<string> _illustrationPlatforms = new ObservableCollection<string>();
        private ObservableCollection<DisableIntroductionAction> _elementsToProcess = new ObservableCollection<DisableIntroductionAction>();
        private DisableIntroductionAction _selectedElementsToProcess;

        private string _name;
        private string _publisher;
        private string _developer;
        private string _selectedInstallFolder;
        private string _selectedGameFolder;
        private string _selectedIllustrationPlatform;
        private IllustrationPlatformEnum _selectedIllustrationPlatformEnum;
        private string _gameIllustrationUrl;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value);
        }

        public string Developer
        {
            get => _developer;
            set => SetProperty(ref _developer, value);
        }

        public ObservableCollection<DisableIntroductionAction> ElementsToProcess
        {
            get => _elementsToProcess;
            set => SetProperty(ref _elementsToProcess, value);
        }

        public DisableIntroductionAction SelectedElementToProcess
        {
            get => _selectedElementsToProcess;
            set => SetProperty(ref _selectedElementsToProcess, value);
        }

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

        public ObservableCollection<string> IllustrationPlatforms
        {
            get => _illustrationPlatforms;
            set => SetProperty(ref _illustrationPlatforms, value);
        }

        public string SelectedInstallFolder
        {
            get => _selectedInstallFolder;
            set
            {
                ClearElementsToProcess();
                BrowseAndDetectPotentialGameFolders(value);
                SetProperty(ref _selectedInstallFolder, value);
            }
        }

        public string SelectedGameFolder
        {
            get => _selectedGameFolder;
            set
            {
                ClearElementsToProcess();
                SetProperty(ref _selectedGameFolder, value);
            }
        }

        public string SelectedIllustrationPlatform
        {
            get => _selectedIllustrationPlatform;
            set
            {
                SetSelectedIllustrationPlatformEnum(value);
                SetProperty(ref _selectedIllustrationPlatform, value);
            }
        }

        public string GameIllustrationUrl
        {
            get => _gameIllustrationUrl;
            set => SetProperty(ref _gameIllustrationUrl, value);
        }

        public event Action CloseEvent;
        public event Action FocusEvent;

        #region Ctor
        public AddNewGameViewModel(InstallFolderService installFolderService, GameService gameService, IllustrationValidationService illustrationValidationService, IntroductionActivationService introductionActivationService)
        {
            _installFolderService = installFolderService;
            _gameService = gameService;
            _illustrationValidationService = illustrationValidationService;
            _introductionActivationService = introductionActivationService;

            InstallFolders.AddRange(_installFolderService.GetAllInstallFolder());
            SelectedInstallFolder = InstallFolders?.FirstOrDefault();

            //Init commands
            AddInstallFolderCommand = new DelegateCommand(AddInstallFolder);
            PickFilesToRenameCommand = new DelegateCommand(PickFilesToRename);
            PickFoldersToRenameCommand = new DelegateCommand(PickFoldersToRename);
            RemoveElementCommand = new DelegateCommand(RemoveElement);
            TestCommand = new DelegateCommand(Test);
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Save);
            IllustrationHelpCommand = new DelegateCommand(LaunchIllustrationHelp);

            InitIllustrationPlatformList();
        }

        private void InitIllustrationPlatformList()
        {
            foreach (IllustrationPlatformEnum val in Enum.GetValues(typeof(IllustrationPlatformEnum)))
            {
                _illustrationDisplayableValue.Add(val.ToString(), val);
                IllustrationPlatforms.Add(val.ToString());
            }

            SelectedIllustrationPlatform = IllustrationPlatformEnum.Steam.ToString();
        }
        #endregion

        public ICommand AddInstallFolderCommand { get; set; }
        public ICommand PickFilesToRenameCommand { get; set; }
        public ICommand PickFoldersToRenameCommand { get; set; }
        public ICommand RemoveElementCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand IllustrationHelpCommand { get; set; }

        private void ClearElementsToProcess()
        {
            SelectedElementToProcess = null;
            ElementsToProcess.Clear();
        }

        private void PickFilesToRename()
        {
            var initialFolder = Path.Combine(SelectedInstallFolder, SelectedGameFolder);

            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = initialFolder,
                EnsureFileExists = true,
                Title = "Select files to rename",
                Multiselect = true
            };

            var result = dialog.ShowDialog();
            FocusEvent?.Invoke();

            if (result == CommonFileDialogResult.Ok)
            {
                var selectedElements = dialog.FileNames;
                AddToElementsToProcess(selectedElements, DisableActionTypeEnum.FileRename);
            }
        }

        private void PickFoldersToRename()
        {
            var initialFolder = Path.Combine(SelectedInstallFolder, SelectedGameFolder);

            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = initialFolder,
                EnsurePathExists = true,
                Title = "Select folder to rename",
                Multiselect = true
            };

            var result = dialog.ShowDialog();
            FocusEvent?.Invoke();

            if (result == CommonFileDialogResult.Ok)
            {
                var selectedElements = dialog.FileNames;
                AddToElementsToProcess(selectedElements, DisableActionTypeEnum.FolderRename);
            }
        }

        private void AddToElementsToProcess(IEnumerable<string> elements, DisableActionTypeEnum actionType)
        {
            foreach (var data in elements)
            {
                var el = new DisableIntroductionAction
                {
                    Type = actionType,
                    InitialName = data
                };
                ElementsToProcess.Add(el);
            }
        }

        private void RemoveElement()
        {
            var findedElement = ElementsToProcess.FirstOrDefault(x => x.InitialName == SelectedElementToProcess.InitialName);

            if (findedElement != null) ElementsToProcess.Remove(findedElement);
        }

        //public class ElementToProcess
        //{
        //    public DisableActionTypeEnum ActionType { get; set; }
        //    public string FullIdentifier { get; set; }
        //}

        private void AddInstallFolder()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok && dialog.IsFolderPicker)
            {
                var folder = dialog.FileName;

                _installFolderService.AddInstallationFolder(folder);
                if (!InstallFolders.Contains(folder)) InstallFolders.Add(folder);
            }

            FocusEvent?.Invoke();
        }

        private void BrowseAndDetectPotentialGameFolders(string parentFolder)
        {
            PotentialGameFolders.Clear();
            PotentialGameFolders.AddRange(_installFolderService.GetSubFolders(parentFolder));
            SelectedGameFolder = PotentialGameFolders?.FirstOrDefault();
        }

        private void SetSelectedIllustrationPlatformEnum(string value)
        {
            _selectedIllustrationPlatformEnum = _illustrationDisplayableValue[value];
        }


        private void Test()
        {
            var gameSetting = GenerateGameSetting();
            var game =  _gameService.GetGameFromSettings(gameSetting);

            if (game.IsDetected && game.State == IntroductionStateEnum.Disabled)
            {
                _introductionActivationService.ReenableIntro(game);
            }
            else if (game.IsDetected && game.State == IntroductionStateEnum.Enabled)
            {
                _introductionActivationService.DisableIntro(game);
            }
            else
            {
                //TODO show info to user
            }
        }

        private void Cancel()
        {
            CloseEvent?.Invoke();
        }

        private void Save()
        {
            var gameSetting = GenerateGameSetting();
            var game = _gameService.GetGameFromSettings(gameSetting);

            if (game.IsDetected)
            {
                _gameService.SaveNewGame(gameSetting);

                //TODO Call API
                CloseEvent?.Invoke();
            }
            else
            {
                //TODO show info to user
            }
        }

        private void LaunchIllustrationHelp()
        {
            //TODO set in config file URL to doc
            Process.Start("https://github.com/NicolasConstant/Video-game-intro-skipper"); 
        }

        private GameSetting GenerateGameSetting()
        {
            if (!string.IsNullOrWhiteSpace(GameIllustrationUrl))
            {
                var isIllustrationValid = _illustrationValidationService.IsIllustrationValid(_selectedIllustrationPlatformEnum,
                        GameIllustrationUrl);
                if (!isIllustrationValid)
                {
                    //TODO show error to user
                    GameIllustrationUrl = string.Empty;
                }
            }

            return _gameService.GetGameSetting(Name, Publisher, Developer, SelectedInstallFolder, SelectedGameFolder, ElementsToProcess.ToList(), _selectedIllustrationPlatformEnum, GameIllustrationUrl);
        }
    }
}
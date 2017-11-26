using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Enums;
using VGIS.Domain.Services;
using VGIS.GUI.AddNewGame;
using VGIS.GUI.Annotations;
using VGIS.GUI.Options;
using VGIS.GUI.ViewModels;

namespace VGIS.GUI
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IntroEditionService _introEditionService;

        private string _filter = "";
        private ObservableCollection<GameViewModel> _detectedGames;

        public ObservableCollection<GameViewModel> DetectedGames
        {
            get => _detectedGames;
            set => SetProperty(ref _detectedGames, value);
        }

        public ICollectionView FilteredDetectedGames
        {
            get
            {
                var source = CollectionViewSource.GetDefaultView(DetectedGames);
                source.Filter = FilterGamesOnName;
                return source;
            }
        }

        private bool FilterGamesOnName(object game)
        {
            if (string.IsNullOrWhiteSpace(Filter)) return true;
            return ((GameViewModel)game).Name.ToLowerInvariant().Contains(Filter.ToLowerInvariant());
        }

        public string Filter
        {
            get => _filter;
            set
            {
                SetProperty(ref _filter, value);
                FilteredDetectedGames.Refresh();
            }
        }

        #region Ctor
        public MainWindowViewModel(IntroEditionService introEditionService)
        {
            _introEditionService = introEditionService;
            //Init commands
            ActivateAllCommand = new DelegateCommand(ActivateAll);
            DisableAllCommand = new DelegateCommand(DisableAll);
            RefreshCommand = new DelegateCommand(Refresh);
            AddNewGameCommand = new DelegateCommand(AddNewGame);
            OpenOptionsCommand = new DelegateCommand(OpenOptions);

            //Load games
            DetectedGames = new ObservableCollection<GameViewModel>();
            LoadGames();
        }
        #endregion

        public ICommand ActivateAllCommand { get; set; }

        public ICommand DisableAllCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand AddNewGameCommand { get; set; }

        public ICommand OpenOptionsCommand { get; set; }

        private void AddNewGame()
        {
            var newGameWindow = new AddNewGameView(new AddNewGameViewModel());
            newGameWindow.ShowDialog();
        }

        private void OpenOptions()
        {
            var optionsWindow = new OptionsView(new OptionsViewModel());
            optionsWindow.ShowDialog();
        }

        private void ActivateAll()
        {
            Task.Run(() =>
            {
                foreach (var gameViewModel in DetectedGames)
                {
                    if (gameViewModel.IsDetected &&
                        gameViewModel.IntroductionCurrentState != IntroductionStateEnum.Enabled)
                        gameViewModel.ChangeStateCommandTo(IntroductionStateEnum.Enabled);
                }
            });
        }

        private void DisableAll()
        {
            Task.Run(() =>
            {
                foreach (var gameViewModel in DetectedGames)
                {
                    if (gameViewModel.IsDetected &&
                        gameViewModel.IntroductionCurrentState != IntroductionStateEnum.Disabled)
                        gameViewModel.ChangeStateCommandTo(IntroductionStateEnum.Disabled);
                }
            });
        }

        private void Refresh()
        {
            Task.Run(() =>
            {
                DispatchToBackground(() =>
                {
                    DetectedGames.Clear();
                });
                Thread.Sleep(150); //Make sure user see the action
                LoadGames();
            });
        }

        private void LoadGames()
        {
            var games = _introEditionService.GetAllGames();
            foreach (var game in games)
            {
                DispatchToBackground(() =>
                {
                    DetectedGames.Add(new GameViewModel(game, _introEditionService));
                });
            }
            DispatchToBackground(() =>
            {
                FilteredDetectedGames.Refresh();
            });
        }

        private void DispatchToBackground(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }
    }
}

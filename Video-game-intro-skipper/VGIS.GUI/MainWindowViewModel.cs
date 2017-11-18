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
using VGIS.GUI.Annotations;
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

            //Load games
            DetectedGames = new ObservableCollection<GameViewModel>();
            LoadGames();
        }
        #endregion

        public ICommand ActivateAllCommand { get; set; }

        public ICommand DisableAllCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        private void ActivateAll()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    foreach (var gameViewModel in DetectedGames)
                    {
                        if (gameViewModel.IsDetected &&
                            gameViewModel.IntroductionCurrentState != IntroductionStateEnum.Enabled)
                            gameViewModel.ChangeStateCommandTo(IntroductionStateEnum.Enabled);
                    }
                }));
            });
        }

        private void DisableAll()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    foreach (var gameViewModel in DetectedGames)
                    {
                        if (gameViewModel.IsDetected &&
                            gameViewModel.IntroductionCurrentState != IntroductionStateEnum.Disabled)
                            gameViewModel.ChangeStateCommandTo(IntroductionStateEnum.Disabled);
                    }
                }));
            });
        }

        private void Refresh()
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    DetectedGames.Clear();
                    LoadGames();
                }));
            });
        }

        private void LoadGames()
        {
            var games = _introEditionService.GetAllGames();
            foreach (var game in games)
            {
                DetectedGames.Add(new GameViewModel(game, _introEditionService));
            }
            FilteredDetectedGames.Refresh();
        }
    }
}

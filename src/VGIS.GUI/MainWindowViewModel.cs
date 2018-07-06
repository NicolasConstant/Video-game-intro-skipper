﻿using System;
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
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Enums;
using VGIS.Domain.Services;
using VGIS.GUI.AddNewGame;
using VGIS.GUI.Annotations;
using VGIS.GUI.Options;
using VGIS.GUI.Structs;
using VGIS.GUI.ViewModels;

namespace VGIS.GUI
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IntroductionActivationService _introEditionService;
        private readonly GameService _gameService;
        private readonly IUnityContainer _container;

        private string _filter = "";
        private bool _allGamesVisible = false;
        private bool _canRemoveIntros = false;
        private string _detectedGamesVisibility = UiVisibilityStruct.Visible;
        private string _allGamesVisibility = UiVisibilityStruct.Collapsed;

        private ObservableCollection<GameViewModel> _detectedGames;

        #region Properties
        public string AllGamesVisibility
        {
            get => _allGamesVisibility;
            set => SetProperty(ref _allGamesVisibility, value);
        }

        public string DetectedGamesVisibility
        {
            get => _detectedGamesVisibility;
            set => SetProperty(ref _detectedGamesVisibility, value);
        }

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
                AnalyseIfIntroAreStillNotDisabled();
            }
        }

        public bool CanRemoveIntros
        {
            get => _canRemoveIntros;
            set => SetProperty(ref _canRemoveIntros, value);
        }
        #endregion

        #region Ctor
        public MainWindowViewModel(IntroductionActivationService introEditionService, GameService gameService, IUnityContainer container)
        {
            _introEditionService = introEditionService;
            _gameService = gameService;
            _container = container;

            //Init commands
            ActivateAllCommand = new DelegateCommand(ActivateAll);
            DisableAllCommand = new DelegateCommand(DisableAll);
            RefreshCommand = new DelegateCommand(Refresh);
            AddNewGameCommand = new DelegateCommand(AddNewGame);
            OpenOptionsCommand = new DelegateCommand(OpenOptions);
            ToogleGamesVisibilityCommand = new DelegateCommand(ToogleGamesVisibility);

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

        public ICommand ToogleGamesVisibilityCommand { get; set; }

        private void AddNewGame()
        {
            var newGameWindow = _container.Resolve<AddNewGameView>();
            newGameWindow.ShowDialog();
            Refresh();
        }

        private void OpenOptions()
        {
            var optionsWindow = _container.Resolve<OptionsView>();
            optionsWindow.ShowDialog();
            Refresh();
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
            var games = _gameService.GetAllGames(!_allGamesVisible);
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
                AnalyseIfIntroAreStillNotDisabled();
            });
        }

        private void AnalyseIfIntroAreStillNotDisabled()
        {
            var filteredElements = FilteredDetectedGames.Cast<GameViewModel>().ToList();
            var introEnabledElements = filteredElements
                .Where(x => x.IsDetected && x.IntroductionCurrentState == IntroductionStateEnum.Enabled).Select(x => x)
                .ToList();
            CanRemoveIntros = introEnabledElements.Any();
        }

        private void ToogleGamesVisibility()
        {
            if (_allGamesVisible)
            {
                AllGamesVisibility = UiVisibilityStruct.Collapsed;
                DetectedGamesVisibility = UiVisibilityStruct.Visible;
            }
            else
            {
                AllGamesVisibility = UiVisibilityStruct.Visible;
                DetectedGamesVisibility = UiVisibilityStruct.Collapsed;
            }
            _allGamesVisible = !_allGamesVisible;

            Refresh();
        }

        private void DispatchToBackground(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }
    }
}

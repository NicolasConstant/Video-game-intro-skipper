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
using System.Windows.Threading;
using Prism.Mvvm;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Services;
using VGIS.GUI.Annotations;
using VGIS.GUI.ViewModels;

namespace VGIS.GUI
{
    public class MainWindowViewModel : BindableBase
    {
        private string _filter = "test";
        private ObservableCollection<GameViewModel> _detectedGames;

        public ObservableCollection<GameViewModel> DetectedGames
        {
            get => _detectedGames;
            set => SetProperty(ref _detectedGames, value);
        }

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        #region Ctor
        public MainWindowViewModel(IntroEditionService introEditionService)
        {
            //Load games
            DetectedGames = new ObservableCollection<GameViewModel>();
            var games = introEditionService.GetAllGames();
            foreach (var game in games)
            {
                DetectedGames.Add(new GameViewModel(game, introEditionService));
            }
        }
        #endregion
    }
}

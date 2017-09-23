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
using VGIS.GUI.Annotations;

namespace VGIS.GUI
{
    public class MainWindowViewModel : BindableBase
    {
        private string _filter = "test";
        private ObservableCollection<DetectedGame> _detectedGames;

        public ObservableCollection<DetectedGame> DetectedGames
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
        public MainWindowViewModel()
        {
            var list = new List<DetectedGame>();
            for (var i = 0; i < 15; i++)
            {
                list.Add(new DetectedGame()
                {
                    ImageUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/493340/header.jpg?t=1504868428",
                });
            }
            DetectedGames = new ObservableCollection<DetectedGame>(list);


            Task.Run(() =>
            {
                Filter = "go";
                Thread.Sleep(3000);
                Filter = "loaded";
                for (var i = 0; i < 15; i++)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        DetectedGames.Add(new DetectedGame()
                        {
                            ImageUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/493340/header.jpg?t=1504868428",
                        });
                    }));
                }
            });
        }
        #endregion
    }

    public class DetectedGame : BindableBase
    {
        private string _imageUrl;

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }
    }
}

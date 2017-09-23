using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VGIS.GUI.Annotations;

namespace VGIS.GUI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DetectedGame> DetectedGames; 
        public string Filter { get; set; } = "filter data";

        #region Ctor
        public MainWindowViewModel()
        {
            DetectedGames = new ObservableCollection<DetectedGame>();
            for (var i = 0; i < 15; i++)
            {
                DetectedGames.Add(new DetectedGame()
                {
                    ImageUrl = "http://cdn.edgecast.steamstatic.com/steam/apps/493340/header.jpg?t=1504868428",
                });
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DetectedGame
    {
        public string ImageUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace VGIS.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var viewModel = new MainWindowViewModel();
            var view = new MainWindow(viewModel);

            Application.Current.MainWindow = view;
            Application.Current.MainWindow.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VGIS.GUI.AddNewGame
{
    /// <summary>
    /// Interaction logic for AddNewGameView.xaml
    /// </summary>
    public partial class AddNewGameView : Window
    {
        public AddNewGameView(AddNewGameViewModel viewModel)
        {
            viewModel.FocusEvent += () => { Focus(); };
            viewModel.CloseEvent += () => { Close(); };
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}

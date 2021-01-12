using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFCovidItalyAnalizer.ViewModel;

namespace WPFCovidItalyAnalizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MenuVM menuVM;
        public MainWindow()
        {
            InitializeComponent();

            menuVM = Resources["vmMenu"] as MenuVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton ctrl = sender as ToggleButton;
            menuVM.Select(ctrl.Name);
        }
    }
}

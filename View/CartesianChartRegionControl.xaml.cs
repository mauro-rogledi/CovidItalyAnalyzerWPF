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
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFCovidItalyAnalizer.ViewModel;

namespace WPFCovidItalyAnalizer.View
{
    /// <summary>
    /// Interaction logic for CartesianChartRegionControl.xaml
    /// </summary>
    public partial class CartesianChartRegionControl : UserControl
    {
        CartesianRegionVM viewModel;

        public CartesianChartRegionControl()
        {
            InitializeComponent();
            viewModel = Resources["VM"] as CartesianRegionVM;
        }

        public void Refresh()
        {
            viewModel.Refresh();
        }
    }
}

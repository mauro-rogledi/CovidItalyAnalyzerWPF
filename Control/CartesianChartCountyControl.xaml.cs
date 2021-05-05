using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;
using WPFCovidItalyAnalizer.ViewModel;

namespace WPFCovidItalyAnalizer.Control
{
    /// <summary>
    /// Interaction logic for CartesianChartCountyControl.xaml
    /// </summary>
    public partial class CartesianChartCountyControl : UserControl
    {

        ChartViewModel chartViewModel { get => DataContext as ChartViewModel; }

        public CartesianChartCountyControl()
        {
            InitializeComponent();
        }
      
        public void Refresh()
        {
            chartViewModel?.SetCartesianChart(new CartesianChartCountyManager(CartesianChart));
            chartViewModel?.Refresh();
        }
    }
}

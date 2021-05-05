using LiveCharts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;
using WPFCovidItalyAnalizer.ViewModel;

namespace WPFCovidItalyAnalizer.Control
{
    /// <summary>
    /// Interaction logic for CartesianChartRegionControl.xaml
    /// </summary>
    public partial class CartesianChartRegionControl : UserControl
    {
        ChartViewModel chartViewModel { get => DataContext as ChartViewModel; }
        public CartesianChartRegionControl()
        {
            InitializeComponent();

        }

        public void Refresh()
        {
            chartViewModel?.SetCartesianChart(new CartesianChartRegionManager(CartesianChart));
            chartViewModel?.Refresh();
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;
using WPFCovidItalyAnalizer.ViewModel;

namespace WPFCovidItalyAnalizer.Control
{
    /// <summary>
    /// Interaction logic for PieChartItalyControl.xaml
    /// </summary>
    public partial class PieChartItalyControl : UserControl
    {
        ChartViewModel chartViewModel { get => DataContext as ChartViewModel; }

        public PieChartItalyControl()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            chartViewModel?.SetCartesianChart(new PieChartItalyManager(PieChart));
            chartViewModel?.Refresh();

            //int numRegion = DataReaderRegion.ReadRegions().Count();
            //TopData.Add(new ComboData() { display = "5", value = 5 });
            //TopData.Add(new ComboData() { display = "10", value = 10 });
            //TopData.Add(new ComboData() { display = "15", value = 15 });
            //TopData.Add(new ComboData() { display = "Tutti", value = numRegion });

            //TopSelected = TopData[0];
        }

        //private void RefreshChart()
        //{
        //    chartManager?.SetChart(ChartSelected);
        //    PieTitle = chartManager?.Title;
        //}
    }
}
using LiveCharts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.View
{
    /// <summary>
    /// Interaction logic for CartesianChartRegionControl.xaml
    /// </summary>
    public partial class CartesianChartRegionControl : UserControl
    {
        public ObservableCollection<Model.ComboData> RegionDatas { get; set; }
        public ObservableCollection<string> ChartDatas { get; set; }

        public SeriesCollection SeriesViews { get; set; }

        private Model.ComboData regionSelected;

        public Model.ComboData RegionSelected
        {
            get { return regionSelected; }
            set
            {
                regionSelected = value;
                RefreshChart();
            }
        }

        private string chartSelected;

        public string ChartSelected
        {
            get { return chartSelected; }
            set
            {
                chartSelected = value;
                RefreshChart();
            }
        }

        readonly CartesianChartRegionManager chartManager;

        public CartesianChartRegionControl()
        {
            InitializeComponent();

            RegionDatas = new ObservableCollection<Model.ComboData>();
            ChartDatas = new ObservableCollection<string>();

            chartManager = new CartesianChartRegionManager(CartesianChart)
            {
                Region = () => regionSelected
            };

            chartManager.GetChartAvailable().ToList().ForEach(e => { ChartDatas.Add(e); });
        }

        public void Refresh()
        {
            DataReaderRegion.ReadRegions()
            .Select(r => new ComboData() { value = r.codice_regione, display = r.denominazione_regione })
            .ToList()
            .ForEach((e) =>
            {
                RegionDatas.Add(e);
            });
        }
        private void RefreshChart()
        {
            if (!string.IsNullOrEmpty(chartSelected) && regionSelected != null)
                chartManager.SetChart(chartSelected, regionSelected.value, regionSelected.display);
        }
    }
}

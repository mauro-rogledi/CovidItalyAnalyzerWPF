using LiveCharts.Wpf;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.ViewModel
{
    public class ChartViewModel : BaseVM
    {
        private CartesianChartRegionManager chartManager;
        public ObservableCollection<string> ChartAvailable { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<ComboData> RegionDatas { get; set; } = new ObservableCollection<ComboData>();
        public ObservableCollection<ComboData> CountynDatas { get; set; } = new ObservableCollection<ComboData>();

        private ComboData regionSelected;

        internal void SetCartesianChart(CartesianChart cartesianChart)
        {
            chartManager = new CartesianChartRegionManager(cartesianChart)
            {
                Region = () => regionSelected,
                FromDate = () => dateFrom,
                ToDate = () => dateTo
            };
            ChartAvailable.Clear();
            chartManager.GetChartAvailable().ToList().ForEach(e => { ChartAvailable.Add(e); });
        }

        public ComboData RegionSelected
        {
            get { return regionSelected; }
            set
            {
                SetValue<ComboData>(ref regionSelected, value);
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

        private void RefreshChart()
        {
            if (!string.IsNullOrEmpty(chartSelected) && regionSelected != null)
                chartManager.SetChart(chartSelected, regionSelected.value, regionSelected.display);
        }

        private DateTime dateFrom;

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { SetValue<DateTime>(ref dateFrom, value); RefreshChart(); }
        }

        private DateTime dateTo;

        public DateTime DateTo
        {
            get { return dateTo; }
            set { SetValue<DateTime>(ref dateTo, value); RefreshChart(); }
        }

        public ChartViewModel()
        {
            DateFrom = new DateTime(2020, 2, 28);
            DateTo = DateTime.Today;
        }

        public void Refresh()
        {
            RegionDatas.Clear();

            DataReaderRegion.ReadRegions()
            .Select(r => new ComboData() { value = r.codice_regione, display = r.denominazione_regione })
            .ToList()
            .ForEach((e) =>
            {
                RegionDatas.Add(e);
            });
        }

    }
}

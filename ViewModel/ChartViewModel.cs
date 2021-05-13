using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;

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
        private IChartManager chartManager = null;
        public ObservableCollection<string> ChartAvailable { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<ComboData> RegionDatas { get; set; } = new ObservableCollection<ComboData>();
        public ObservableCollection<ComboData> CountyDatas { get; set; } = new ObservableCollection<ComboData>();
        public ObservableCollection<ComboData> TopData { get; set; } = new ObservableCollection<ComboData>();

        private ComboData regionSelected;

        internal void SetCartesianChart(IChartManager chartManager)
        {
            this.chartManager = chartManager;
            this.chartManager.Region = () => regionSelected;
            this.chartManager.County = () => countySelected;
            this.chartManager.FromDate = () => dateFrom;
            this.chartManager.ToDate = () => dateTo;

            this.chartManager.UpdateTitle += ChartManager_UpdateTitle;

            ChartAvailable.Clear();
            chartManager.GetChartAvailable().ToList().ForEach(e => { ChartAvailable.Add(e); });
        }

        private void ChartManager_UpdateTitle(object sender, string title)
        {
            PieTitle = title;
        }

        private string pieTitle;

        public string PieTitle
        {
            get { return pieTitle; }
            set { SetValue<string>(ref pieTitle, value); }
        }


        public ComboData RegionSelected
        {
            get { return regionSelected; }
            set
            {
                SetValue<ComboData>(ref regionSelected, value);
                RefreshCounty();
                RefreshChart();
            }
        }

        private void RefreshCounty()
        {
            CountyDatas.Clear();

            DataReaderCounty.ReadCounties(regionSelected?.value ?? 0)
                .Select(r => new ComboData() { value = r.codice_provincia, display = r.denominazione_provincia })
                .ToList()
                .ForEach((e) => { CountyDatas.Add(e); });
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

        private ComboData countySelected;

        public ComboData CountySelected
        {
            get { return countySelected; }
            set
            {
                countySelected = value;
                RefreshChart();
            }
        }

        private ComboData topSelected;

        public ComboData TopSelected
        {
            get { return topSelected; }
            set
            {
                topSelected = value;
                RefreshChart();
            }
        }

        private void RefreshChart()
        {
            if (!string.IsNullOrEmpty(chartSelected))
                chartManager.SetChart(chartSelected, regionSelected?.value ??0, countySelected?.value ?? 0, regionSelected?.display ??"");
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


            int numRegion = DataReaderRegion.ReadRegions().Count();
            TopData.Add(new ComboData() { display = "5", value = 5 });
            TopData.Add(new ComboData() { display = "10", value = 10 });
            TopData.Add(new ComboData() { display = "15", value = 15 });
            TopData.Add(new ComboData() { display = "Tutti", value = numRegion });

            TopSelected = TopData[0];

        }

    }
}

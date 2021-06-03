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
                WriteRegionSetting();
            }
        }

        private void WriteRegionSetting()
        {
            if (SettingManager.RememberLastData)
                SettingManager.Region = regionSelected?.value ?? 0;
        }

        private void RefreshCounty()
        {
            CountyDatas.Clear();

            DataReaderCounty.ReadCounties(regionSelected?.value ?? 0)
                .Select(r => new ComboData() { value = r.codice_provincia, display = r.denominazione_provincia })
                .ToList()
                .ForEach((e) => { CountyDatas.Add(e); });

            if (SettingManager.RememberLastData)
                CountySelected = CountyDatas.FirstOrDefault((c) => c.value == SettingManager.County);
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
                SetValue<ComboData>(ref countySelected, value);
                RefreshChart();
                WriteCountySetting();
            }
        }

        private void WriteCountySetting()
        {
            if (SettingManager.RememberLastData)
                SettingManager.County = countySelected?.value ?? 0;
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


        private DateTime displayDateFrom;

        public DateTime DisplayDateFrom
        {
            get { return displayDateFrom; }
            set { SetValue<DateTime>(ref displayDateFrom, value); }
        }

        private DateTime displayDateEnd;

        public DateTime DisplayDateEnd
        {
            get { return displayDateEnd; }
            set { SetValue<DateTime>(ref displayDateEnd, value);}
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

            if (SettingManager.RememberLastData)
                RegionSelected = RegionDatas.FirstOrDefault((c) => c.value == SettingManager.Region);

            DisplayDateFrom = DateFrom = DataReaderRegion
                .ReadRegionData(1)
                .OrderBy(x =>x.data)
                .Select(x => x.data)
                .First();

            DisplayDateEnd = DateTo = DataReaderRegion
                .ReadRegionData(1)
                .OrderByDescending(x => x.data)
                .Select(x => x.data)
                .First();
        }

    }
}

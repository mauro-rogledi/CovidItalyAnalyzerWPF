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

namespace WPFCovidItalyAnalizer.Control
{
    /// <summary>
    /// Interaction logic for CartesianChartCountyControl.xaml
    /// </summary>
    public partial class CartesianChartCountyControl : UserControl, INotifyPropertyChanged
    {

        readonly CartesianChartCountyManager chartManager;
        private string chartSelected;
        private ComboData countySelected;
        private ComboData regionSelected;
        public CartesianChartCountyControl()
        {
            InitializeComponent();

            RegionDatas = new ObservableCollection<ComboData>();
            CountyDatas = new ObservableCollection<ComboData>();
            ChartDatas = new ObservableCollection<string>();

            chartManager = new CartesianChartCountyManager(CartesianChart);
            chartManager.GetChartAvailable().ToList().ForEach(e => { ChartDatas.Add(e); });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<string> ChartDatas { get; set; }
        public string ChartSelected
        {
            get { return chartSelected; }
            set
            {
                chartSelected = value;
                RefreshChart();
            }
        }

        public ObservableCollection<ComboData> CountyDatas { get; set; }

        public ComboData CountySelected
        {
            get { return countySelected; }
            set
            {
                countySelected = value;
                RefreshChart();
            }
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

        public ObservableCollection<ComboData> RegionDatas { get; set; }

        public ComboData RegionSelected
        {
            get { return regionSelected; }
            set
            {
                SetValue<ComboData>(ref regionSelected, value);

                CountyDatas.Clear();

                DataReaderCounty.ReadCounties(regionSelected?.value ?? 0)
                    .Select(r => new ComboData() { value = r.codice_provincia, display = r.denominazione_provincia })
                    .ToList()
                    .ForEach((e) => { CountyDatas.Add(e); });

                RefreshChart();
            }
        }

        private void RefreshChart()
        {
            if (!string.IsNullOrEmpty(chartSelected) && regionSelected != null && countySelected != null)
                chartManager.SetChart(chartSelected, regionSelected.value, countySelected.value, regionSelected.display);
        }

        public void SetValue<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(field, newValue) == false)
            {
                field = newValue;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
    }
}

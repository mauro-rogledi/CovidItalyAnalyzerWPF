using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.Control
{
    /// <summary>
    /// Interaction logic for PieChartItalyControl.xaml
    /// </summary>
    public partial class PieChartItalyControl : UserControl, INotifyPropertyChanged
    {
        private readonly PieChartItalyManager chartManager;
        private string chartSelected = string.Empty;
        private DateTime dateFrom = DateTime.Today;
        private DateTime dateTo = DateTime.Today;
        private string pieTitle;
        private ComboData topSelected;
        public PieChartItalyControl()
        {
            InitializeComponent();

            TopData = new ObservableCollection<ComboData>();
            ChartDatas = new ObservableCollection<string>();

            chartManager = new PieChartItalyManager(PieChart)
            {
                FromDate = () => DateFrom,
                ToDate = () => DateTo,
                Top = () => TopSelected
            };
            chartManager.GetChartAvailable().ToList().ForEach(e => { ChartDatas.Add(e); });

            dtpFrom.SelectedDateFormat = DatePickerFormat.Short;
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
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                SetValue<DateTime>(ref dateFrom, value);
                if (DateTo < DateFrom)
                    DateTo = DateFrom;

                RefreshChart();
            }
        }

        public DateTime DateTo
        {
            get { return dateTo; }
            set
            {
                SetValue<DateTime>(ref dateTo, value);
                if (DateFrom > DateTo)
                    DateFrom = DateTo;
                RefreshChart();
            }
        }

        public string PieTitle
        {
            get { return pieTitle; }
            set { SetValue<string>(ref pieTitle,value); }
        }
        public ObservableCollection<ComboData> TopData { get; set; }
        public ComboData TopSelected
        {
            get { return topSelected; }
            set
            {
                SetValue<ComboData>(ref topSelected,  value);
                RefreshChart();
            }
        }
        public void Refresh()
        {
            int numRegion = DataReaderRegion.ReadRegions().Count();
            TopData.Add(new ComboData() { display = "5", value = 5 });
            TopData.Add(new ComboData() { display = "10", value = 10 });
            TopData.Add(new ComboData() { display = "15", value = 15 });
            TopData.Add(new ComboData() { display = "Tutti", value = numRegion });

            TopSelected = TopData[0];
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

        private void OnPropertyChanged([CallerMemberName] string v = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        private void RefreshChart()
        {
            chartManager?.SetChart(ChartSelected);
            PieTitle = chartManager?.Title;
        }
    }
}
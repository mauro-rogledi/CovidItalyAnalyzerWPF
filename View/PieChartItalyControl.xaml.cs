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

namespace WPFCovidItalyAnalizer.View
{
    /// <summary>
    /// Interaction logic for PieChartItalyControl.xaml
    /// </summary>
    public partial class PieChartItalyControl : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<ComboData> TopData { get; set; }
        public ObservableCollection<string> ChartDatas { get; set; }

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

        private DateTime dateFrom = DateTime.Today;

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { 
                SetValue<DateTime>(ref dateFrom, value);
                if (DateTo < DateFrom)
                    DateTo = DateFrom;

                RefreshChart();
            }
        }

        private DateTime dateTo = DateTime.Today;

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

        private string chartSelected = string.Empty;

        public string ChartSelected
        {
            get { return chartSelected; }
            set
            {
                chartSelected = value;
                RefreshChart();
            }
        }

        readonly PieChartItalyManager chartManager;

        public event PropertyChangedEventHandler PropertyChanged;

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
        }

        public void Refresh()
        {
            int numRegion = DataReaderRegion.ReadRegions().Count();
            TopData.Add(new ComboData() { display = "5", value = 5 });
            TopData.Add(new ComboData() { display = "10", value = 10 });
            TopData.Add(new ComboData() { display = "15", value = 15 });
            TopData.Add(new ComboData() { display = "Tutti", value = numRegion });
        }

        private void RefreshChart()
        {
            chartManager?.SetChart(ChartSelected);
            //mtlTitle.Text = chartManager?.Title;
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
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for DateRangeControl.xaml
    /// </summary>
    public partial class DateRangeControl : UserControl
    {

        private DateTime Today = DateTime.Today;
        public ObservableCollection<ComboData> RangeType { get; set; } = new ObservableCollection<ComboData>();

        public DateRangeControl()
        {
            InitializeComponent();
            ComboHelper.FillComboWithEnum<DateRange>(RangeType, (DateRange x) => true);
            range.ItemsSource = RangeType;
            range.SelectedItem = RangeType.Where(x => x.value == (int)DateRange.AllDate).FirstOrDefault();

            Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            if (Thread.CurrentThread.CurrentCulture.Name == "it-IT")
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            else
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
        }

        public DateTime? DateFrom
        {
            get { return (DateTime)GetValue(DateFromProperty); }
            set { SetValue(DateFromProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateFrom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateFromProperty =
            DependencyProperty.Register("DateFrom", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Today, (o, a) => (o as DateRangeControl).OnDateFromChanged()));

        private void OnDateFromChanged()
        {
            dateFrom.SelectedDate = DateFrom;
        }

        public DateTime? DateTo
        {
            get { return (DateTime)GetValue(DateToProperty); }
            set { SetValue(DateToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateFrom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateToProperty =
            DependencyProperty.Register("DateTo", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Today, (o, a) => (o as DateRangeControl).OnDateToChanged()));

        private void OnDateToChanged()
        {
            dateTo.SelectedDate = DateTo;
        }

        public DateTime? DisplayDateStart
        {
            get { return (DateTime)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayDateStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayDateStartProperty =
            DependencyProperty.Register("DisplayDateStart", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Today, (o, a) => (o as DateRangeControl).OnDisplayDateStartChanged()));

        private void OnDisplayDateStartChanged()
        {
            dateFrom.DisplayDateStart = DisplayDateStart;
            dateTo.DisplayDateStart = DisplayDateStart;
        }

        public DateTime? DisplayDateEnd
        {
            get { return (DateTime)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayDateStart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayDateEndProperty =
            DependencyProperty.Register("DisplayDateEnd", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Today, (o, a) => (o as DateRangeControl).OnDisplayDateEndChanged()));

        private void OnDisplayDateEndChanged()
        {
            dateFrom.DisplayDateEnd = DisplayDateEnd;
            dateTo.DisplayDateEnd = DisplayDateEnd;
            Today = DisplayDateEnd ?? DateTime.Today;
        }

        private void dateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFrom = dateFrom.SelectedDate;
        }

        private void dateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTo = dateTo.SelectedDate;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeRangeDate((DateRange)(range.SelectedItem as ComboData).value);
        }

        private void ChangeRangeDate(DateRange selectedValue)
        {
            DateTime dateto = DateTo ?? DateTime.Today;
            switch (selectedValue)
            {
                case DateRange.Today:
                    DateFrom = Today;
                    DateTo = Today;
                    break;
                case DateRange.ThisWeek:
                    DateTo = Today;
                    DateFrom = Today.StartOfWeek(DayOfWeek.Monday);
                    break;
                case DateRange.LastSevenDays:
                    DateTo = Today;
                    DateFrom = DateTo?.AddDays(-6);
                    break;
                case DateRange.LastTwoWeeks:
                    DateTo = Today;
                    DateFrom = Today.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                    break;
                case DateRange.ThisMonth:
                    DateTo = Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1);
                    break;
                case DateRange.LastThirtyDays:
                    DateTo = Today;
                    DateFrom = DateTo?.AddDays(-30);
                    break;
                case DateRange.LastTwoMonths:
                    DateTo = Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1).AddMonths(-1);
                    break;
                case DateRange.LastThreeMonths:
                    DateTo = Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1).AddMonths(-2);
                    break;
                case DateRange.LastSixMonths:
                    DateTo = Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1).AddMonths(-5);
                    break;
                case DateRange.LastYear:
                    DateTo = Today;
                    DateFrom = dateto.AddYears(-1);
                    break;
                case DateRange.AllDate:
                    DateFrom = dateFrom.DisplayDateStart;
                    DateTo = Today;
                    break;
            }
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}

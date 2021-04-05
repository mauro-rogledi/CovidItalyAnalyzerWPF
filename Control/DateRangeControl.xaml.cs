using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for DateRangeControl.xaml
    /// </summary>
    public partial class DateRangeControl : UserControl
    {
        public ObservableCollection<ComboData> RangeType { get; set; } = new ObservableCollection<ComboData>();

        public DateRangeControl()
        {
            InitializeComponent();
            ComboHelper.FillComboWithEnum<DateRange>(RangeType, (DateRange x) => true);
            range.ItemsSource = RangeType;
            range.SelectedItem = RangeType.Where(x => x.value == (int)DateRange.AllDate).FirstOrDefault();

            dateFrom.SelectedDate = new DateTime(2020,2,28);
            dateTo.SelectedDate = DateTime.Today;
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
                    DateFrom = DateTime.Today;
                    DateTo = DateTime.Today;
                    break;
                case DateRange.ThisWeek:
                    DateTo = DateTime.Today;
                    DateFrom = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
                    break;
                case DateRange.LastSevenDays:
                    DateTo = DateTime.Today;
                    DateFrom = DateTo?.AddDays(-6);
                    break;
                case DateRange.LastTwoWeeks:
                    DateTo = DateTime.Today;
                    DateFrom = DateTime.Today.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                    break;
                case DateRange.ThisMonth:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1);
                    break;
                case DateRange.LastThirtyDays:
                    DateTo = DateTime.Today;
                    DateFrom = DateTo?.AddDays(-30);
                    break;
                case DateRange.LastTwoMonths:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1).AddMonths(-1);
                    break;
                case DateRange.LastThreeMonths:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1).AddMonths(-2);
                    break;
                case DateRange.LastSixMonths:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(dateto.Year, dateto.Month, 1).AddMonths(-5);
                    break;
                case DateRange.AllDate:
                    DateFrom = new DateTime(2020, 2, 24);
                    DateTo = DateTime.Today;
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

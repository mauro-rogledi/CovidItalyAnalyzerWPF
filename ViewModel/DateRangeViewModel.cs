using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;




namespace WPFCovidItalyAnalizer.ViewModel
{
    public class DateRangeViewModel : BaseVM
    {
        public ObservableCollection<ComboData> RangeType { get; set; } = new ObservableCollection<ComboData>();

        private DateTime dateFrom;

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { SetValue<DateTime>(ref dateFrom, value); }
        }

        private DateTime dateTo;

        public DateTime DateTo
        {
            get { return dateTo; }
            set { SetValue<DateTime>(ref dateTo, value); }
        }

        private DateRange selectedValue;

        public DateRange SelectedValue
        {
            get { return selectedValue; }
            set
            {
                SetValue<DateRange>(ref selectedValue, value);
                ChangeRangeDate();
            }
        }

        private void ChangeRangeDate()
        {
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
                    DateFrom = DateTo.AddDays(-6); 
                    break;
                case DateRange.LastTwoWeeks:
                    DateTo = DateTime.Today;
                    DateFrom = DateTime.Today.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                    break;
                case DateRange.ThisMonth:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(DateTo.Year, DateTo.Month, 1);
                    break;
                case DateRange.LastThirtyDays:
                    DateTo = DateTime.Today;
                    DateFrom = DateTo.AddDays(-30);
                    break;
                case DateRange.LastTwoMonths:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(DateTo.Year, DateTo.Month, 1).AddMonths(-1);
                    break;
                case DateRange.LastThreeMonths:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(DateTo.Year, DateTo.Month, 1).AddMonths(-2);
                    break;
                case DateRange.LastSixMonths:
                    DateTo = DateTime.Today;
                    DateFrom = new DateTime(DateTo.Year, DateTo.Month, 1).AddMonths(-5);
                    break;
                case DateRange.All:
                    DateFrom = new DateTime(2020, 2, 24);
                    DateTo = DateTime.Today;
                    break;
            }
        }

        public DateRangeViewModel()
        {
            ComboHelper.FillComboWithEnum<DateRange>(RangeType, (DateRange x) => true);
            selectedValue = DateRange.All;
            dateFrom = new DateTime(2020, 2, 24);
            dateTo = DateTime.Today;
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

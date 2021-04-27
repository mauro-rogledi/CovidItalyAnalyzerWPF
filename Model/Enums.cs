using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCovidItalyAnalizer.Model
{
   public enum DateRange
    {
        Today,
        ThisWeek,
        LastSevenDays,
        LastTwoWeeks,
        ThisMonth,
        LastThirtyDays,
        LastTwoMonths,
        LastThreeMonths,
        LastSixMonths,
        LastYear,
        AllDate
    }
}

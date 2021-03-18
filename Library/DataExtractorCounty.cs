using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCovidItalyAnalizer.Library
{
    public class DataExtractorCounty
    {
        static CultureInfo myCI = CultureInfo.CurrentCulture;
        static Calendar myCal = myCI.Calendar;
        static CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
        static DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;


        internal static List<ReturnData> FillDailyCases(int region, int county)
        {
            var list = DataReaderCounty.ReadCountyData(region, county)
                .ToList();

            return list
                .Select((curr, i) => new ReturnData()
                {
                    data = curr.data,
                    value = i > 0 ? curr.totale_casi - list[i - 1].totale_casi : curr.totale_casi,
                    lbl = curr.data.ToString("dd/MM/yy")
                }
                )
                .ToList();
        }
        internal static List<ReturnData> FillWeeklyCases(int region, int county)
        {
            return FillDailyCases(region, county)
                .GroupBy(g => $"{g.data.Year}-{myCal.GetWeekOfYear(g.data, myCWR, myFirstDOW)}")
                .Select((s) => new ReturnData
                {
                    data = s.Max(f => f.data),
                    lbl = s.Max(f => f.data).ToString("dd/MM/yy"),
                    value = s.Sum(c => c.value)
                }
                )
                .ToList();
        }

        internal static List<ReturnData> FillTotalyCases(int region, int county)
        {
            return DataReaderCounty.ReadCountyData(region, county)
                .OrderBy(d => d.data)
                .Select((s) => new ReturnData
                {
                    data = s.data,
                    lbl = s.data.ToString("dd/MM/yy"),
                    value = s.totale_casi
                }
                )
                .ToList();
        }
    }
}

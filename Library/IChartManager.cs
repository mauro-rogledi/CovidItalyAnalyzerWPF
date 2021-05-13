using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.Library
{
    public interface IChartManager
    {
        Func<ComboData> Region { get; set; }

        Func<ComboData> County { get; set; }
        Func<DateTime> FromDate { get; set; }
        Func<DateTime> ToDate { get; set; }

        string[] GetChartAvailable();

        void SetChart(string chart, int region, int county, string display);

        event EventHandler<string> UpdateTitle;
    }
}

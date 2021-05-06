using LiveCharts;
using LiveCharts.Wpf;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.Library
{
    public class CartesianChartCountyManager : IChartManager
    {
        private readonly CultureInfo myCI;
        private readonly Calendar myCal;
        private readonly CalendarWeekRule myCWR;
        private readonly DayOfWeek myFirstDOW;
        private readonly CartesianChart chart;

        public Func<ComboData> Region { get; set; }

        public Func<ComboData> County { get; set; }
        public Func<DateTime> FromDate { get; set; }
        public Func<DateTime> ToDate { get; set; }

        private readonly Dictionary<string, Action<int, int, string>> ChartAvailable = new Dictionary<string, Action<int, int, string>>();

        public CartesianChartCountyManager(CartesianChart chart)
        {
            this.chart = chart;
            myCI = CultureInfo.CurrentCulture;
            myCal = myCI.Calendar;
            myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            ChartAvailable.Add(Properties.Resources.DailyCases, (int r, int c, string s) => FillChartWithDailyCases(r, c, s));
            ChartAvailable.Add(Properties.Resources.WeeklyCases, (int r, int c, string s) => FillChartWithWeeklyCases(r, c, s));
            ChartAvailable.Add(Properties.Resources.TotalCases, (int r, int c, string s) => FillChartWitTotalCases(r, c, s));
            ChartAvailable.Add(Properties.Resources.WeeklyCasesInhabitant, (int r, int c, string s) => FillChartWithWeeklyCasesInhabitant(r, c, s));
        }

        public string[] GetChartAvailable()
        {
            return ChartAvailable
                .Aggregate(new List<string>(), (acc, x) => { acc.Add(x.Key); return acc; })
                .ToArray();
        }

        public void SetChart(string chart, int region, int county, string display)
        {
            ChartAvailable[chart].Invoke(region, county, display);
        }

        public void FillChartWithDailyCases(int region, int county, string countyName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var data = DataExtractorCounty.FillDailyCases(region, county, dateFrom, dateTo);

            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"{Properties.Resources.DailyCases} {countyName}",
                    Values = new ChartValues<float>(data.Select(s => s.value < 0 ? 0 : s.value)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0")
                }
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = data.Select(s => s.lbl).ToList(),
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = 7,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = "Infetti",
                LabelFormatter = value => value.ToString("N0")
            });

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }

        public void FillChartWithWeeklyCases(int region, int county, string countyName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var data = DataExtractorCounty.FillWeeklyCases(region, county, dateFrom, dateTo);

            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = $"{Properties.Resources.WeeklyCases} {countyName}",
                    Values = new ChartValues<float>(data.Select(s => s.value < 0 ? 0 : s.value)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0")
                }
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = data.Select(s => s.lbl).ToList(),
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = "Infetti",
                LabelFormatter = value => value.ToString("N0")
            });

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }
        public void FillChartWithWeeklyCasesInhabitant(int region, int county, string countyName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var data = DataExtractorCounty.FillWeeklyCases(region, county, dateFrom, dateTo);
            var poples = 100000 / DataReaderPeople.ReadPeopleByCounty(region, county);

            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = $"{Properties.Resources.WeeklyCasesInhabitant} {countyName}",
                    Values = new ChartValues<float>(data.Select(s => (s.value < 0 ? 0 : s.value) * poples)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0")
                }
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = data.Select(s => s.lbl).ToList(),
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = "Infetti",
                LabelFormatter = value => value.ToString("N0")
            });

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }

        public void FillChartWitTotalCases(int region, int county, string countyName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var data = DataExtractorCounty.FillTotalyCases(region, county, dateFrom, dateTo);

            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"{Properties.Resources.DailyCases} {countyName}",
                    Values = new ChartValues<float>(data.Select(s => s.value<0 ? 0 : s.value)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = false,
                    LabelPoint = point => point.Y.ToString("N0")
                }
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = data.Select(s => s.lbl).ToList(),
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = 7,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = "Infetti",
                LabelFormatter = value => value.ToString("N0")
            });

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }
    }
}
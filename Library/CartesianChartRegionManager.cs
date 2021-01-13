using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.Library
{
    public class CartesianChartRegionManager
    {
        private CultureInfo myCI;
        private Calendar myCal;
        private CalendarWeekRule myCWR;
        private DayOfWeek myFirstDOW;
        private readonly CartesianChart chart;

        public Func<ComboRegionData> Region { get; set; }

        private readonly Dictionary<string, Action<int, string>> ChartAvailable = new Dictionary<string, Action<int, string>>();

        public CartesianChartRegionManager(CartesianChart chart)
        {
            this.chart = chart;
            myCI = CultureInfo.CurrentCulture;
            myCal = myCI.Calendar;
            myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            ChartAvailable.Add(Properties.Resources.DailyCases, (int r, string s) => FillChartWitNewPositives(r, s));
            ChartAvailable.Add(Properties.Resources.DailySwabs, (int r, string s) => FillChartWitDailySwabs(r, s));
            ChartAvailable.Add(Properties.Resources.DailyCasesSwabs, (int r, string s) => FillChartWithDailySwabCases(r, s));
            ChartAvailable.Add(Properties.Resources.DailyDeads, (int r, string s) => FillChartWitDailyDeads(r, s));
            ChartAvailable.Add(Properties.Resources.WeeklyCases, (int r, string s) => FillChartWithWeeklyCases(r, s));
            ChartAvailable.Add(Properties.Resources.WeeklySwabs, (int r, string s) => FillChartWithWeeklySwab(r, s));
            ChartAvailable.Add(Properties.Resources.WeeklyCasesSwabs, (int r, string s) => FillChartWithWeeklySwabCases(r, s));
            ChartAvailable.Add(Properties.Resources.WeeklyDeads, (int r, string s) => FillChartWithWeeklyDead(r, s));
            ChartAvailable.Add(Properties.Resources.TotalCases, (int r, string s) => FillChartWitTotalCases(r, s));
            ChartAvailable.Add(Properties.Resources.IntensiveCare, (int r, string s) => FillChartWitIntensiveCare(r, s));
            ChartAvailable.Add(Properties.Resources.Hospital, (int r, string s) => FillChartWitHospital(r, s));
        }

        public string[] GetChartAvailable()
        {
            return ChartAvailable
                .Aggregate(new List<string>(), (acc, x) => { acc.Add(x.Key); return acc; })
                .ToArray();
        }

        public void SetChart(string chart, int value, string display)
        {
            ChartAvailable[chart].Invoke(value, display);
        }

        private void FillChartWitIntensiveCare(int region, string regionName)
        {
            FillChartWithLinearSeries(region, regionName, Properties.Resources.IntensiveCare, Properties.Resources.IntensiveCare, DataExtractorRegion.FillIntensiveCare);
        }

        private void FillChartWitHospital(int region, string regionName)
        {
            FillChartWithLinearSeries(region, regionName, Properties.Resources.Hospital, Properties.Resources.Hospital, DataExtractorRegion.FillHospital);
        }

        private void FillChartWitDailyDeads(int region, string regionName)
        {
            FillChartWithLinearSeries(region, regionName, Properties.Resources.DailyDeads, Properties.Resources.Deads, DataExtractorRegion.FillDailyDeads);
        }

        private void FillChartWitDailySwabs(int region, string regionName)
        {
            FillChartWithLinearSeries(region, regionName, Properties.Resources.DailySwabs, Properties.Resources.Swabs, DataExtractorRegion.FillDailySwabs);
        }

        public void FillChartWitNewPositives(int region, string regionName)
        {
            FillChartWithLinearSeries(region, regionName, Properties.Resources.DailyCases, Properties.Resources.Cases, DataExtractorRegion.FillDailyCases);
        }

        public void FillChartWitTotalCases(int region, string regionName)
        {
            FillChartWithLinearSeries(region, regionName, Properties.Resources.DailyCases, Properties.Resources.Cases, DataExtractorRegion.FillTotalyCases);
        }

        public void FillChartWithWeeklyCases(int region, string regionName)
        {
            FillChartWithColumnSeries(region, regionName, Properties.Resources.WeeklyCases, Properties.Resources.Cases, DataExtractorRegion.FillWeeklyCases);
        }

        public void FillChartWithWeeklySwab(int region, string regionName)
        {
            FillChartWithColumnSeries(region, regionName, Properties.Resources.WeeklySwabs, Properties.Resources.Swabs, DataExtractorRegion.FillWeeklySwab);
        }

        public void FillChartWithWeeklyDead(int region, string regionName)
        {
            FillChartWithColumnSeries(region, regionName, Properties.Resources.WeeklySwabs, Properties.Resources.Swabs, DataExtractorRegion.FillWeeklyDeads);
        }

        public void FillChartWithWeeklySwabCases(int region, string regionName)
        {
            var swab = DataExtractorRegion.FillWeeklySwabCases(region);
            var cases = DataExtractorRegion.FillWeeklyCases(region);

            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = $"{Properties.Resources.WeeklyCases} {regionName}",
                    Values = new ChartValues<float>(cases.Select(s => s.value)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0"),
                    ScalesYAt = 0
                },
                new LineSeries
                {
                    Title = Properties.Resources.RelationshipCasesSwabs,
                    Values = new ChartValues<float>(swab.Select(s => s.value)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = true,
                    ScalesYAt = 1
                }
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = swab.Select(s => s.lbl).ToList(),
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = Properties.Resources.NewCases,
                LabelFormatter = value => value.ToString("N0")
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = Properties.Resources.RelationshipCasesSwabs,
                LabelFormatter = value => value.ToString("P"),
                Position = AxisPosition.RightTop
            });

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }

        public void FillChartWithDailySwabCases(int region, string regionName)
        {
            var swab = DataExtractorRegion.FillDailySwabCases(region);
            var cases = DataExtractorRegion.FillDailyCases(region);

            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"{Properties.Resources.DailyCases} {regionName}",
                    Values = new ChartValues<float>(cases.Select(s => s.value)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = true,
                    //LabelPoint = point => point.Y.ToString("N0"),
                    ScalesYAt = 0
                },
                new LineSeries
                {
                    Title = Properties.Resources.RelationshipCasesSwabs,
                    Values = new ChartValues<float>(swab.Select(s => s.value)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = false,
                    ScalesYAt = 1
                }
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = swab.Select(s => s.lbl).ToList(),
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = 7,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = Properties.Resources.NewCases,
                LabelFormatter = value => value.ToString("N0")
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = Properties.Resources.RelationshipCasesSwabs,
                LabelFormatter = value => value.ToString("P"),
                Position = AxisPosition.RightTop
            });

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }

        private void FillChartWithLinearSeries(int region, string regionName, string titleSeries, string titleY, Func<int, List<ReturnData>> func)
        {
            var data = func.Invoke(region);
            var linearSeries = new LineSeries
            {
                Title = $"{titleSeries} {regionName}",
                Values = new ChartValues<float>(data.Select(s => s.value)),
                PointGeometry = DefaultGeometries.None,
                DataLabels = false,
                LabelPoint = point => point.Y.ToString("N0")
            };

            var labels = data.Select(s => s.lbl).ToList();
            FillChart(titleSeries, titleY, linearSeries, labels, 7);
        }

        private void FillChartWithColumnSeries(int region, string regionName, string titleSeries, string titleY, Func<int, List<ReturnData>> func)
        {
            var data = func.Invoke(region);
            var columnSeries = new ColumnSeries
            {
                Title = $"{titleSeries} {regionName}",
                Values = new ChartValues<float>(data.Select(s => s.value)),
                PointGeometry = DefaultGeometries.None,
                DataLabels = true,
                LabelPoint = point => point.Y.ToString("N0")
            };

            var labels = data.Select(s => s.lbl).ToList();
            FillChart(titleSeries, titleY, columnSeries, labels, 1);
        }

        private void FillChart(string titleSeries, string titleY, ISeriesView series, IList<string> label, int step)
        {
            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
               series
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = label,
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = step,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            this.chart.AxisY.Add(new Axis
            {
                Title = titleY,
                LabelFormatter = value => value.ToString("N0"),
                MinValue = 0
            });

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }
    }
}
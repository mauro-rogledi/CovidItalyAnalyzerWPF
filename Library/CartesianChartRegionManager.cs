using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.Library
{
    public class CartesianChartRegionManager : IChartManager
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

        private readonly Dictionary<string, Action<int, string>> ChartAvailable = new Dictionary<string, Action<int, string>>();

        public event EventHandler<string> UpdateTitle;

        public CartesianChartRegionManager(Chart chart)
        {
            this.chart = chart as CartesianChart;
            myCI = CultureInfo.CurrentCulture;
            myCal = myCI.Calendar;
            myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            ChartAvailable.Add(Properties.Resources.CurrentCases, (int r, string s) => FillChartWithCurrentCases(r, s));
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
            ChartAvailable.Add(Properties.Resources.WeeklyCasesInhabitant, (int r, string s) => FillChartWithWeeklyCasesInabitant(r, s));
        }

        Func<double, string> prova = val =>
        {
            System.Diagnostics.Debug.WriteLine(val);
            return val.ToString();
        };

        private void FillChartWithCurrentCases(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var intesiveCare = DataExtractorRegion.FillIntensiveCare(region, dateFrom, dateTo);
            var withSyntoms = DataExtractorRegion.FillWithSymptoms(region, dateFrom, dateTo);
            var dailyCases = DataExtractorRegion.FillDailyCases(region, dateFrom, dateTo);

            chart.LegendLocation = LegendLocation.Top;
            chart.DataContext = null;

            chart.Series = new SeriesCollection
            {
                new StackedAreaSeries
                {
                    Title = Properties.Resources.DailyCases,
                    Values = new ChartValues<DateTimePoint>(dailyCases.Select(s => new DateTimePoint(s.data, s.value))),
                },
                new StackedAreaSeries
                {
                    Title = Properties.Resources.HospitalizedWithSymptoms,
                    Values = new ChartValues<DateTimePoint>(withSyntoms.Select(s => new DateTimePoint(s.data, s.value))),
                    Fill = new SolidColorBrush(Colors.Yellow)

                },
                new StackedAreaSeries
                {
                    Title = Properties.Resources.IntensiveCare,
                    Values = new ChartValues<DateTimePoint>(intesiveCare.Select(s => new DateTimePoint(s.data, s.value))),
                    Fill = new SolidColorBrush(Colors.Red)
                }
            };

            chart.AxisX.Clear();
            this.chart.AxisX.Add(new Axis
            {
                LabelFormatter = val => new DateTime((long)val).ToString("dd-MM-yyyy")
            });

            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis
            {
                LabelFormatter = value => value.ToString("N0")
            });
        }

        public string[] GetChartAvailable()
        {
            return ChartAvailable
                .Aggregate(new List<string>(), (acc, x) => { acc.Add(x.Key); return acc; })
                .ToArray();
        }

        public void SetChart(string chart, int region, int county, string display)
        {
            ChartAvailable[chart].Invoke(region, display);
        }

        private void FillChartWitIntensiveCare(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithLinearSeries(region, regionName, dateFrom, dateTo, Properties.Resources.IntensiveCare, Properties.Resources.IntensiveCare, DataExtractorRegion.FillIntensiveCare);
        }

        private void FillChartWitHospital(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithLinearSeries(region, regionName, dateFrom, dateTo, Properties.Resources.Hospital, Properties.Resources.Hospital, DataExtractorRegion.FillHospital);
        }

        private void FillChartWitDailyDeads(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithLinearSeries(region, regionName, dateFrom, dateTo, Properties.Resources.DailyDeads, Properties.Resources.Deads, DataExtractorRegion.FillDailyDeads);
        }

        private void FillChartWitDailySwabs(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithLinearSeries(region, regionName, dateFrom, dateTo, Properties.Resources.DailySwabs, Properties.Resources.Swabs, DataExtractorRegion.FillDailySwabs);
        }

        public void FillChartWitNewPositives(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithLinearSeries(region, regionName, dateFrom, dateTo, Properties.Resources.DailyCases, Properties.Resources.Cases, DataExtractorRegion.FillDailyCases);
        }

        public void FillChartWitTotalCases(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithLinearSeries(region, regionName, dateFrom, dateTo, Properties.Resources.DailyCases, Properties.Resources.Cases, DataExtractorRegion.FillTotalyCases);
        }

        public void FillChartWithWeeklyCases(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithColumnSeries(region, regionName, dateFrom, dateTo, Properties.Resources.WeeklyCases, Properties.Resources.Cases, DataExtractorRegion.FillWeeklyCases);
        }

        public void FillChartWithWeeklySwab(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithColumnSeries(region, regionName, dateFrom, dateTo, Properties.Resources.WeeklySwabs, Properties.Resources.Swabs, DataExtractorRegion.FillWeeklySwab);
        }

        public void FillChartWithWeeklyDead(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            FillChartWithColumnSeries(region, regionName, dateFrom, dateTo, Properties.Resources.WeeklySwabs, Properties.Resources.Swabs, DataExtractorRegion.FillWeeklyDeads);
        }

        public void FillChartWithWeeklyCasesInabitant(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var cases = DataExtractorRegion.FillWeeklyCases(region, dateFrom, dateTo);
            var poples = 100000 / DataReaderPeople.ReadPeopleByRegion(region);

            this.chart.Series.Clear();
            this.chart.AxisX.Clear();
            this.chart.AxisY.Clear();

            this.chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = $"{Properties.Resources.WeeklyCasesInhabitant} {regionName}",
                    Values = new ChartValues<float>(cases.Select(s => s.value * poples)),
                    PointGeometry = DefaultGeometries.None,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0"),
                    ScalesYAt = 0
                }
            };

            this.chart.AxisX.Add(new Axis
            {
                Labels = cases.Select(s => s.lbl).ToList(),
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

            this.chart.LegendLocation = LegendLocation.Top;
            this.chart.Zoom = ZoomingOptions.X;
        }

        public void FillChartWithWeeklySwabCases(int region, string regionName)
        {
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var swab = DataExtractorRegion.FillWeeklySwabCases(region, dateFrom, dateTo);
            var cases = DataExtractorRegion.FillWeeklyCases(region, dateFrom, dateTo);

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
            var dateFrom = FromDate?.Invoke() ?? DateTime.Today;
            var dateTo = ToDate?.Invoke() ?? DateTime.Today;

            var swab = DataExtractorRegion.FillDailySwabCases(region, dateFrom, dateTo);
            var cases = DataExtractorRegion.FillDailyCases(region, dateFrom, dateTo);

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

        private void FillChartWithLinearSeries(int region, string regionName, DateTime dateFrom, DateTime dateTo, string titleSeries, string titleY, Func<int, DateTime, DateTime, List<ReturnData>> func)
        {
            var data = func.Invoke(region, dateFrom, dateTo);
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

        private void FillChartWithColumnSeries(int region, string regionName, DateTime dateFrom, DateTime dateTo, string titleSeries, string titleY, Func<int, DateTime, DateTime, List<ReturnData>> func)
        {
            var data = func.Invoke(region, dateFrom, dateTo);
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
            CartesianChartRegionManager cartesianChartRegionManager = this;
            cartesianChartRegionManager.chart.Series.Clear();
            cartesianChartRegionManager.chart.AxisX.Clear();
            cartesianChartRegionManager.chart.AxisY.Clear();

            cartesianChartRegionManager.chart.Series = new SeriesCollection
            {
               series
            };

            cartesianChartRegionManager.chart.AxisX.Add(new Axis
            {
                Labels = label,
                LabelsRotation = 15,
                Separator = new Separator
                {
                    Step = step,
                    IsEnabled = true //disable it to make it invisible.
                }
            });

            cartesianChartRegionManager.chart.AxisY.Add(new Axis
            {
                Title = titleY,
                LabelFormatter = value => value.ToString("N0"),
                MinValue = 0
            });

            cartesianChartRegionManager.chart.LegendLocation = LegendLocation.Top;
            cartesianChartRegionManager.chart.Zoom = ZoomingOptions.X;
        }
    }
}
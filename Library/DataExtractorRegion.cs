using WPFCovidItalyAnalizer.Model;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WPFCovidItalyAnalizer.Library
{
    public class ReturnData
    {
        public DateTime data { get; set; }
        public string lbl { get; set; }
        public float value { get; set; }
    }

    public class DataExtractorRegion
    {
        private static CultureInfo myCI = CultureInfo.CurrentCulture;
        private static Calendar myCal = myCI.Calendar;
        private static CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
        private static DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

        public static List<ReturnData> FillIntensiveCare(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, f => f.terapia_intensiva);
        }

        public static List<ReturnData> FillHospital(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, f => f.totale_ospedalizzati);
        }

        public static List<ReturnData> FillWithSymptoms(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, f => f.ricoverati_con_sintomi);
        }

        public static List<ReturnData> FillDailyDeads(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, f => f.nuovi_deceduti);
        }

        public static List<ReturnData> FillDailySwabs(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, f => f.nuovi_tamponi_test_molecolare);
        }

        public static List<ReturnData> FillDailyCases(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, p => p.nuovi_positivi);
        }

        public static List<ReturnData> FillTotalCases(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, p => p.totale_positivi);
        }

        public static List<ReturnData> FillCasesToHome(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillRegionWithFunction(region, dateFrom, dateTo, p => p.isolamento_domiciliare);
        }

        internal static List<ReturnData> FillTotalRegionCasesAtDate(DateTime date)
        {
            return DataReaderRegion
                .ReadRegionsAtDate(date)
                .Select((curr) => new ReturnData()
                {
                    data = curr.data,
                    value = curr.totale_positivi,
                    lbl = curr.denominazione_regione
                })
                .OrderByDescending(v => v.value)
                .ToList();
        }

        internal static List<ReturnData> FillRangeData(DateTime dateFrom, DateTime dateTo,  Func<RegionData, float> func)
        {
            return dateFrom.Date == dateTo.Date
                ? DataReaderRegion
                    .ReadRegionsAtDate(dateFrom)
                    .Select((curr) => new ReturnData()
                    {
                        data = curr.data,
                        value = func.Invoke(curr),
                        lbl = curr.denominazione_regione
                    })
                    .OrderByDescending(v => v.value)
                    .ToList()
                : DataReaderRegion
                    .ReadRegionsAtRangeDate(dateFrom, dateTo)
                    .GroupBy(g => g.codice_regione)
                    .Select((curr) => new ReturnData()
                    {
                        data = curr.Max(r => r.data),
                        value = curr.Sum(r => func.Invoke(r)),
                        lbl = curr.Max(r => r.denominazione_regione)
                    })
                    .OrderByDescending(v => v.value)
                    .ToList();
        }

        internal static Dictionary<int, List<ReturnData>> FillRangeDataDiff(DateTime dateFrom, DateTime dateTo, Func<RegionData, float> func)
        {
            var list = dateFrom.Date == dateTo.Date
                ? DataReaderRegion
                    .ReadRegionsAtDate(dateFrom)
                    .ToList()
                : DataReaderRegion
                    .ReadRegionsAtRangeDate(dateFrom, dateTo)
                    .ToList();

            return null;
            //return list.Aggregate(new Dictionary<List<int>, ReturnData>(), (acc, v) => { acc.Add(v.codice_regione, new ReturnData() { data = v.data, value = func.Invoke(v), lbl = v.data.ToString("dd/MM/yy") });  return acc; });
        }


        internal static List<ReturnData> FillRangeDataInhabitants(DateTime dateFrom, DateTime dateTo,  Func<RegionData, float> func)
        {
            return dateFrom.Date == dateTo.Date
                ? DataReaderRegion
                    .ReadRegionsAtDate(dateFrom)
                    .Select((curr) => new ReturnData()
                    {
                        data = curr.data,
                        value = func.Invoke(curr) * 100000 / DataReaderPeople.ReadPeopleByRegion(curr.codice_regione),
                        lbl = curr.denominazione_regione
                    })
                    .OrderByDescending(v => v.value)
                    .ToList()
                : DataReaderRegion
                    .ReadRegionsAtRangeDate(dateFrom, dateTo)
                    .GroupBy(g => g.codice_regione)
                    .Select((curr) => new ReturnData()
                    {
                        data = curr.Max(r => r.data),
                        value = curr.Sum(r => func.Invoke(r) * 100000 / DataReaderPeople.ReadPeopleByRegion(curr.First().codice_regione)),
                        lbl = curr.Max(r => r.denominazione_regione)
                    })
                    .OrderByDescending(v => v.value)
                    .ToList();
        }

        public static List<ReturnData> FillWeeklyCases(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillDailyCases(region, dateFrom, dateTo)
                .GroupBy(g => $"{g.data.StartOfWeek(DayOfWeek.Monday).Year}-{myCal.GetWeekOfYear(g.data, myCWR, myFirstDOW)}")
                .Select((s) => new ReturnData
                {
                    data = s.Max(f => f.data),
                    lbl = $"{s.Min(f => f.data).ToString("dd/MM/yy")} - {s.Max(f => f.data).ToString("dd/MM/yy")}",
                    value = s.Sum(c => c.value)
                }
                )
                .ToList();
        }

        public static List<ReturnData> FillWeeklySwab(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillDailySwabs(region, dateFrom, dateTo)
                .GroupBy(g => $"{g.data.StartOfWeek(DayOfWeek.Monday).Year}-{myCal.GetWeekOfYear(g.data, myCWR, myFirstDOW)}")
                .Select((s) => new ReturnData
                {
                    data = s.Max(f => f.data),
                    lbl = $"{s.Min(f => f.data).ToString("dd/MM/yy")} - {s.Max(f => f.data).ToString("dd/MM/yy")}",
                    value = s.Sum(c => c.value)
                }
                )
                .ToList();
        }

        public static List<ReturnData> FillWeeklyDeads(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillDailyDeads(region, dateFrom, dateTo)
                .GroupBy(g => $"{g.data.StartOfWeek(DayOfWeek.Monday).Year}-{myCal.GetWeekOfYear(g.data, myCWR, myFirstDOW)}")
                .Select((s) => new ReturnData
                {
                    data = s.Max(f => f.data),
                    lbl = $"{s.Min(f => f.data).ToString("dd/MM/yy")} - {s.Max(f => f.data).ToString("dd/MM/yy")}",
                    value = s.Sum(c => c.value)
                }
                )
                .ToList();
        }

        public static List<ReturnData> FillWeeklyAverageHospital(int region, DateTime dateFrom, DateTime dateTo)
        {
            return FillHospital(region, dateFrom, dateTo)
                .GroupBy(g => $"{g.data.StartOfWeek(DayOfWeek.Monday).Year}-{myCal.GetWeekOfYear(g.data, myCWR, myFirstDOW)}")
                .Select((s) => new ReturnData
                {
                    data = s.Max(f => f.data),
                    lbl = $"{s.Min(f => f.data).ToString("dd/MM/yy")} - {s.Max(f => f.data).ToString("dd/MM/yy")}",
                    value = s.Average(c => c.value)
                }
                )
                .ToList();
        }

        //public static List<ReturnData> FillWeeklyHospital(int region)
        //{
        //    return FillDailyHos(region)
        //        .GroupBy(g => myCal.GetWeekOfYear(g.data, myCWR, myFirstDOW))
        //        .Select((s) => new ReturnData
        //        {
        //            data = s.Max(f => f.data),
        //            lbl = s.Max(f => f.data).ToString("dd/MM/yy"),
        //            value = s.Sum(c => c.value)
        //        }
        //        )
        //        .ToList();
        //}

        public static List<ReturnData> FillWeeklySwabCases(int region, DateTime dateFrom, DateTime dateTo)
        {
            var cases = FillWeeklyCases(region, dateFrom, dateTo);
            var swab = FillWeeklySwab(region, dateFrom, dateTo);

            return cases.Zip(swab, (c, s) => new ReturnData()
            {
                lbl = c.lbl,
                data = c.data,
                value = c.value / s.value
            })
            .ToList();
        }

        public static List<ReturnData> FillDailySwabCases(int region, DateTime dateFrom, DateTime dateTo)
        {
            var cases = FillDailyCases(region, dateFrom, dateTo);
            var swab = FillDailySwabs(region, dateFrom, dateTo);

            return cases.Zip(swab, (c, s) => new ReturnData()
            {
                lbl = c.lbl,
                data = c.data,
                value = s.value != 0 && s.value > c.value ? c.value / s.value : 0F
            })
            .ToList();
        }

        internal static List<ReturnData> FillTotalyCases(int region, DateTime dateFrom, DateTime dateTo)
        {
            var list = dateFrom == dateTo
                 ? DataReaderRegion.ReadRegionData(region)
                 : DataReaderRegion.ReadRegionDataAtRangeData(region, dateFrom, dateTo);

            return list
                .Select((s) => new ReturnData
                {
                    data = s.data,
                    lbl = s.data.ToString("dd/MM/yy"),
                    value = s.totale_casi
                }
                )
                .ToList();
        }

        //public static List<ReturnData> FillRegionDifferentsWithFunction(int region, Func<RegionData, float> func)
        //{
        //    List<RegionData> list = DataReaderRegion.ReadRegionData(region)
        //        .ToList();

        //    return list
        //        .Select((curr, i) => new ReturnData()
        //        {
        //            data = curr.data,
        //            value = i > 1 ? (func?.Invoke(curr) ?? 0F) - (func?.Invoke(list[i - 1]) ?? 0F) : func?.Invoke(curr) ?? 0F,
        //            lbl = curr.data.ToString("dd/MM/yy")
        //        }
        //        )
        //        .ToList();
        //}

        public static List<ReturnData> FillRegionWithFunction(int region, DateTime dateFrom, DateTime dateTo, Func<RegionData, float> func)
        {
            return dateFrom == dateTo
            ? DataReaderRegion.ReadRegionData(region)
                .Select((curr, i) => new ReturnData()
                {
                    data = curr.data,
                    value = func?.Invoke(curr) ?? 0F,
                    lbl = curr.data.ToString("dd/MM/yy")
                }
                )
                .ToList()
           : DataReaderRegion.ReadRegionDataAtRangeData(region, dateFrom, dateTo)
                .Select((curr, i) => new ReturnData()
                {
                    data = curr.data,
                    value = func?.Invoke(curr) ?? 0F,
                    lbl = curr.data.ToString("dd/MM/yy")
                }
                )
                .ToList();
        }
    }
}
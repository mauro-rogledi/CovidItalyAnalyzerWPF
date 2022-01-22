using WPFCovidItalyAnalizer.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace WPFCovidItalyAnalizer.Library
{
    public static class DataReaderRegion
    {
        private static List<RegionData> RegionDatas;
        private static List<ItalyRegion> italyRegions;

        private static string folder = string.Empty;

        private static readonly string file = "dpc-covid19-ita-regioni.json";

        internal static async Task RefreshData(bool keepACopy, string folderData)
        {
            await ReadRegionDataFromWeb(keepACopy, file);
        }

        public static bool HasData { get => folder != string.Empty && File.Exists(Path.Combine(folder, file)); }

        internal static async Task ReadData(bool keepACopy, string folderName)
        {
            folder = folderName;
            if (keepACopy && HasData)
                ReadRegionData(folderName);
            else
                await ReadRegionDataFromWeb(keepACopy, file);
        }

        private static async Task ReadRegionDataFromWeb(bool keepACopy, string folderName)
        {
            var data = await GitFilePicker.GetFilesAsync(file);
            DeserializeData(data);
            if (keepACopy)
                File.WriteAllText(Path.Combine(folder, file), data);
        }

        internal static IOrderedEnumerable<RegionData> ReadRegionData(int region)
        {
            return DataReaderRegion.RegionDatas
                 .Where(r => r.codice_regione == region)
                 .OrderBy(d => d.data);
        }

        internal static IOrderedEnumerable<RegionData> ReadRegionDataAtRangeData(int region, DateTime dateFrom, DateTime dateTo)
        {
            return DataReaderRegion.RegionDatas
                 .Where(r => r.codice_regione == region && r.data.Date >= dateFrom.Date && r.data.Date <= dateTo.Date)
                 .OrderBy(d => d.data);
        }

        internal static IEnumerable<RegionData> ReadRegionsAtDate(DateTime date)
        {
            return DataReaderRegion.RegionDatas
                 .Where(r => r.data.Date == date.Date);
        }

        internal static IEnumerable<RegionData> ReadRegionsAtRangeDate(DateTime dateFrom, DateTime dateTo)
        {
            return DataReaderRegion.RegionDatas
                 .Where(r => r.data.Date >= dateFrom.Date && r.data.Date <= dateTo.Date);
        }

        internal static IOrderedEnumerable<ItalyRegion> ReadRegions()
        {
            return italyRegions?
                .OrderBy(o => o.denominazione_regione);
        }

        private static void ReadRegionData(string folderName)
        {
            string fileName = Path.Combine(folderName, file);
            if (!File.Exists(fileName))
                return;
            DeserializeData(File.ReadAllText(fileName));
        }

        private static void DeserializeData(string stringdata)
        {
            var allData = JsonSerializer.Deserialize<List<RegionData>>(stringdata);
            ReadItalyRegions(allData);

            RegionDatas = new List<RegionData>();

            foreach (var region in italyRegions)
            {
                var data = allData
                    .Distinct(new RegionDateCompare())
                    .Where(r => r.codice_regione == region.codice_regione)
                    .OrderBy(o => o.data)
                    .ToList();

                RegionDatas.AddRange(
                    data.Select((curr, i) =>
                    {
                        curr.nuovi_tamponi = Math.Abs(i > 0 ? curr.tamponi - data[i - 1].tamponi : curr.tamponi);
                        curr.nuovi_tamponi_test_molecolare = GetTamponi(data, curr, i);
                        curr.nuovi_tamponi_test_antigenico_rapido = Math.Abs(i > 0 ? curr.tamponi_test_antigenico_rapido_not_null - data[i - 1].tamponi_test_antigenico_rapido_not_null : curr.tamponi_test_antigenico_rapido_not_null);
                        curr.nuovi_terapia_intensiva = i > 0 ? curr.terapia_intensiva - data[i - 1].terapia_intensiva : curr.terapia_intensiva;
                        curr.nuovi_ospedalizzati = i > 0 ? curr.totale_ospedalizzati - data[i - 1].totale_ospedalizzati : curr.totale_ospedalizzati;
                        curr.nuovi_deceduti = i > 0 ? curr.deceduti - data[i - 1].deceduti : curr.deceduti;
                        return curr;
                    })
                );
            }
        }

        private static float GetTamponi(List<RegionData> data, RegionData curr, int i)
        {
            //float Today = curr.tamponi_test_molecolare_not_null > 0 ? curr.tamponi_test_molecolare_not_null : curr.tamponi;
            //float Yesterday = i > 0
            //                    ? data[i - 1].tamponi_test_molecolare_not_null > 0 ? data[i - 1].tamponi_test_molecolare_not_null : data[i - 1].tamponi
            //                    : 0;

            float Today = curr.tamponi;
            float Yesterday = i > 0
                                ? data[i - 1].tamponi
                                : 0;

            return Today - Yesterday;
        }

        private static void ReadItalyRegions(List<RegionData> allData)
        {
            italyRegions = allData
                .OrderBy(d => d.codice_regione)
                .Distinct(new RegionCompare())
                .Aggregate<RegionData, List<ItalyRegion>>(
                    new List<ItalyRegion>(), (reg, d) =>
                    {
                        reg.Add(new ItalyRegion() { codice_regione = d.codice_regione, denominazione_regione = d.denominazione_regione });
                        return reg;
                    });
        }
    }

    public class RegionCompare : IEqualityComparer<RegionData>
    {
        public bool Equals(RegionData x, RegionData y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.denominazione_regione == y.denominazione_regione && x.codice_regione == y.codice_regione;
        }

        public int GetHashCode(RegionData obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            //Get hash code for the Name field if it is not null.
            return obj.denominazione_regione.GetHashCode() ^ obj.codice_regione.GetHashCode();
        }
    }

    public class RegionDateCompare : IEqualityComparer<RegionData>
    {
        public bool Equals(RegionData x, RegionData y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.denominazione_regione == y.denominazione_regione && x.codice_regione == y.codice_regione && x.data == y.data;
        }

        public int GetHashCode(RegionData obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            //Get hash code for the Name field if it is not null.
            return obj.denominazione_regione.GetHashCode() ^ obj.codice_regione.GetHashCode() ^ obj.data.GetHashCode();
        }
    }
}
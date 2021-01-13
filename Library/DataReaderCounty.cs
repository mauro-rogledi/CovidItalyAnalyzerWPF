using WPFCovidItalyAnalizer.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WPFCovidItalyAnalizer.Library
{
    public static class DataReaderCounty
    {
        private static List<CountyData> CountyDatas;
        private static List<ItalyCounty> italyCounties;
        private static string folder;

        private static readonly string file = "dpc-covid19-ita-province.json";
        public static bool HasData { get => folder != string.Empty && File.Exists(Path.Combine(folder, file)); }

        internal static async Task ReadData(bool keepACopy, string folderName)
        {
            folder = folderName;
            if (keepACopy && HasData)
                ReadCountyData(folder);
            else
                await ReadCountyDataFromWeb(keepACopy, file);
        }

        internal static  async Task RefreshData(bool keepACopy, string folderData)
        {
            await ReadCountyDataFromWeb(keepACopy, file);
        }

        private static async Task ReadCountyDataFromWeb(bool keepACopy, string folderName)
        {
            var data = await GitFilePicker.GetFilesAsync(file);
            DeserializeData(data);
            if (keepACopy)
                File.WriteAllText(Path.Combine(folder, file), data);
        }

        private static void DeserializeData(string stringdata)
        {
            var allData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CountyData>>(stringdata);
            ReadItalyCounties(allData);

            CountyDatas = new List<CountyData>();

            foreach (var county in italyCounties)
            {
                var data = allData
                    .Distinct(new CountyDateCompare())
                    .Where(r => r.codice_regione == county.codice_regione && r.codice_provincia == county.codice_provincia)
                    .OrderBy(o => o.data)
                    .ToList();

                CountyDatas.AddRange(
                    data.Select((curr, i) =>
                    {
                        curr.nuovi_positivi = i > 0 ? curr.totale_casi - data[i - 1].totale_casi : curr.totale_casi;
                        return curr;
                    })
                );
            }
        }

        private static void ReadCountyData(string folderName)
        {
            string fileName = Path.Combine(folderName, file);
            if (!File.Exists(fileName))
                return;
            DeserializeData(File.ReadAllText(fileName));
        }

        internal static IOrderedEnumerable<CountyData> ReadCountyData(int region, int county)
        {
            return DataReaderCounty.CountyDatas
             .Where(r => r.codice_regione == region && r.codice_provincia == county)
             .OrderBy(d => d.data);
        }

        internal static IOrderedEnumerable<ItalyCounty> ReadCounties(int region)
        {
            return italyCounties
                .Where(r => r.codice_regione == region && r.sigla_provincia != null)
                .OrderBy(o => o.denominazione_provincia);

            //.Select(r => new  ComboData() { value = r.codice_regione, display =r.denominazione_regione }).ToArray();
        }

        internal static IOrderedEnumerable<ItalyCounty> ReadAllCounties()
        {
            return italyCounties
                .Where(r => r.sigla_provincia != null)
                .OrderBy(o => o.denominazione_provincia);

            //.Select(r => new  ComboData() { value = r.codice_regione, display =r.denominazione_regione }).ToArray();
        }

        private static void ReadItalyCounties(List<CountyData> allData)
        {
            italyCounties = allData?
                .OrderBy(d => d.codice_provincia)
                .Distinct(new CountyCompare())
                .Aggregate<CountyData, List<ItalyCounty>>(
                    new List<ItalyCounty>(), (reg, d) =>
                    {
                        reg.Add(new ItalyCounty()
                        {
                            codice_regione = d.codice_regione,
                            denominazione_regione = d.denominazione_regione,
                            codice_provincia = d.codice_provincia,
                            denominazione_provincia = d.denominazione_provincia,
                            sigla_provincia = d.sigla_provincia
                        });
                        return reg;
                    });
        }
    }

    internal class CountyDateCompare : IEqualityComparer<CountyData>
    {
        public bool Equals(CountyData x, CountyData y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.codice_provincia == y.codice_provincia && x.codice_regione == y.codice_regione && x.data == y.data;
        }

        public int GetHashCode(CountyData obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            //Get hash code for the Name field if it is not null.
            return obj.codice_regione.GetHashCode() ^ obj.codice_provincia.GetHashCode() ^ obj.data.GetHashCode();
        }
    }
}

public class CountyCompare : IEqualityComparer<CountyData>
{
    public bool Equals(CountyData x, CountyData y)
    {
        //Check whether the compared objects reference the same data.
        if (Object.ReferenceEquals(x, y)) return true;

        //Check whether any of the compared objects is null.
        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            return false;

        //Check whether the products' properties are equal.
        return x.denominazione_provincia == y.denominazione_provincia && x.codice_provincia == y.codice_provincia;
    }

    public int GetHashCode(CountyData obj)
    {
        //Check whether the object is null
        if (Object.ReferenceEquals(obj, null)) return 0;

        //Get hash code for the Name field if it is not null.
        return obj.denominazione_provincia.GetHashCode() ^ obj.codice_provincia.GetHashCode();
    }
}
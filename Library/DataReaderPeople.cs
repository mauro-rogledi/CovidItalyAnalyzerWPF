using WPFCovidItalyAnalizer.Model;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCovidItalyAnalizer.Library
{
    public static class DataReaderPeople
    {
        private static List<PeopleRegionData> PeopleRegion;
        private static List<PeopleCountyData> PeopleCounty;

        public static float ReadPeopleByRegion(int region)
        {
            if (PeopleRegion == null)
                PeopleRegion = ReadRegionDataFromJson();

            return PeopleRegion
                .Find(p => p.codice_regione == region)
                .popolazione;
        }

        public static float ReadPeopleCounty(int region, int county)
        {
            if (PeopleCounty == null)
                PeopleCounty = ReadCountyDataFromJson();

            return PeopleCounty
                .Find(p => p.codice_regione == region && p.codice_provincia == county)
                .popolazione;
        }

        private static List<PeopleRegionData> ReadRegionDataFromJson()
        {
            var asciiString = Encoding.ASCII.GetString(Properties.Resources.PopolazioneRegioni, 0, Properties.Resources.PopolazioneRegioni.Length);
            return JsonConvert.DeserializeObject<List<PeopleRegionData>>(asciiString);
        }

        private static List<PeopleCountyData> ReadCountyDataFromJson()
        {
            var asciiString = Encoding.ASCII.GetString(Properties.Resources.PopolazioneProvince, 0, Properties.Resources.PopolazioneProvince.Length);
            return JsonConvert.DeserializeObject<List<PeopleCountyData>>(asciiString);
        }
    }
}

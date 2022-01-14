using System;
using System.Text.Json.Serialization;

namespace WPFCovidItalyAnalizer.Model
{
    public class CountyData
    {
        public DateTime data { get; set; }
        public string stato { get; set; }
        public int codice_regione { get; set; }
        public string denominazione_regione { get; set; }
        public int codice_provincia { get; set; }
        public string denominazione_provincia { get; set; }
        public string sigla_provincia { get; set; }
        public float totale_casi { get; set; }

        [JsonIgnore]
        public float nuovi_positivi { get; set; }
    }
}
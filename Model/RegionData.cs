using Newtonsoft.Json;

using System;

namespace WPFCovidItalyAnalizer.Model
{
    public class RegionData
    {
        public DateTime data { get; set; }
        public string stato { get; set; }
        public int codice_regione { get; set; }
        public string denominazione_regione { get; set; }
        public float ricoverati_con_sintomi { get; set; }
        public float terapia_intensiva { get; set; }
        public float totale_ospedalizzati { get; set; }
        public float isolamento_domiciliare { get; set; }
        public float totale_positivi { get; set; }
        public float variazione_totale_positivi { get; set; }
        public float nuovi_positivi { get; set; }
        public float dimessi_guariti { get; set; }
        public float deceduti { get; set; }
        public float? casi_da_sospetto_diagnostico { get; set; }
        public float? casi_da_screening { get; set; }
        public float totale_casi { get; set; }
        public float tamponi { get; set; }
        public float? casi_testati { get; set; }

        [JsonIgnore]
        public float nuovi_tamponi { get; set; }

        [JsonIgnore]
        public float nuovi_deceduti { get; set; }

        [JsonIgnore]
        public float nuovi_ospedalizzati { get; set; }

        [JsonIgnore]
        public float nuovi_terapia_intensiva { get; set; }
    }
}
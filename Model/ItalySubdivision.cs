namespace WPFCovidItalyAnalizer.Model
{
    public class ItalyRegion
    {
        public int codice_regione { get; set; }
        public string denominazione_regione { get; set; }
    }

    public class ItalyCounty
    {
        public int codice_regione { get; set; }
        public string denominazione_regione { get; set; }
        public int codice_provincia { get; set; }
        public string denominazione_provincia { get; set; }
        public string sigla_provincia { get; set; }
    }
}
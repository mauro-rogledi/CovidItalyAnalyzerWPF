using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.ViewModel
{
    public class CartesianRegionVM : BaseVM, INotifyPropertyChanged
    {
        public ObservableCollection<ComboRegionData> RegionDatas { get; set; }
        public ObservableCollection<string> ChartDatas { get; set; }

        private ComboRegionData regionSelected;

        public ComboRegionData RegionSelected
        {
            get { return regionSelected; }
            set { regionSelected = value; }
        }

        private string chartSelected;

        public string ChartSelected
        {
            get { return chartSelected; }
            set { chartSelected = value; }
        }


        public CartesianRegionVM()
        {
            RegionDatas = new ObservableCollection<ComboRegionData>();
            ChartDatas = new ObservableCollection<string>();

            var chartManager = new CartesianChartRegionManager(null)
            {
                Region = () => regionSelected
            };

            chartManager.GetChartAvailable().ToList().ForEach(e => { ChartDatas.Add(e); });
        }

        internal void Refresh()
        { 
            DataReaderRegion.ReadRegions()
            .Select(r => new ComboRegionData() { value = r.codice_regione, display = r.denominazione_regione })
            .ToList()
            .ForEach((e) =>
            {
            RegionDatas.Add(e);
        });
    }
}
}
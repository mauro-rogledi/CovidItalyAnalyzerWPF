using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFCovidItalyAnalizer.ViewModel
{
    public class MenuVM : INotifyPropertyChanged
    {
        readonly Brush background = new SolidColorBrush(Color.FromArgb(255, 43, 87, 154));
        readonly Brush selected = new SolidColorBrush(Color.FromArgb(255, 0, 32, 80));
        public event PropertyChangedEventHandler PropertyChanged;

        private Visibility regionVisible = Visibility.Collapsed;

        public Visibility RegionVisible
        {
            get { return regionVisible; }
            set { SetValue<Visibility>(ref regionVisible, value); }
        }

        private bool regionSelected;

        public bool RegionSelected
        {
            get { return regionSelected; }
            set { 
                SetValue<bool>(ref regionSelected, value); 
                if (value)
                {
                    CountySelected = false;
                    ItalySelected = false;
                    RegionVisible = Visibility.Visible;
                }
            }
        }

        private bool countySelected;

        public bool CountySelected
        {
            get { return countySelected; }
            set { 
                SetValue<bool>(ref countySelected, value);
                if (value)
                {
                    RegionSelected = false;
                    ItalySelected = false;
                    RegionVisible = Visibility.Collapsed;
                }
            }
        }

        private bool italySelected;

        public bool ItalySelected
        {
            get { return italySelected; }
            set { 
                SetValue<bool>(ref italySelected, value); 
                if (value)
                {
                    RegionSelected = false;
                    CountySelected = false;
                }
            }
        }

        internal void Select(string name)
        {
            switch(name)
            {
                case "Region":
                    RegionVisible = Visibility.Visible;
                    break;
                case "County":
                    RegionVisible = Visibility.Collapsed;
                    break;
                case "Italy":
                    RegionVisible = Visibility.Collapsed;
                    break;
            }
        }

        private void SetValue<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {

            if (object.Equals(field, newValue) == false)
            {
                field = newValue;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string v = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public MenuVM()
        {
        }
    }
}

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

        private Brush regionBackColor;

        public Brush RegionBackColor
        {
            get { return regionBackColor; }
            set { SetValue<Brush>(ref regionBackColor, value); }
        }

        private Brush countyBackColor;

        public Brush CountyBackColor
        {
            get { return countyBackColor; }
            set { SetValue<Brush>(ref countyBackColor, value); }
        }

        private Brush italyBackColor;

        public Brush ItalyBackColor
        {
            get { return italyBackColor; }
            set { SetValue<Brush>(ref italyBackColor, value); }
        }

        internal void Select(string name)
        {
            switch(name)
            {
                case "Region":
                    RegionBackColor = selected;
                    CountyBackColor = background;
                    ItalyBackColor = background;
                    RegionVisible = Visibility.Visible;
                    break;
                case "County":
                    CountyBackColor = selected;
                    RegionBackColor = background;
                    ItalyBackColor = background;
                    RegionVisible = Visibility.Collapsed;
                    break;
                case "Italy":
                    ItalyBackColor = selected;
                    CountyBackColor = background;
                    RegionBackColor = background;
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
            regionBackColor = background;
            countyBackColor = background;
        }
    }
}

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
    public class MenuVM : BaseVM, INotifyPropertyChanged
    {
        readonly Brush background = new SolidColorBrush(Color.FromArgb(255, 43, 87, 154));
        readonly Brush selected = new SolidColorBrush(Color.FromArgb(255, 0, 32, 80));

        private bool countySelected;
        private bool italySelected;
        private bool regionSelected;
        private Visibility italyVisible = Visibility.Collapsed;
        private Visibility regionVisible = Visibility.Collapsed;
        private Visibility countyVisible = Visibility.Collapsed;

        public MenuVM()
        {
        }

        public bool CountySelected
        {
            get { return countySelected; }
            set
            {
                SetValue<bool>(ref countySelected, value);
                if (value)
                {
                    RegionSelected = false;
                    ItalySelected = false;
                    CountyVisible = Visibility.Visible;
                    RegionVisible = Visibility.Collapsed;
                    ItalyVisible = Visibility.Collapsed;
                }
            }
        }

        public bool ItalySelected
        {
            get { return italySelected; }
            set
            {
                SetValue<bool>(ref italySelected, value);
                if (value)
                {
                    RegionSelected = false;
                    CountySelected = false;
                    ItalyVisible = Visibility.Visible;
                    RegionVisible = Visibility.Collapsed;
                    CountyVisible = Visibility.Collapsed;
                }
            }
        }

        public Visibility ItalyVisible
        {
            get { return italyVisible; }
            set { SetValue<Visibility>(ref italyVisible, value); }
        }

        public bool RegionSelected
        {
            get { return regionSelected; }
            set
            {
                SetValue<bool>(ref regionSelected, value);
                if (value)
                {
                    CountySelected = false;
                    ItalySelected = false;
                    RegionVisible = Visibility.Visible;
                    ItalyVisible = Visibility.Collapsed;
                    CountyVisible = Visibility.Collapsed;
                }
            }
        }

        public Visibility RegionVisible
        {
            get { return regionVisible; }
            set { SetValue<Visibility>(ref regionVisible, value); }
        }

        public Visibility CountyVisible
        {
            get { return countyVisible; }
            set { SetValue<Visibility>(ref countyVisible, value); }
        }
    }
}

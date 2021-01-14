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

        private Visibility regionVisible = Visibility.Collapsed;

        public Visibility RegionVisible
        {
            get { return regionVisible; }
            set { SetValue<Visibility>(ref regionVisible, value); }
        }

        private Visibility italyVisible = Visibility.Collapsed;

        public Visibility ItalyVisible
        {
            get { return italyVisible; }
            set { SetValue<Visibility>(ref italyVisible, value);  }
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
                    ItalyVisible = Visibility.Collapsed;
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
                    ItalyVisible = Visibility.Collapsed;
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
                    ItalyVisible = Visibility.Visible;
                    RegionVisible = Visibility.Collapsed;
                }
            }
        }

        public MenuVM()
        {
        }
    }
}

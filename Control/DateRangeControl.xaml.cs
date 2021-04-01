using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFCovidItalyAnalizer.Control
{
    /// <summary>
    /// Interaction logic for DateRangeControl.xaml
    /// </summary>
    public partial class DateRangeControl : UserControl
    {
        public DateRangeControl()
        {
            InitializeComponent();
        }

        public DateTime DateFrom
        {
            get { return (DateTime)GetValue(DateFromProperty); }
            set { SetValue(DateFromProperty, value); }
        }

        public static readonly DependencyProperty DateFromProperty =
            DependencyProperty.Register("DateFrom", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Today, (o, a) => (o as DateRangeControl).OnDateFromChanged()));

        private void OnDateFromChanged()
        {
            
        }

        private void OnDateToChanged()
        {

        }


        public DateTime DateTo
        {
            get { return (DateTime)GetValue(DateToProperty); }
            set { SetValue(DateToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateToProperty =
            DependencyProperty.Register("DateTo", typeof(DateTime), typeof(DateRangeControl), new PropertyMetadata(DateTime.Today, (o, a) => (o as DateRangeControl).OnDateToChanged()));


    }
}

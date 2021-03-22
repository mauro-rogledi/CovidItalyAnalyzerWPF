using CovidItaWPFCovidItalyAnalizerlyAnalyzer.Library;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFCovidItalyAnalizer.Library;
using WPFCovidItalyAnalizer.View;
using WPFCovidItalyAnalizer.ViewModel;

namespace WPFCovidItalyAnalizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MenuVM menuVM;
        public MainWindow()
        {
            InitializeComponent();

            menuVM = Resources["vmMenu"] as MenuVM;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SettingManager.ReadData();

            await LoadData();

            CartesianRegion.Refresh();
            PieChartItaly.Refresh();
            CartesianCounty.Refresh();
        }

        private async Task LoadData(bool isRefresh = false)
        {
            myGif.Visibility = Visibility.Visible;
            await Task.Run(async () =>
            {
                if (isRefresh)
                    await DataReader.RefreshData();
                else
                    await DataReader.ReadData();
            });

            myGif.Visibility = Visibility.Collapsed;
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            new SettingWindow().ShowDialog();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadData(true);

            CartesianRegion.Refresh();
            PieChartItaly.Refresh();
            CartesianCounty.Refresh();
        }
    }
}

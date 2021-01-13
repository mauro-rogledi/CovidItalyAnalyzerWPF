using CovidItaWPFCovidItalyAnalizerlyAnalyzer.Library;

using System.Windows;

using WPFCovidItalyAnalizer.Library;

namespace WPFCovidItalyAnalizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        private async void App_Startup(object sender, StartupEventArgs e)
        {
            SettingManager.ReadData();
            await DataReader.ReadData();
        }
    }
}

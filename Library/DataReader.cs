using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFCovidItalyAnalizer.Library;

namespace CovidItaWPFCovidItalyAnalizerlyAnalyzer.Library
{
    public static class DataReader
    {
        public static bool HasReadData { get => HasData(); }

        private static bool HasData()
        {
            if (SettingManager.FolderData == "")
                return false;

            return Directory.Exists(Path.Combine(SettingManager.FolderData, "dati-json"));
        }

        public static async Task RefreshData()
        {
            await DataReaderRegion.RefreshData(SettingManager.KeepACopy, SettingManager.FolderData);
            await DataReaderCounty.RefreshData(SettingManager.KeepACopy,SettingManager.FolderData);
        }

        public async static Task<bool> ReadData()
        {
            await DataReaderRegion.ReadData(SettingManager.KeepACopy, SettingManager.FolderData);
            await DataReaderCounty.ReadData(SettingManager.KeepACopy, SettingManager.FolderData);

            return true;
        }
    }
}

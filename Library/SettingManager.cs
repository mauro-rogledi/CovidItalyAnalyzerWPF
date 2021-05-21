using System;
using System.Configuration;

namespace WPFCovidItalyAnalizer.Library
{
    public static class SettingManager
    {
        private const string folderdata = "Folderdata";
        private const string keepacopy = "keepacopy";
        private const string rememberlastdata = "rememberlastdata";
        private const string region = "region";
        private const string county = "county";
        private const string countyregion = "countyregion";

        public static string FolderData { get; set; }

        public static bool KeepACopy { get; set; }

        public static bool RememberLastData { get; set; }

        public static int Region { get; set; }

        public static int County { get; set; }
        public static int CountyRegion { get; set; }

        public static void ReadData()
        {
            FolderData = ReadSetting<string>(folderdata);
            KeepACopy = ReadSetting<bool>(keepacopy);
            RememberLastData = ReadSetting<bool>(rememberlastdata);

            Region = ReadSetting<int>(region);
            County = ReadSetting<int>(county);
            CountyRegion = ReadSetting<int>(countyregion);
        }

        public static void SaveData()
        {
            AddUpdateAppSettings(folderdata, FolderData);
            AddUpdateAppSettings(keepacopy, KeepACopy.ToString());
            AddUpdateAppSettings(rememberlastdata, RememberLastData.ToString());
            AddUpdateAppSettings(region, Region.ToString());
            AddUpdateAppSettings(county, County.ToString());
            AddUpdateAppSettings(countyregion, CountyRegion.ToString());
        }

        private static T ReadSetting<T>(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return (T)Convert.ChangeType(appSettings[key] ?? "", typeof(T));
            }
            catch (ConfigurationErrorsException)
            {
                return default;
            }
            catch(Exception)
            {
                return default;
            }
        }

        private static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
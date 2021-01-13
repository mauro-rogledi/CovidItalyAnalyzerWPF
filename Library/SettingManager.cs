using System;
using System.Configuration;

namespace WPFCovidItalyAnalizer.Library
{
    public static class SettingManager
    {
        private const string folderdata = "Folderdata";
        private const string keepacopy = "keepacopy";

        public static string FolderData { get; set; }

        public static bool KeepACopy { get; set; }

        public static void ReadData()
        {
            FolderData = ReadSetting<string>(folderdata);
            bool keepaCopy = false;
            if (Boolean.TryParse(ReadSetting<bool>(keepacopy), out keepaCopy))
                KeepACopy = keepaCopy;
        }

        public static void SaveData()
        {
            AddUpdateAppSettings(folderdata, FolderData);
            AddUpdateAppSettings(keepacopy, KeepACopy.ToString());
        }

        private static string ReadSetting<T>(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? "";
            }
            catch (ConfigurationErrorsException)
            {
                return "";
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
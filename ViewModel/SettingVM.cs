using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFCovidItalyAnalizer.Library;

namespace WPFCovidItalyAnalizer.ViewModel
{
    public class SettingVM : BaseVM, INotifyPropertyChanged
    {
        private string folderData;
        const string waterMark = "Clicca per inserire";

        public string Folder
        {
            get { return folderData; }
            set { SetValue<string>(ref folderData, value); }
        }

        private bool keepData;

        public bool KeepData
        {
            get { return keepData; }
            set { SetValue<bool>(ref keepData, value); }
        }

        private bool rememberLastData;

        public bool RememberLastData
        {
            get { return rememberLastData; }
            set { SetValue<bool>(ref rememberLastData, value); }
        }


        public SettingVM()
        {
            KeepData = SettingManager.KeepACopy;
            folderData = SettingManager.FolderData;
            rememberLastData = SettingManager.RememberLastData;

            if (folderData==string.Empty)
                folderData = waterMark;
        }

        internal void Save()
        {
            SettingManager.KeepACopy = KeepData;
            SettingManager.FolderData = folderData == waterMark ? string.Empty : folderData;
            SettingManager.RememberLastData = RememberLastData;
            SettingManager.SaveData();
        }
    }
}

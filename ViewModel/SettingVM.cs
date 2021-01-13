using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCovidItalyAnalizer.ViewModel
{
    public class SettingVM : BaseVM, INotifyPropertyChanged
    {
        private string folderData;

        public string Folder
        {
            get { return folderData; }
            set { SetValue<string>(ref folderData, value); }
        }

        private bool keepData;

        public bool KeepData
        {
            get { return keepData; }
            set { keepData = value; }
        }

        public SettingVM()
        {
            KeepData = true;
            folderData = "Clicca per inserire";
        }
    }
}

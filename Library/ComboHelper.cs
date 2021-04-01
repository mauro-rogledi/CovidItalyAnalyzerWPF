using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.Library
{
    public class ComboHelper
    {
        static public void FillComboWithEnum<T>(ObservableCollection<ComboData> comboList, Func<T, bool> displayItem = null)
        {
            foreach (T i in Enum.GetValues(typeof(T)))
            {
                if (displayItem == null || displayItem(i))
                {
                    int val = (int)Enum.Parse(typeof(T), i.ToString());
                    var disp = Enum.GetName(typeof(T), i);
                    disp = Properties.Resources.ResourceManager.GetString(disp)?? disp;
                    comboList.Add(new ComboData() { value = val, display = disp });
                }
            }
        }
    }
}

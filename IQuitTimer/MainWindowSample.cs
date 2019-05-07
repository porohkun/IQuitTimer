using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQuitTimer
{
    public class MainWindowSample
    {
        public ObservableCollection<DayEntry> Days { get; set; } = new ObservableCollection<DayEntry>()
        {
            new DayEntry(DateTime.Now, new TimeSpan(11,0,0),new TimeSpan(20,0,0),new TimeSpan(0,1,0))
        };
    }
}

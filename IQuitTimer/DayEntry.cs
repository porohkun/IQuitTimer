using System;
using System.Collections.ObjectModel;

namespace IQuitTimer
{
    public class DayEntry
    {
        public DateTime Date { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public TimeSpan Step { get; private set; }

        public ObservableCollection<bool> Items { get; set; } = new ObservableCollection<bool>();

        public DayEntry(DateTime date, TimeSpan startTime, TimeSpan endTime, TimeSpan step)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Step = step;

            for (var t = startTime; t < endTime; t += step)
            {
                Items.Add(date + t < DateTime.Now);
            }
        }
    }
}
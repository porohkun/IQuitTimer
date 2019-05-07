using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IQuitTimer
{
    public class DayEntry
    {
        public DateTime Date { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public TimeSpan Step { get; private set; }

        public ObservableCollection<StepItem> Items { get; set; } = new ObservableCollection<StepItem>();

        public DayEntry(DateTime date, TimeSpan startTime, TimeSpan endTime, TimeSpan step)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Step = step;

            for (var t = startTime; t < endTime; t += step)
            {
                Items.Add(new StepItem(date + t < DateTime.Now, date + t < DateTime.Now));
            }
        }

        public void Update()
        {
            var t = StartTime;
            for (int i=0;i< Items.Count;i++)
            {
                Items[i].Passed = Date + t < DateTime.Now;
                t += Step;
            }
        }
    }

    public class StepItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private bool _passed;
        private bool _clicked;

        public bool Passed
        {
            get => _passed;
            set
            {
                if (Passed != value)
                {
                    _passed = value;
                    NotifyPropertyChanged(nameof(Passed));
                    NotifyPropertyChanged(nameof(Summary));
                }
            }
        }
        public bool Clicked
        {
            get => _clicked;
            set
            {
                if (!Clicked && value)
                {
                    _clicked = value;
                    NotifyPropertyChanged(nameof(Clicked));
                    NotifyPropertyChanged(nameof(Summary));
                }
            }
        }
        public bool Summary => Passed && Clicked;

        public StepItem(bool passed, bool clicked)
        {
            _passed = passed;
            _clicked = clicked;
        }
    }
}
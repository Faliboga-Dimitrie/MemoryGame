using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MemoryGame.Models
{
    public class TimeTracker : BaseClass
    {
        int _seconds;
        public int Seconds
        {
            get => _seconds;
            set
            {
                _seconds = value;
                OnPropertyChanged();
            }
        }

        int _minutes;
        public int Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value;
                OnPropertyChanged();
            }
        }

        int _secondLeft;
        public int SecondLeft
        {
            get => _secondLeft;
            set
            {
                _secondLeft = value;
                OnPropertyChanged();
            }
        }

        int _minuteLeft;
        public int MinuteLeft
        {
            get => _minuteLeft;
            set
            {
                _minuteLeft = value;
                OnPropertyChanged();
            }
        }

        private DispatcherTimer _timer;

        public DispatcherTimer Timer
        {
            get => _timer;
            set
            {
                _timer = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _remainingTime;

        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set
            {
                _remainingTime = value;
                OnPropertyChanged();
            }
        }

        public TimeTracker()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _seconds = 0;
            _minutes = 0;
        }
    }
}

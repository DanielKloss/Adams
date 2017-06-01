using System;
using Windows.UI.Xaml;

namespace TheClockEnd.Data.Models
{
    public class Clock : BaseModel
    {
        private DispatcherTimer _timer;

        private DateTime _theTime;
        public DateTime theTime
        {
            get { return _theTime; }
            set
            {
                _theTime = value;
                onPropertyChanged(nameof(theTime));
            }
        }

        public Clock()
        {
            _timer = new DispatcherTimer();
            setTimer();
        }

        public Clock(DispatcherTimer timer)
        {
            _timer = timer;
            setTimer();
        }

        private void setTimer()
        {
            _timer.Interval = new TimeSpan(0, 0, 10);
            _timer.Tick += TimerTick;

            theTime = DateTime.Now;

            _timer.Start();
        }

        private void TimerTick(object sender, object e)
        {
            theTime = DateTime.Now;
        }
    }
}

using System;
using Windows.UI.Xaml;

namespace TheClockEnd.ViewModels
{
    public class ClockViewModel : BaseViewModel
    {
        private DispatcherTimer timer;

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

        public ClockViewModel()
        {
            SetTimer();
        }

        private void SetTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += TimerTick;

            theTime = DateTime.Now;

            timer.Start();
        }

        private void TimerTick(object sender, object e)
        {
            theTime = DateTime.Now;
        }
    }
}

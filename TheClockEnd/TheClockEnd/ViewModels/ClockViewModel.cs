using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace TheClockEnd.ViewModels
{
    public class ClockViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer timer;

        private DateTime _theTime;
        public DateTime theTime
        {
            get { return _theTime; }
            set
            {
                _theTime = value;
                onPropertyChanged("theTime");
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

        #region INPC
        private void onPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

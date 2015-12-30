using TheClockEnd.Models;
using Windows.UI.Xaml;

namespace TheClockEnd.ViewModels
{
    public class ClockViewModel : BaseViewModel
    {
        public Clock clock { get; set; }

        public ClockViewModel()
        {
            clock = new Clock(new DispatcherTimer());
        }

        public ClockViewModel(Clock newClock)
        {
            clock = newClock;
        }
    }
}

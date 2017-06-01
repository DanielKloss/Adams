using TheClockEnd.Data.Models;
using Windows.UI.Xaml;

namespace TheClockEnd.UI.ViewModels
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

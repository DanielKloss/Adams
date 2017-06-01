using Microsoft.Toolkit.Uwp.UI.Extensions;
using TheClockEnd.UI.ViewModels;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TheClockEnd.UI.Views
{
    public sealed partial class ClockView : Page
    {
        public ClockView()
        {
            InitializeComponent();

            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                StatusBar.SetIsVisible(this, false);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = new ClockViewModel();
        }
    }
}

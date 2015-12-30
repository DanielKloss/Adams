using System;
using System.Windows.Input;
using TheClockEnd.Helpers;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.System;

namespace TheClockEnd.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private ICommand _SendEmailCommand;
        public ICommand SendEmailCommand
        {
            get
            {
                if (_SendEmailCommand == null)
                {
                    _SendEmailCommand = new Command(SendEmail, CanCommand);
                }
                return _SendEmailCommand;
            }
            set { _SendEmailCommand = value; }
        }

        private ICommand _RateAndReviewCommand;
        public ICommand RateAndReviewCommand
        {
            get
            {
                if (_RateAndReviewCommand == null)
                {
                    _RateAndReviewCommand = new Command(RateAndReview, CanCommand);
                }
                return _RateAndReviewCommand;
            }
            set { _RateAndReviewCommand = value; }
        }

        private ICommand _DonateCommand;
        public ICommand DonateCommand
        {
            get
            {
                if (_DonateCommand == null)
                {
                    _DonateCommand = new Command(Donate, CanCommand);
                }
                return _DonateCommand;
            }
            set { _DonateCommand = value; }
        }

        public AboutViewModel()
        {
            hasBackButtonHardware = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }

        private bool CanCommand()
        {
            return true;
        }

        private async void SendEmail()
        {
            var mailto = new Uri("mailto:?to=theclockendapp@outlook.com&subject=The Clock End Windows Feedback");
            await Launcher.LaunchUriAsync(mailto);
        }

        private async void RateAndReview()
        {
            string packageFamilyName = Package.Current.Id.FamilyName;
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=" + packageFamilyName));
        }

        private void Donate()
        {

        }
    }
}

using System;
using System.ComponentModel;
using System.Windows.Input;
using TheClockEnd.Helpers;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Xaml;

namespace TheClockEnd.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private bool _hasBackButtonHardware;
        public bool hasBackButtonHardware
        {
            get { return _hasBackButtonHardware; }
            set
            {
                _hasBackButtonHardware = value;
                onPropertyChanged(nameof(hasBackButtonHardware));
            }
        }

        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_BackCommand == null)
                {
                    _BackCommand = new Command(GoBack, CanCommand);
                }
                return _BackCommand;
            }
            set { _BackCommand = value; }
        }

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

        private void GoBack()
        {
            ((App)Application.Current).rootFrame.GoBack();
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

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TheClockEnd.Helpers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace TheClockEnd.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private MessageDialog dialog;

        private bool _donatedBeer;
        public bool donatedBeer
        {
            get { return _donatedBeer; }
            set
            {
                _donatedBeer = value;
                onPropertyChanged(nameof(donatedBeer));
            }
        }

        private bool _donatedPie;
        public bool donatedPie
        {
            get { return _donatedPie; }
            set
            {
                _donatedPie = value;
                onPropertyChanged(nameof(donatedPie));
            }
        }

        private bool _donatedProgramme;
        public bool donatedProgramme
        {
            get { return _donatedProgramme; }
            set
            {
                _donatedProgramme = value;
                onPropertyChanged(nameof(donatedProgramme));
            }
        }

        private bool _working;
        public bool working
        {
            get { return _working; }
            set
            {
                _working = value;
                onPropertyChanged(nameof(working));
            }
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
                    _DonateCommand = new Command<string>(Donate, CanCommand);
                }
                return _DonateCommand;
            }
            set { _DonateCommand = value; }
        }

        public AboutViewModel()
        {
            HasDonatedBeer();
            HasDonatedPie();
            HasDonatedProgramme();
        }

        private bool CanCommand()
        {
            if (working)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CanCommand(string donation)
        {
            if (working)
            {
                return true;
            }
            else
            {
                return true;
            }
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

        private async void Donate(string donation)
        {
            //For Testing Only
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///DesignData/WindowsStoreProxy.xml"));
            await CurrentAppSimulator.ReloadSimulatorAsync(file);


            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            if (connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                try
                {
                    working = true;
                    await CurrentAppSimulator.RequestProductPurchaseAsync(donation);
                }
                catch (ArgumentException)
                {
                    await PurchaseError();
                }
                catch (COMException)
                {
                    await PurchaseError();
                }
                catch (OutOfMemoryException)
                {
                    await PurchaseError();
                }
                finally
                {
                    HasDonatedBeer();
                    HasDonatedPie();
                    HasDonatedProgramme();
                    working = false;
                }
            }
            else
            {
                dialog = new MessageDialog("You dont have an internet connection, please try again", "No Internet");
                await dialog.ShowAsync();
            }
        }

        private async Task PurchaseError()
        {
            dialog = new MessageDialog("Something went wrong with your donation, please try again", "Error");
            await dialog.ShowAsync();
        }

        private void HasDonatedBeer()
        {
            if (((App)Application.Current).licenseInfo.ProductLicenses["PreMatchBeer"].IsActive)
            {
                donatedBeer = true;
            }
            else
            {
                donatedBeer = false;
            }
        }

        private void HasDonatedProgramme()
        {
            if (((App)Application.Current).licenseInfo.ProductLicenses["PreMatchProgramme"].IsActive)
            {
                donatedProgramme = true;
            }
            else
            {
                donatedProgramme = false;
            }
        }

        private void HasDonatedPie()
        {
            if (((App)Application.Current).licenseInfo.ProductLicenses["PreMatchPie"].IsActive)
            {
                donatedPie = true;
            }
            else
            {
                donatedPie = false;
            }
        }
    }
}

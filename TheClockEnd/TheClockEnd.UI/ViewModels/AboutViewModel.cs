using MvvmDialogs;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TheClockEnd.UI.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private IDialogService _dialogService;

        private string _version;
        public string version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged(nameof(version));
            }
        }

        private bool _donatedBeer;
        public bool donatedBeer
        {
            get { return _donatedBeer; }
            set
            {
                _donatedBeer = value;
                OnPropertyChanged(nameof(donatedBeer));
            }
        }

        private bool _donatedPie;
        public bool donatedPie
        {
            get { return _donatedPie; }
            set
            {
                _donatedPie = value;
                OnPropertyChanged(nameof(donatedPie));
            }
        }

        private bool _donatedProgramme;
        public bool donatedProgramme
        {
            get { return _donatedProgramme; }
            set
            {
                _donatedProgramme = value;
                OnPropertyChanged(nameof(donatedProgramme));
            }
        }

        private bool _working;
        public bool working
        {
            get { return _working; }
            set
            {
                _working = value;
                OnPropertyChanged(nameof(working));
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

        public AboutViewModel(IDialogService dialogService)
        {
            version = String.Format("{0}.{1}", Package.Current.Id.Version.Major.ToString(), Package.Current.Id.Version.Minor.ToString());
            _dialogService = dialogService;

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
            //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///DesignData/WindowsStoreProxy.xml"));
            //await CurrentAppSimulator.ReloadSimulatorAsync(file);


            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            if (connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                try
                {
                    working = true;
                    await CurrentApp.RequestProductPurchaseAsync(donation);
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
                await ShowDialog("Connection Error", "The Windows Store couldn't be reached, please check your internet connection and try again.");
            }
        }

        private async Task PurchaseError()
        {
            await ShowDialog("Something went wrong with your donation, please check your internet connection and try again", "Error");
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

        private async Task ShowDialog(string title, string text)
        {
            var dialogViewModel = new ConfirmContentDialogViewModel(title, text);

            ContentDialogResult result = await _dialogService.ShowContentDialogAsync(dialogViewModel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using TheClockEnd.Data;
using TheClockEnd.Helpers;
using TheClockEnd.Models;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Xaml;

namespace TheClockEnd.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        private ICustomDataReader reader;
        private List<Stat> stats;

        private bool _refreshing;
        public bool refreshing
        {
            get { return _refreshing; }
            set
            {
                _refreshing = value;
                onPropertyChanged(nameof(refreshing));
            }
        }

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

        private ObservableCollection<TrophyYear> _trophies;
        public ObservableCollection<TrophyYear> trophies
        {
            get { return _trophies; }
            set
            {
                _trophies = value;
                onPropertyChanged(nameof(trophies));
            }
        }

        private ObservableCollection<Player> _appearances;
        public ObservableCollection<Player> appearances
        {
            get { return _appearances; }
            set
            {
                _appearances = value;
                onPropertyChanged(nameof(appearances));
            }
        }

        private ObservableCollection<Player> _goals;
        public ObservableCollection<Player> goals
        {
            get { return _goals; }
            set
            {
                _goals = value;
                onPropertyChanged(nameof(goals));
            }
        }

        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_BackCommand == null)
                {
                    _BackCommand = new Command(GoBack, CanGoBack);
                }
                return _BackCommand;
            }
            set { _BackCommand = value; }
        }

        private ICommand _RefreshStatsCommand;
        public ICommand RefreshStatsCommand
        {
            get
            {
                if (_RefreshStatsCommand == null)
                {
                    _RefreshStatsCommand = new Command(RefreshStats, CanRefreshStats);
                }
                return _RefreshStatsCommand;
            }
            set { _RefreshStatsCommand = value; }
        }

        public StatsViewModel()
        {
            hasBackButtonHardware = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");

            stats = new List<Stat>()
            {
                new Stat(){name = "Trophies", url = "https://onedrive.live.com/download?resid=341DB5D34CE90A21!112&authkey=!AAuqRdhBlQ3Lbv4&ithint=file%2cxml"},
                new Stat(){name = "Appearances", url = "https://onedrive.live.com/download?resid=341DB5D34CE90A21!110&authkey=!AJVhFM8EXjnHeds&ithint=file%2cxml"},
                new Stat(){name = "Goals", url = "https://onedrive.live.com/download?resid=341DB5D34CE90A21!109&authkey=!AFkH9wIbn3eORwY&ithint=file%2cxml"}
            };

            ReadStats();
        }

        private bool CanGoBack()
        {
            return true;
        }

        private void GoBack()
        {
            ((App)Application.Current).rootFrame.GoBack();
        }

        private bool CanRefreshStats()
        {
            return !refreshing;
        }

        private async void RefreshStats()
        {
            bool errors = false;

            refreshing = true;

            foreach (Stat stat in stats)
            {
                errors = await RefreshStat(stat);
                if (errors)
                {
                    await ((App)Application.Current).NoConnectionPopup();
                    break;
                }
            }

            ReadStats();

            refreshing = false;
        }

        private async void ReadStats()
        {
            foreach (Stat stat in stats)
            {
                reader = DataReaderFactory.GetDataReader(stat.name);

                switch (stat.name)
                {
                    case "Trophies":
                        trophies = await reader.GetAllTrophyYears();
                        break;
                    case "Appearances":
                        appearances = await reader.GetAllAppearances();
                        break;
                    case "Goals":
                        goals = await reader.GetAllGoals();
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task<bool> RefreshStat(Stat stat)
        {
            try
            {
                refreshing = true;

                //Download the file
                WebResponse resp = await WebRequest.Create(stat.url).GetResponseAsync();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string source = sr.ReadToEnd();

                //Write the file
                StorageFile _file = await ApplicationData.Current.LocalFolder.GetFileAsync(stat.name + ".xml");
                await FileIO.WriteTextAsync(_file, source);

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}

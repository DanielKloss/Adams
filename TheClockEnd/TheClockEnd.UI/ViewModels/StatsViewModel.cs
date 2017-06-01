using MvvmDialogs;
using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using TheClockEnd.Data;
using TheClockEnd.Data.Models;
using Windows.UI.Xaml.Controls;

namespace TheClockEnd.UI.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        private IDialogService _dialogService;
        private XmlDataService _service;
        private XmlDataReader _reader;

        private ObservableCollection<TrophyYear> _trophies;
        public ObservableCollection<TrophyYear> trophies
        {
            get { return _trophies; }
            set
            {
                _trophies = value;
                OnPropertyChanged(nameof(trophies));
            }
        }

        private ObservableCollection<Player> _appearances;
        public ObservableCollection<Player> appearances
        {
            get { return _appearances; }
            set
            {
                _appearances = value;
                OnPropertyChanged(nameof(appearances));
            }
        }

        private ObservableCollection<Player> _goals;
        public ObservableCollection<Player> goals
        {
            get { return _goals; }
            set
            {
                _goals = value;
                OnPropertyChanged(nameof(goals));
            }
        }

        public StatsViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _service = new XmlDataService();
            _reader = new XmlDataReader();
            ReadStats();
        }

        private async void ReadStats()
        {
            string trophiesResponse = await _service.MakeRequestAsync("Trophies");
            string appearancesResponse = await _service.MakeRequestAsync("Appearances");
            string goalsResponse = await _service.MakeRequestAsync("Goals");

            if (trophiesResponse == string.Empty || appearancesResponse == string.Empty || goalsResponse == string.Empty)
            {
                ConfirmContentDialogViewModel dialogViewModel = new ConfirmContentDialogViewModel("Connection Error", "There was an error connecting to the internet, please check your connection and try again");
                ContentDialogResult result = await _dialogService.ShowContentDialogAsync(dialogViewModel);
            }
            else
            {
                trophies = new ObservableCollection<TrophyYear>(_reader.GetAllTrophyYears(XDocument.Parse(trophiesResponse)));
                appearances = new ObservableCollection<Player>(_reader.GetAllAppearances(XDocument.Parse(appearancesResponse)));
                goals = new ObservableCollection<Player>(_reader.GetAllGoals(XDocument.Parse(goalsResponse)));
            }
        }
    }
}

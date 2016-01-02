using System.Collections.ObjectModel;
using TheClockEnd.Data;
using TheClockEnd.Models;

namespace TheClockEnd.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        private ICustomDataReader _reader;
        private IDataReaderFactory _factory;

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

        public StatsViewModel(IDataReaderFactory dataReaderFactory)
        {
            _factory = dataReaderFactory;
            _reader = _factory.Create();
            ReadStats();
        }

        public StatsViewModel()
        {
            _factory = new XmlDataReaderFactory();
            _reader = _factory.Create();
            ReadStats();
        }

        private async void ReadStats()
        {
            trophies = await _reader.GetAllTrophyYears();
            appearances = await _reader.GetAllAppearances();
            goals = await _reader.GetAllGoals();
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TheClockEnd.Models;

namespace TheClockEnd.Data
{
    public interface ICustomDataReader
    {
        Task LoadFile(Uri xmlLocation);
        Task<ObservableCollection<TrophyYear>> GetAllTrophyYears();
        Task<ObservableCollection<Player>> GetAllAppearances();
        Task<ObservableCollection<Player>> GetAllGoals();
    }
}

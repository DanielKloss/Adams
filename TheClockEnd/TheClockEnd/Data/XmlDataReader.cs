using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TheClockEnd.Models;
using Windows.Storage;

namespace TheClockEnd.Data
{
    public class XmlDataReader : ICustomDataReader
    {
        public XDocument xmlFile;

        public async Task LoadFile(Uri xmlLocation)
        {
            StorageFile MyFile = await StorageFile.GetFileFromApplicationUriAsync(xmlLocation);
            var stream = await MyFile.OpenStreamForReadAsync();
            xmlFile = XDocument.Load(stream);
        }

        public async Task<ObservableCollection<TrophyYear>> GetAllTrophyYears()
        {
            await LoadFile(new Uri("ms-appx:///Data/Trophies.xml"));

            ObservableCollection<TrophyYear> yearsToReturn = new ObservableCollection<TrophyYear>();

            var years = from year in xmlFile.Descendants("TrophyYear")
                        select new
                        {
                            trophyYear = year.Element("Year").Value,
                            trophies = year.Elements("Trophy")
                        };

            foreach (var year in years)
            {
                TrophyYear trophyYear = new TrophyYear()
                {
                    year = year.trophyYear,
                    trophyUrls = new List<string>()
                };

                foreach (var trophy in year.trophies)
                {
                    trophyYear.trophyUrls.Add("/Assets/Trophies/" + trophy.Value + ".png");
                }

                yearsToReturn.Add(trophyYear);
            }

            return yearsToReturn;
        }

        public async Task<ObservableCollection<Player>> GetAllAppearances()
        {
            await LoadFile(new Uri("ms-appx:///Data/Appearances.xml"));

            ObservableCollection<Player> appearancessToReturn = new ObservableCollection<Player>();

            var appearances = from appearance in xmlFile.Descendants("Appearance")
                              select new
                              {
                                  appearanceName = appearance.Element("Name").Value,
                                  appearanceAppearances = appearance.Element("Appearances").Value,
                                  appearanceShirtNumber = appearance.Element("Number").Value,
                              };

            foreach (var appearance in appearances)
            {
                Player app = new Player()
                {
                    name = appearance.appearanceName,
                    stat = appearance.appearanceAppearances,
                    number = "/Assets/Shirts/" + appearance.appearanceShirtNumber + ".png"
                };

                appearancessToReturn.Add(app);
            }

            return appearancessToReturn;
        }

        public async Task<ObservableCollection<Player>> GetAllGoals()
        {
            await LoadFile(new Uri("ms-appx:///Data/Goals.xml"));

            ObservableCollection<Player> goalsToReturn = new ObservableCollection<Player>();

            var goals = from goal in xmlFile.Descendants("Goal")
                        select new
                        {
                            goalName = goal.Element("Name").Value,
                            goalGoals = goal.Element("Goals").Value,
                            goalShirtNumber = goal.Element("Number").Value,
                        };

            foreach (var goal in goals)
            {
                Player scored = new Player()
                {
                    name = goal.goalName,
                    stat = goal.goalGoals,
                    number = "/Assets/Shirts/" + goal.goalShirtNumber + ".png"
                };

                goalsToReturn.Add(scored);
            }

            return goalsToReturn;
        }
    }
}

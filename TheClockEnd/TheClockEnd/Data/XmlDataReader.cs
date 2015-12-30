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
        private string _xmlLocation;
        public XDocument xmlFile;

        public XmlDataReader(string xmlLocation)
        {
            _xmlLocation = xmlLocation;
        }

        public async Task LoadFile(string xmlLocation)
        {
            StorageFile MyFile = await ApplicationData.Current.LocalFolder.GetFileAsync(xmlLocation);
            var stream = await MyFile.OpenStreamForReadAsync();
            xmlFile = XDocument.Load(stream);
        }

        public async Task<ObservableCollection<TrophyYear>> GetAllTrophyYears()
        {
            if (xmlFile == null)
            {
                await LoadFile(_xmlLocation);
            }

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
            if (xmlFile == null)
            {
                await LoadFile(_xmlLocation);
            }

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
            if (xmlFile == null)
            {
                await LoadFile(_xmlLocation);
            }

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

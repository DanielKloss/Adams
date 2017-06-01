using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TheClockEnd.Data.Models;

namespace TheClockEnd.Data
{
    public class XmlDataReader
    {
        public ICollection<TrophyYear> GetAllTrophyYears(XDocument xmlFile)
        {
            ICollection<TrophyYear> yearsToReturn = new List<TrophyYear>();

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

        public ICollection<Player> GetAllAppearances(XDocument xmlFile)
        {
            ICollection<Player> appearancessToReturn = new List<Player>();

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

        public ICollection<Player> GetAllGoals(XDocument xmlFile)
        {
            ICollection<Player> goalsToReturn = new List<Player>();

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

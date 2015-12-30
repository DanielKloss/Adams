using System.Collections.Generic;
using TheClockEnd.Models;

namespace TheClockEnd.DesignData
{
    public class TophiesDesignData
    {
        private List<TrophyYear> _trophies;
        public List<TrophyYear> trophies
        {
            get { return _trophies; }
            set { _trophies = value; }
        }

        public TophiesDesignData()
        {
            trophies = new List<TrophyYear>()
            {
                new TrophyYear()
                {
                    year="2015",
                    trophyUrls = new List<string>()
                                {
                                    "FA Cup",
                                    "Premier League"
                                }
                }
            };
        }
    }
}

using System.Collections.Generic;

namespace TheClockEnd.Models
{
    public class TrophyYear : Stat
    {
        private string _year;
        public string year
        {
            get { return _year; }
            set { _year = value; }
        }

        private List<string> _trophyUrls;
        public List<string> trophyUrls
        {
            get { return _trophyUrls; }
            set { _trophyUrls = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj is TrophyYear)
            {
                TrophyYear other = (TrophyYear)obj;
                return Equals(other.trophyUrls, trophyUrls) && Equals(other.year, year);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

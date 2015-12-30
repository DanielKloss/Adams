using System;

namespace TheClockEnd.DesignData
{
    public class ClockDesignData
    {
        private DateTime _theTime;
        public DateTime theTime
        {
            get { return _theTime; }
            set { _theTime = value; }
        }

        public ClockDesignData()
        {
            theTime = DateTime.Now;
        }
    }
}

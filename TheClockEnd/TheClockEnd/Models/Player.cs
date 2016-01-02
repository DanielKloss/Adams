namespace TheClockEnd.Models
{
    public class Player : Stat
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _stat;
        public string stat
        {
            get { return _stat; }
            set { _stat = value; }
        }

        private string _number;
        public string number
        {
            get { return _number; }
            set { _number = value; }
        }
    }
}

using System;
using Windows.UI.Xaml.Controls;

namespace TheClockEnd.Models
{
    public class MenuItem
    {
        private Symbol _symbol;
        public Symbol symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private char _symbolAsChar;
        public char symbolAsChar
        {
            get { return (char)this.symbol; }
            set { _symbolAsChar = value; }
        }

        private string _label;
        public string label
        {
            get { return _label; }
            set { _label = value; }
        }

        private Type _destPage;
        public Type destPage
        {
            get { return _destPage; }
            set { _destPage = value; }
        }
    }
}

using System.ComponentModel;

namespace TheClockEnd.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region INPC
        protected void onPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

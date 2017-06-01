using System.ComponentModel;

namespace TheClockEnd.Data.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region INPC
        protected void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

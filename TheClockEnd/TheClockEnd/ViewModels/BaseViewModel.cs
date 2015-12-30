using System.ComponentModel;

namespace TheClockEnd.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
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

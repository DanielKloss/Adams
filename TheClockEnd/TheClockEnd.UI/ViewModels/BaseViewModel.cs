﻿using System.ComponentModel;
using System.Windows.Input;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace TheClockEnd.UI.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _hasBackButtonHardware;
        public bool hasBackButtonHardware
        {
            get { return _hasBackButtonHardware; }
            set
            {
                _hasBackButtonHardware = value;
                OnPropertyChanged(nameof(hasBackButtonHardware));
            }
        }

        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_BackCommand == null)
                {
                    _BackCommand = new Command(GoBack, CanGoBack);
                }
                return _BackCommand;
            }
            set { _BackCommand = value; }
        }

        public BaseViewModel()
        {
            hasBackButtonHardware = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }

        private void GoBack()
        {
            ((App)Application.Current).rootFrame.GoBack();
        }

        private bool CanGoBack()
        {
            return true;
        }

        #region INPC
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

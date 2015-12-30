using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TheClockEnd.Helpers;
using TheClockEnd.Models;
using TheClockEnd.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TheClockEnd.ViewModels
{
    public class NavigationViewModel : INotifyPropertyChanged
    {
        private Type _currentPage;
        public Type currentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                onPropertyChanged("currentPage");
            }
        }

        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> menuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                onPropertyChanged("menuItems");
            }
        }

        public NavigationViewModel()
        {
            menuItems = new ObservableCollection<MenuItem>()
            {
                //new MenuItem()
                //{
                //    label = "Home",
                //    symbol = Symbol.Clock,
                //    destPage = typeof(MainPage)
                //},
                new MenuItem()
                {
                    label = "Stats",
                    symbol = Symbol.FourBars,
                    destPage = typeof(Stats)
                },
                new MenuItem()
                {
                    label = "About",
                    symbol = Symbol.Help,
                    destPage = typeof(About)
                }
            };

            currentPage = menuItems.First().destPage;
        }

        private ICommand _navigateCommand;
        public ICommand navigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                {
                    _navigateCommand = new Command<MenuItem>(Navigate, CanNavigate);
                }
                return _navigateCommand;
            }
            set { _navigateCommand = value; }
        }

        private bool CanNavigate(object argument)
        {
            return true;
        }

        public void Navigate(object arguemnt)
        {
            MenuItem menuItem = arguemnt as MenuItem;

            if (((App)Application.Current).rootFrame.CurrentSourcePageType != menuItem.destPage)
            {
                ((App)Application.Current).rootFrame.Navigate(menuItem.destPage);
            }
        }

        #region INPC
        private void onPropertyChanged(string propertyName)
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
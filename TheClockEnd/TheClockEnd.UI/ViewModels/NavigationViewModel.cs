using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TheClockEnd.Data.Models;
using TheClockEnd.UI.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TheClockEnd.UI.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private Type _currentPage;
        public Type currentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(currentPage));
            }
        }

        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> menuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(menuItems));
            }
        }

        public NavigationViewModel()
        {
            menuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem()
                {
                    label = "Stats",
                    symbol = Symbol.FourBars,
                    destPage = typeof(StatsView)
                },
                new MenuItem()
                {
                    label = "About",
                    symbol = Symbol.Help,
                    destPage = typeof(AboutView)
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

        private bool CanNavigate(MenuItem argument)
        {
            return true;
        }

        public void Navigate(MenuItem menuItem)
        {
            if (((App)Application.Current).rootFrame.CurrentSourcePageType != menuItem.destPage)
            {
                ((App)Application.Current).rootFrame.Navigate(menuItem.destPage);
            }
        }
    }
}

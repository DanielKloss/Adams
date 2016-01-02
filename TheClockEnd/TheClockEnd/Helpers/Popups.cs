using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace TheClockEnd.Helpers
{
    public class Popups
    {
        public async Task NoConnectionPopup()
        {
            CoreDispatcher coreDispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            var message = "No internet connection has been found. Please check your connection and try again.";
            var title = "Connection Error";
            var messageDialog = new MessageDialog(message);
            messageDialog.Title = title;
            messageDialog.Commands.Add(new UICommand("Ok"));
            await messageDialog.ShowAsync();
        }
    }
}

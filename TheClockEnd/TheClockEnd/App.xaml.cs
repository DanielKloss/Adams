using BackgroundTasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TheClockEnd.Helpers;
using TheClockEnd.Models;
using TheClockEnd.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TheClockEnd
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public Frame rootFrame;
        private const string TASKNAME = "ClockBackgroundTask";
        private const string TASKENTRYPOINT = "BackgroundTasks.ClockBackgroundTask";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values["FirstTime"] == null || (bool)ApplicationData.Current.LocalSettings.Values["FirstTime"])
            {
                await FirstTimeSetup();
                ApplicationData.Current.LocalSettings.Values["FirstTime"] = false;
            }

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(Clock), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();

            //Register hardware back button
            SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonPressed;

            //Register Background Task to update clock live tile
            WindowsLiveTileSchedule.CreateSchedule();
            RegisterClockTask();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void BackButtonPressed(object sender, BackRequestedEventArgs e)
        {
            var frame = ((App)Application.Current).rootFrame;
            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        private async void RegisterClockTask()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity || result == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == TASKNAME)
                    {
                        return;
                    }
                }
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                builder.Name = TASKNAME;
                builder.TaskEntryPoint = TASKENTRYPOINT;
                builder.SetTrigger(new TimeTrigger(240, false));
                builder.Register();
            }
        }

        private async Task FirstTimeSetup()
        {
            bool errors = false;

            List<Stat> stats = new List<Stat>()
            {
                new Stat(){name = "Trophies", url = "https://onedrive.live.com/download?resid=341DB5D34CE90A21!112&authkey=!AAuqRdhBlQ3Lbv4&ithint=file%2cxml"},
                new Stat(){name = "Appearances", url = "https://onedrive.live.com/download?resid=341DB5D34CE90A21!110&authkey=!AJVhFM8EXjnHeds&ithint=file%2cxml"},
                new Stat(){name = "Goals", url = "https://onedrive.live.com/download?resid=341DB5D34CE90A21!109&authkey=!AFkH9wIbn3eORwY&ithint=file%2cxml"}
            };


            foreach (Stat stat in stats)
            {
                string fileName = stat.name + ".xml";

                //Check if dependant files exist
                if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName) == null)
                {
                    //If they dont, create them
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName);
                    //Write the file
                    StorageFile _file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                    await FileIO.WriteTextAsync(_file, "<?xml version='1.0' encoding='utf-8' ?><" + stat.name + "></" + stat.name + ">");
                }

                errors = await RefreshStat(stat);
                if (errors)
                {
                    await NoConnectionPopup();
                    break;
                }
            }
        }

        private async Task<bool> RefreshStat(Stat stat)
        {
            try
            {
                //Download the file
                WebResponse resp = await WebRequest.Create(stat.url).GetResponseAsync();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string source = sr.ReadToEnd();

                //Write the file
                StorageFile _file = await ApplicationData.Current.LocalFolder.GetFileAsync(stat.name + ".xml");
                await FileIO.WriteTextAsync(_file, source);

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

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

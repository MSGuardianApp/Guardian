using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace SOS.Phone.Pages
{
    public partial class Locate : PhoneApplicationPage
    {
        DispatcherTimer timer;

        public Locate()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception exception)
            {
                //ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RefreshPage();
        }

        private async void RefreshPage()
        {
            if (Globals.IsRegisteredUser)
            {
                DataContext = App.LocateBuddies;
                BuddyPanel.Visibility = Visibility.Visible;

                //if(App.LocateBuddies.LocateBuddies.Count
                //App.LocateBuddies.LoadLocateBuddies();
                this.progbar.Visibility = System.Windows.Visibility.Visible;
                await App.LocateBuddies.RefreshLocateBuddies();
                this.progbar.Visibility = System.Windows.Visibility.Collapsed;

                timer = new DispatcherTimer();
                RunInBackground((int)Constants.RefreshInterval);
            }
            else
            {
                RegisterPanel.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = false;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (timer != null) timer.Stop();
            timer = null;

            base.OnNavigatedFrom(e);
        }

        public void RunInBackground(int runEveryNMilSecs)
        {
            timer.Tick +=
            delegate(object s, EventArgs args)
            {
                this.progbar.Visibility = System.Windows.Visibility.Visible;
                App.LocateBuddies.RefreshLocateBuddies();
                this.progbar.Visibility = System.Windows.Visibility.Collapsed;

            };

            timer.Start();
            timer.Interval = new TimeSpan(0, 0, 0, 0, runEveryNMilSecs);
        }

        private void BuddyUC_TrackNavigationHandler(object sender, EventArgs e)
        {
            timer.Stop();
            timer = null;
            NavigationService.Navigate(((NavigationEventArgs)e).Uri);
        }

        private async void Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (Globals.IsRegisteredUser)
                {
                    this.progbar.Visibility = System.Windows.Visibility.Visible;
                    await App.LocateBuddies.RefreshLocateBuddies();
                    this.progbar.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception exception)
            {
                this.progbar.Visibility = System.Windows.Visibility.Collapsed;
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

    }
}
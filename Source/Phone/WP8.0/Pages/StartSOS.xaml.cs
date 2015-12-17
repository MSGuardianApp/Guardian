using Microsoft.Phone.Controls;
using System;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace SOS.Phone.Pages
{
    public partial class StartSOS : PhoneApplicationPage
    {
        //TODO: To discuss back button and other button press while the counter is on.
        DispatcherTimer dispatcherTimer = null;
        int counter = 1;

        public StartSOS()
        {
            InitializeComponent();

            if (!Globals.CurrentProfile.IsSOSOn)
                ShowCounter();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string IsFromTile = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("DefaultTitle", out IsFromTile) && (IsFromTile == "SOSTile") && Globals.CurrentProfile.IsSOSOn )
                NavigationService.Navigate(new Uri("/Pages/SOS.xaml?DefaultTitle=SOSTile", UriKind.Relative));

            if (StartCounterTextBlock.Text == 1.ToString())
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void StartSosApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.dispatcherTimer != null)
                this.dispatcherTimer.Stop();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ShowCounter()
        {
            if (this.dispatcherTimer == null)
            {
                this.dispatcherTimer = new DispatcherTimer();
                this.dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
                this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            }

            this.dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            StartCounterTextBlock.Text = (Constants.SOSCountdownCounter - counter).ToString();
            this.counter++;
            if (this.counter >= Constants.SOSCountdownCounter)
            {
                this.dispatcherTimer.Stop();

                if (!StateUtility.IsRunningInBackground)
                    StartSosImmediately();
            }
        }

        private void CancelSOS_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.dispatcherTimer.Stop();            
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

        }

        private void StartSOS_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (this.dispatcherTimer != null)
                this.dispatcherTimer.Stop();

            StartSosImmediately();
        }

        private void StartSosImmediately() 
        {
            string IsFromTile = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("DefaultTitle", out IsFromTile) && (IsFromTile == "SOSTile"))
                NavigationService.Navigate(new Uri("/Pages/SOS.xaml?DefaultTitle=SOSTile", UriKind.Relative));
            else
                NavigationService.Navigate(new Uri("/Pages/SOS.xaml", UriKind.Relative));
            Globals.RetainSOSState = false;
        }
    }
}
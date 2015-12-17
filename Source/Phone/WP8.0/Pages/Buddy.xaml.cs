using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace SOS.Phone.Pages
{
    public partial class Buddy : PhoneApplicationPage
    {
        public Buddy()
        {
            try
            {
                InitializeComponent();
                this.DataContext = App.MyBuddies;
                this.Loaded += Buddy_Loaded;
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
           
        }

        void Buddy_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.MyBuddies.IsDataLoaded == false)
                {
                    App.MyBuddies.LoadBuddies(Globals.User.CurrentProfileId);
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
           
        }

        private void BuddyUC_TrackNavigationHandler(object sender, EventArgs e)
        {
            NavigationService.Navigate(((NavigationEventArgs)e).Uri);
        }

    }
}
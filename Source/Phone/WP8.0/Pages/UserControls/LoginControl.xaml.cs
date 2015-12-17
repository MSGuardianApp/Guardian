using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SOS.Phone.UserControls
{
    public partial class LoginControl : UserControl
    {
        public Page ParentPage { get; set; }
        
        public LoginControl()
        {
            InitializeComponent();

            this.Loaded += delegate
            {
                ParentPage = (Application.Current.RootVisual as Frame).Content as Page;
            };
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Globals.IsDataNetworkAvailable)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.NetworkNotAvailableText, "basicWrap", "Connection Unavailable"));
                return;
            }
            ParentPage.NavigationService.Navigate(new Uri("/Pages/Register.xaml?Scenario=Register", UriKind.Relative));
        }

        private void PrivacyLink_MouseEnter(object sender, MouseEventArgs e)
        {
            Globals.ShowPrivacy();
        }
    }
}


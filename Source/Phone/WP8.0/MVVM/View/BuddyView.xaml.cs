using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace SOS.Phone.UserControls
{
    public partial class BuddyView : UserControl
    {
        public event EventHandler TrackHandler;

        #region Properties

        public static readonly DependencyProperty _myBuddiesSourceProperty =
        DependencyProperty.Register("IsMyBuddy", typeof(bool), typeof(BuddyView), new PropertyMetadata(false));

        public static readonly DependencyProperty _locateBuddiesSourceProperty =
        DependencyProperty.Register("IsLocateBuddy", typeof(bool), typeof(BuddyView), new PropertyMetadata(false));

        public static readonly DependencyProperty _myGroupsSourceProperty =
        DependencyProperty.Register("IsGroup", typeof(bool), typeof(BuddyView), new PropertyMetadata(false));

        public bool IsMyBuddy
        {
            set { SetValue(_myBuddiesSourceProperty, value); }
            get { return (bool)GetValue(_myBuddiesSourceProperty); }
        }

        public bool IsLocateBuddy
        {
            set { SetValue(_locateBuddiesSourceProperty, value); }
            get { return (bool)GetValue(_locateBuddiesSourceProperty); }
        }

        public bool IsGroup
        {
            set { SetValue(_myGroupsSourceProperty, value); }
            get { return (bool)GetValue(_myGroupsSourceProperty); }
        }
        #endregion

        public BuddyView()
        {
            InitializeComponent();
            this.Loaded += PeopleUC_Loaded;
        }

        void PeopleUC_Loaded(object sender, RoutedEventArgs e)
        {
            string imageMapSource = @"\Assets\Images\mapicon.png";
            string imageCallSource = @"\Assets\Images\call.png";
            if (this.IsLocateBuddy)
            {
                FirstIconImage.Source = new BitmapImage(new Uri(imageMapSource, UriKind.Relative));
                SecondIconImage.Source = new BitmapImage(new Uri(imageCallSource, UriKind.Relative));
            }
            else if (this.IsMyBuddy || this.IsGroup)
            {
                FirstIconImage.Source = new BitmapImage(new Uri(imageCallSource, UriKind.Relative));
                //SecondIconImage.Source = new BitmapImage(new Uri(imageMapSource, UriKind.Relative));
                SecondIconPanel.Visibility = Visibility.Collapsed;
                NamePanel.Width = 270;
                FirstIconPanel.Width = 325;
            }

            if (this.IsGroup || this.IsMyBuddy)
                AddressPanel.Visibility = Visibility.Collapsed;
        }

        public void MethodToNavigateToPage(Uri uri)
        {
            try
            {
                var e = new NavigationEventArgs(null, uri);
                if (TrackHandler != null)
                {
                    TrackHandler(this, e);
                }
            }
            catch (Exception)
            {
                CallErrorHandler();
            }
        }

        private void BuddyName_TapEvent(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                StackPanel oSPName = (StackPanel)sender;
                if (this.IsLocateBuddy)
                {
                    var oData = (LocateBuddyTableEntity)oSPName.DataContext;
                    MethodToNavigateToPage(new Uri(string.Format(Constants.MapPageUrl + "?ProfileId={0}&Name={1}", oData.BuddyProfileId, oData.Name), UriKind.Relative));
                }
                else if (this.IsMyBuddy)
                {
                    var oData = (BuddyTableEntity)oSPName.DataContext;
                    Utility.InitiateCall(new Callee() { DisplayName = oData.Name, PhoneNumber = oData.PhoneNumber });
                }
                else
                {
                    var oData = (GroupTableEntity)oSPName.DataContext;
                    Utility.InitiateCall(new Callee() { DisplayName = oData.Name, PhoneNumber = oData.PhoneNumber });
                }
            }
            catch (Exception)
            {
                CallErrorHandler();
            }
        }

        private void btnCallBuddy_click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                //BL: This event will be fired only for IsLocateBuddy
                StackPanel oSPName = (StackPanel)sender;
                var oData = (LocateBuddyTableEntity)oSPName.DataContext;
                Utility.InitiateCall(new Callee() { DisplayName = oData.Name, PhoneNumber = oData.PhoneNumber });
            }
            catch (Exception)
            {
                CallErrorHandler();
            }
        }

        private async void btnSMSBuddy_click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                StackPanel oSPName = (StackPanel)sender;
                string message = this.IsLocateBuddy ? Constants.LocateBuddyMessageText : await Utility.GetDistressMessage();

                if (this.IsGroup)
                {
                    GroupTableEntity oData = (GroupTableEntity)oSPName.DataContext;
                    Utility.SendSMS(new SMSMessage() { PhoneNumbers = oData.PhoneNumber, Message = message });
                }
                else if (this.IsLocateBuddy)
                {
                    LocateBuddyTableEntity oData = (LocateBuddyTableEntity)oSPName.DataContext;
                    Utility.SendSMS(new SMSMessage() { PhoneNumbers = oData.PhoneNumber, Message = message });
                }
                else
                {
                    BuddyTableEntity oData = (BuddyTableEntity)oSPName.DataContext;
                    Utility.SendSMS(new SMSMessage() { PhoneNumbers = oData.PhoneNumber, Message = message });
                }
            }
            catch (Exception ex)
            {
                CallErrorHandler();
            }
        }

        private async void btnEmail_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                StackPanel oSPName = (StackPanel)sender;
                EmailComposeTask emailcomposer = new EmailComposeTask();
                if (this.IsGroup)
                {
                    GroupTableEntity oData = (GroupTableEntity)oSPName.DataContext;
                    emailcomposer.To = oData.Email;
                }
                else if (this.IsLocateBuddy)
                {
                    LocateBuddyTableEntity oData = (LocateBuddyTableEntity)oSPName.DataContext;
                    emailcomposer.To = oData.Email;
                }
                else
                {
                    BuddyTableEntity oData = (BuddyTableEntity)oSPName.DataContext;
                    emailcomposer.To = oData.Email;
                }

                string strMessage = Constants.MessageTemplateText;
                if (Globals.CurrentProfile.MessageTemplate.Trim() != string.Empty)
                    strMessage = Globals.CurrentProfile.MessageTemplate;
                if (this.IsLocateBuddy)
                    strMessage = "I'm reaching to help you";
                emailcomposer.Subject = strMessage;

                emailcomposer.Body = this.IsLocateBuddy ? Constants.LocateBuddyMessageText : await Utility.GetDistressMessage();

                emailcomposer.Show();
            }
            catch (Exception)
            {
                CallErrorHandler();
            }
        }

        private void CallErrorHandler()
        {            
           MessageBox.Show("Error has occured. Please try again later", "Error", MessageBoxButton.OK);
        }

    }
}


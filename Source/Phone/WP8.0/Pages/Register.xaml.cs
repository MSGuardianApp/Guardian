using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Input;
using System.Text;
using Microsoft.Live;
using System.Threading.Tasks;
using SOS.Phone.MembershipServiceRef;
using SOS.Phone.ServiceWrapper;
using System.Threading;
using System.Text.RegularExpressions;

namespace SOS.Phone
{
    public partial class Register : PhoneApplicationPage
    {
        public CancellationTokenSource tokenValidateButton = null;
        private bool IsMobileNumberEdited = false;
        private string AccessToken = string.Empty;
        private string RefreshToken = string.Empty;
        CountryCodes selectedCountryCode = null;
        public bool IsComingFromBackground = false;
        public Register()
        {
            try
            {
                InitializeComponent();
                SetControls();
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        #region Navigation Event Handlers

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (tokenValidateButton != null)
                tokenValidateButton.Cancel();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                ProfileScrollViewer.Height = Application.Current.RootVisual.RenderSize.Height - 100;

                if (!Config.IsEnterpriseBuild)
                {
                    EnterpriseEmailPanel.Visibility = Visibility.Collapsed;
                    EmailKeyPanel.Visibility = Visibility.Collapsed;
                }

                string scenario = "Unknown";
                if (NavigationContext.QueryString.TryGetValue("Scenario", out scenario))
                {
                    switch (scenario)
                    {
                        case "MobileNumberUpdate":
                            IsMobileNumberEdited = true;
                            if (e.NavigationMode == NavigationMode.New)
                                LoadExistingProfileDetails();
                            break;
                        case "Register":
                            if (e.NavigationMode == NavigationMode.New || IsComingFromBackground)
                            {
                                IsComingFromBackground = false;
                                LiveLogin_Click(null, null);
                            }
                            break;
                        default:
                            break;
                    }
                }


                Dispatcher.BeginInvoke(() =>
                {
                    RenderUI();
                });

            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }

        private void LoadExistingProfileDetails()
        {
            try
            {
                if (Globals.IsRegisteredUser)
                {
                    //NameTextBox.Text = App.CurrentUser.User.Name;
                    //LiveIdTextBox.Text = App.CurrentUser.User.LiveEmail;
                    ProfileHeading.Text = "Update Mobile Number";
                    string Number = App.MyProfiles.CurrentProfile.MobileNumber;
                    MobileNumberTextBox.Text = Number.Replace(Constants.CountryCode, "");
                    ProfilePanel.Visibility = Visibility.Visible;
                    WaitTextBlock.Visibility = Visibility.Collapsed;
                    CountryCodeListPicker.SelectedIndex = CountryCodeListPicker.Items.IndexOf(CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == Constants.CountryCode)));
                    MobileNumberTextBox.Focus();
                    EnterpriseEmailPanel.Visibility = Visibility.Collapsed;
                    EmailKeyPanel.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (!Globals.IsRegisteredUser)
                NavigationService.GoBack();
            base.OnBackKeyPress(e);


        }

        #endregion Navigation Event Handlers

        #region Live Id Authentication
        /// <summary>
        ///     Defines the scopes the application needs.
        /// </summary>
        private static readonly string[] scopes = new string[] { "wl.emails", "wl.skydrive_update", "wl.offline_access" };

        /// <summary>
        ///     Stores the LiveAuthClient instance.
        /// </summary>
        private LiveAuthClient authClient;

        /// <summary>
        ///     Stores the LiveConnectClient instance.
        /// </summary>
        private LiveConnectClient liveClient;


        private void LiveLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tokenValidateButton != null)
                tokenValidateButton.Cancel();
            ValidateNumber.IsEnabled = true;
            InitializeSignInAsync();
        }

        /// <summary>
        ///     Calls LiveAuthClient.Initialize to get the user login status.
        ///     Retrieves user profile information if user is already signed in.
        /// </summary>
        private async void InitializeSignInAsync()
        {
            try
            {
                this.authClient = new LiveAuthClient(Config.LiveAuthClientId);
                m_ProgressBar.Visibility = System.Windows.Visibility.Visible;
                LiveLoginResult loginResult = await this.authClient.LoginAsync(scopes);
                if (loginResult.Status == LiveConnectSessionStatus.Connected)
                {
                    this.liveClient = new LiveConnectClient(loginResult.Session);
                    AccessToken = loginResult.Session.AccessToken;
                    RefreshToken = loginResult.Session.RefreshToken;
                    LiveOperationResult operationResult = await this.liveClient.GetAsync("me");
                    dynamic properties = operationResult.Result;
                    LiveIdTextBox.Text = properties.emails.account;
                    LiveAuthTokenPanel.Visibility = System.Windows.Visibility.Visible;
                    WaitTextBlock.Visibility = System.Windows.Visibility.Collapsed;

                    if (NameTextBox.Text.Trim() == string.Empty && properties.name != null)
                        NameTextBox.Text = properties.name;

                    //LiveLoginButton.Visibility = Visibility.Collapsed;
                    //Save Email to Local Storage
                    Globals.User.Name = NameTextBox.Text;
                    Globals.User.LiveEmail = LiveIdTextBox.Text;
                    Globals.User.LiveAuthId = loginResult.Session.AuthenticationToken;
                    App.CurrentUser.UpdateUser(Globals.User);


                    //If user already exists, get all his info and store locally. Delete any local profile //TODO
                    ProfileList profiles = null;
                    try
                    {
                        profiles = await MembershipServiceWrapper.GetProfilesForLiveId();
                    }
                    catch (Exception ex)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnableToConnectService, "basicWrap", "Information!"));
                        return;
                    }
                    if (profiles != null && profiles.List.Count > 0)
                    {
                        //Load all profiles to Local Storage 
                        // + Assign first profile as CurrentProfile  

                        //If atleast one buddy, retain the profile. Else delete!
                        //Globals.CurrentProfile.MobileNumber = "0000000000";

                        profiles.List[0].IsSOSOn = Globals.CurrentProfile.IsSOSOn;
                        profiles.List[0].IsTrackingOn = Globals.CurrentProfile.IsTrackingOn;
                        //profiles.List[0].SessionID = Globals.CurrentProfile.SessionToken;

                        App.MyProfiles.DeleteOfflineProfile();

                        UserViewModel.RestoreAllProfiles(profiles.List);

                        Globals.Load2CurrentProfile(profiles.List[0].ProfileID.ToString());

                        Globals.IsAutoUpgradeFailed = false;
                        //if (NavigationService.BackStack != null && NavigationService.BackStack.Count() > 0)
                        //    NavigationService.RemoveBackEntry();

                        if (Globals.CurrentProfile.CountryCode != null) Globals.SetEmergencyNumbers(Globals.AllCountryCodes.First(c => c.IsdCode == Globals.CurrentProfile.CountryCode));

                        //Allow change of Current Profile in Settings
                        NavigationService.Navigate(new Uri("/Pages/Settings.xaml?FromPage=Register", UriKind.Relative));
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.ProfileLoadedSuccessfullyText, "basicWrap", "Info!"));
                    }
                    else
                    {
                        WaitTextBlock.Visibility = System.Windows.Visibility.Collapsed;
                        ProfilePanel.Visibility = Visibility.Visible;
                        CountryCodeListPicker.SelectedIndex = CountryCodeListPicker.Items.IndexOf(CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == Constants.CountryCode)));
                    }
                }
                else if (loginResult.Status == LiveConnectSessionStatus.NotConnected || loginResult.Status == LiveConnectSessionStatus.Unknown)
                {
                    NavigationService.Navigate(new Uri("/Pages/Settings.xaml?FromPage=Register", UriKind.Relative));
                }
                else
                {
                    WaitTextBlock.Text = CustomMessage.UnableToGetLoginInfoText;
                }
            }
            catch (LiveAuthException authExp)
            {
                if (authExp.ErrorCode == "access_denied")
                {
                    IsComingFromBackground = true;
                }
                WaitTextBlock.Text = CustomMessage.UnableToGetLoginInfoText;
                //TODO: Authentication Token Expired!
            }
            catch (LiveConnectException ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.LiveConnectExceptionText, "basicWrap", "Connection error."));
                WaitTextBlock.Text = CustomMessage.UnableToGetLoginInfoText;
            }
            finally
            {
                m_ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        #endregion

        #region Create Profile

        private async Task delayedWork()
        {
            try
            {
                tokenValidateButton = new CancellationTokenSource();
                await Task.Delay(Constants.TimeToDisableValidateButton, tokenValidateButton.Token);
                this.ReactivateValidateButton();
            }
            catch (Exception)
            {
                //ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        private void ReactivateValidateButton()
        {
            ValidateNumber.IsEnabled = true;
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            if (!IsMobileNumberEdited && Config.IsEnterpriseBuild && (string.IsNullOrWhiteSpace(EnterpriseEmailTextBox.Text) || !EnterpriseEmailTextBox.Text.EndsWith(Config.EnterpriseEmailDomain)))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidEnterpriseEmailWhileRegistering, "basicWrap", "Oops!"));
                EnterpriseEmailTextBox.Focus();
                return;
            }

            selectedCountryCode = (CountryCodes)CountryCodeListPicker.SelectedItem;
            if (selectedCountryCode == null)
            {
                selectedCountryCode = (CountryCodes)CountryCodeListPicker.Items[0];
                CountryCodeListPicker.SelectedIndex = CountryCodeListPicker.Items.IndexOf(CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == Constants.CountryCode)));
            }
            if (MobileNumberTextBox.Text == String.Empty || MobileNumberTextBox.Text.Length > int.Parse(selectedCountryCode.MaxPhoneDigits))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidMobileNumberWhileRegistering, "basicWrap", "Oops!"));
                MobileNumberTextBox.Focus();
                return;
            }

            if (IsMobileNumberEdited)
            {
                if (selectedCountryCode.IsdCode + MobileNumberTextBox.Text.Trim() == Globals.CurrentProfile.MobileNumber)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.MobileNumberEditingText, "basicWrap", "Oops!"));
                    MobileNumberTextBox.Focus();
                    return;
                }
            }
            //BL: Disable the button for 5 minutes 
            //Call Server to send SMS/ email
            try
            {
                m_ProgressBar.Visibility = Visibility.Visible;

                MembershipServiceWrapper.ValidateMobileNumberAsync(NameTextBox.Text, LiveIdTextBox.Text, selectedCountryCode.IsdCode, MobileNumberTextBox.Text, EnterpriseEmailTextBox.Text);
                m_ProgressBar.Visibility = Visibility.Collapsed;
                ValidateNumber.IsEnabled = false;
                Task ignoredAwaitableResult = this.delayedWork();

                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.MobileNumberValidatingSMSToast, "basicWrap", "Information!"));
            }
            catch (Exception)
            {
                m_ProgressBar.Visibility = Visibility.Collapsed;
            }

        }

        private async void ConfirmButton_Click(object sender, EventArgs e)
        {
            //Need to remove the back entry . Currently we are removing the back entry from Settings page.
            try
            {
                if (!ValidateInputs())
                    return;

                if (!Globals.IsDataNetworkAvailable)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.NetworkNotAvailableText, "basicWrap", "Connection Unavailable"));
                    return;
                }

                var countryDetails = selectedCountryCode == null ? (CountryCodes)CountryCodeListPicker.Items[CountryCodeListPicker.SelectedIndex] : selectedCountryCode;
                if ((countryDetails.IsdCode != Constants.CountryCode && countryDetails.CountryName != Constants.CountryName) || (countryDetails.IsdCode == Constants.CountryCode && countryDetails.CountryName != Constants.CountryName))
                {
                    if (!Globals.IsRegisteredUser)
                    {
                        var BuddiesToDelete = App.MyBuddies.Buddies.Count > 0 ? App.MyBuddies.Buddies.Where(c => !c.PhoneNumber.StartsWith(countryDetails.IsdCode)) : null;
                        if (BuddiesToDelete != null && BuddiesToDelete.Count() > 0)
                        {
                            StringBuilder message = new StringBuilder();
                            message.Append("The below buddies would be removed as they don't belong to ");
                            message.Append(countryDetails.CountryName + Environment.NewLine);

                            foreach (var buddy in BuddiesToDelete)
                            {
                                message.Append(buddy.Name + "(" + buddy.PhoneNumber + ")" + Environment.NewLine);
                            }

                            if (MessageBox.Show(message.ToString(), "Buddy update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                var count = BuddiesToDelete.Count();
                                while (BuddiesToDelete.Count() > 0)
                                {
                                    var buddy = BuddiesToDelete.ElementAt(0);

                                    if (buddy != null)
                                        App.MyBuddies.DeleteBuddy(buddy);
                                    if (App.MyBuddies.IsSuccess)
                                    {
                                    }
                                }
                                Globals.SetEmergencyNumbers(countryDetails);
                            }
                            else
                            {
                                CountryCodeListPicker.SelectedItem = CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == Constants.CountryCode));
                                return;
                            }
                        }
                        else
                            Globals.SetEmergencyNumbers(countryDetails);
                    }
                    else
                    {
                        var BuddiesToDelete = App.MyBuddies.Buddies.Count > 0 ? App.MyBuddies.Buddies : null;
                        if (BuddiesToDelete != null && BuddiesToDelete.Count() > 0)
                        {
                            if (MessageBox.Show(CustomMessage.RegisteredBuddiesDeleteText, "Buddy Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {

                                var count = BuddiesToDelete.Count();
                                while (BuddiesToDelete.Count() > 0)
                                {
                                    var buddy = BuddiesToDelete.ElementAt(0);

                                    if (buddy != null)
                                        App.MyBuddies.DeleteBuddy(buddy);
                                    if (App.MyBuddies.IsSuccess)
                                    {

                                    }
                                }

                                Globals.SetEmergencyNumbers(countryDetails);
                            }
                            else
                            {
                                CountryCodeListPicker.SelectedItem = CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == Constants.CountryCode));
                                return;
                            }
                        }
                        else
                            Globals.SetEmergencyNumbers(countryDetails);
                    }
                }

                Profile profile = null;
                m_ProgressBar.Visibility = Visibility.Visible;
                if (IsMobileNumberEdited)
                {
                    string message = await App.CurrentUser.UpdateUserLocal2Server(true, countryDetails.IsdCode + Utility.GetPlainPhoneNumber(MobileNumberTextBox.Text), HiddenSecurityCodeTextBox.Text.Trim());
                    if (message != "INCORRECTSECURITYCODE")
                        NavigationService.Navigate(new Uri("/Pages/Settings.xaml?ToPage=Profile&FromPage=Register", UriKind.Relative));

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (message == "PROFILENOTFOUND")
                            Globals.DisplayToast(CustomMessage.ProfileNotFound, "basicWrap", "Oops!");
                        else if (message == "INCORRECTSECURITYCODE")
                            Globals.DisplayToast(CustomMessage.IncorrectSecurityCode, "basicWrap", "Oops!");
                        else if (message == "ERROR")
                            Globals.DisplayToast(CustomMessage.UnableToUpdateMobile, "basicWrap", "Oops!");
                        else
                            Globals.DisplayToast(CustomMessage.ProfileLoadedSuccessfullyText, "basicWrap", "Success!");
                    });
                }
                else
                {
                    CountryCodeTextBox.Text = countryDetails.IsdCode;
                    profile = await MembershipServiceWrapper.CreateProfile(NameTextBox.Text, LiveIdTextBox.Text, CountryCodeTextBox.Text, Utility.GetPlainPhoneNumber(MobileNumberTextBox.Text), HiddenSecurityCodeTextBox.Text.Trim(), EnterpriseEmailTextBox.Text, HiddenEmailSecurityCodeTextBox.Text.Trim(), AccessToken, RefreshToken);
                    //1. If profile doesn't exist, send the local profile(if any) to create a new profile in server.
                    //2. If security code matches. Else, throw error 

                    if (profile != null && ResultInterpreter.IsError(profile.DataInfo))
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.ProfileCreationFailText, "basicWrap", "Error!"));
                        m_ProgressBar.Visibility = Visibility.Collapsed;
                        return;
                    }
                    else if (profile != null && ResultInterpreter.IsAuthError(profile.DataInfo))
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.IncorrectSecurityCode, "basicWrap", "Error!"));
                        m_ProgressBar.Visibility = Visibility.Collapsed;
                        return;
                    }

                    App.CurrentUser.SyncFullProfileServer2Local(profile);

                    NavigationService.Navigate(new Uri("/Pages/Settings.xaml?ToPage=Buddies&FromPage=Register", UriKind.Relative));
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.ProfileCreationSuccessText, "basicWrap", "Info!"));
                }
                m_ProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                m_ProgressBar.Visibility = Visibility.Collapsed;
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnableToConnectService, "basicWrap", "Info!"));
                return;
            }


        }

        private bool ValidateInputs()
        {

            if (NameTextBox.Text == String.Empty)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidUserNameWhileRegistering, "basicWrap", "Oops!"));
                NameTextBox.Focus();
                return false;
            }
            if (MobileNumberTextBox.Text == String.Empty)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidMobileNumberWhileRegistering, "basicWrap", "Oops!"));
                MobileNumberTextBox.Focus();
                return false;
            }
            if (Config.IsEnterpriseBuild)
            {
                if (string.IsNullOrWhiteSpace(HiddenEmailSecurityCodeTextBox.Text) || HiddenEmailSecurityCodeTextBox.Text.Length > 5)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidEmailSecurityCodeWhileRegistering, "basicWrap", "Oops!"));
                    EmailSecurityCodeTextBox.Focus();
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(HiddenSecurityCodeTextBox.Text) || HiddenSecurityCodeTextBox.Text.Length > 5)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.InvalidSecurityCodeWhileRegistering, "basicWrap", "Oops!"));
                SecurityCodeTextBox.Focus();
                return false;
            }
            //TODO: If there is alerady profile exisits, return error message "Profile Already exisits"
            return true;
        }

        string _enteredPasscode = "";
        string _passwordChar = "*";


        private void SecurityKeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //modify new passcode according to entered key
            _enteredPasscode = GetNewPasscode(_enteredPasscode, e);
            HiddenSecurityCodeTextBox.Text = _enteredPasscode;
            // ActualPasscode.Text = _enteredPasscode;
            //replace text by *
            SecurityCodeTextBox.Text = Regex.Replace(_enteredPasscode, @".", _passwordChar);

            //take cursor to end of string
            SecurityCodeTextBox.SelectionStart = SecurityCodeTextBox.Text.Length;

        }

        string _enteredEmailPasscode = "";
        private void EmailSecurityKeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //modify new passcode according to entered key
            _enteredEmailPasscode = GetNewPasscode(_enteredEmailPasscode, e);
            HiddenEmailSecurityCodeTextBox.Text = _enteredEmailPasscode;
            //replace text by *
            EmailSecurityCodeTextBox.Text = Regex.Replace(_enteredEmailPasscode, @".", _passwordChar);

            //take cursor to end of string
            EmailSecurityCodeTextBox.SelectionStart = EmailSecurityCodeTextBox.Text.Length;
        }
        private string GetNewPasscode(string oldPasscode, KeyEventArgs keyEventArgs)
        {
            string newPasscode = string.Empty;
            switch (keyEventArgs.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                    newPasscode = oldPasscode + (keyEventArgs.PlatformKeyCode - 48);
                    break;
                case Key.Back:
                    if (oldPasscode.Length > 0)
                        newPasscode = oldPasscode.Substring(0, oldPasscode.Length - 1);
                    break;
                default:
                    //others
                    newPasscode = oldPasscode;
                    break;
            }
            return newPasscode;
        }


        private void RevealPWChars_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SecurityCodeTextBox.Visibility = Visibility.Collapsed;
            HiddenSecurityCodeTextBox.Visibility = Visibility.Visible;
        }
        private void RevealPWChars_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SecurityCodeTextBox.Visibility = Visibility.Collapsed;
            HiddenSecurityCodeTextBox.Visibility = Visibility.Visible;
        }
        private void RevealPWChars_MouseLeave(object sender, MouseEventArgs e)
        {
            SecurityCodeTextBox.Visibility = Visibility.Visible;
            HiddenSecurityCodeTextBox.Visibility = Visibility.Collapsed;
        }

        private void RevealEmailPWChars_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailSecurityCodeTextBox.Visibility = Visibility.Collapsed;
            HiddenEmailSecurityCodeTextBox.Visibility = Visibility.Visible;
        }
        private void RevealEmailPWChars_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailSecurityCodeTextBox.Visibility = Visibility.Collapsed;
            HiddenEmailSecurityCodeTextBox.Visibility = Visibility.Visible;
        }
        private void RevealEmailPWChars_MouseLeave(object sender, MouseEventArgs e)
        {
            EmailSecurityCodeTextBox.Visibility = Visibility.Visible;
            HiddenEmailSecurityCodeTextBox.Visibility = Visibility.Collapsed;
        }

        private void CountryCodeListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var countryDetails = (CountryCodes)CountryCodeListPicker.SelectedItem;
            if (countryDetails != null)
            {
                App.SosViewModel.Helplines = null;
                TokenInfoTextBlock.Text = (Config.IsEnterpriseBuild) ? CustomMessage.EnterpriseSecurityTokenSMSInfoText : CustomMessage.SecurityTokenSMSInfoText;
            }
        }

        private void SetControls()
        {
            try
            {
                RenderUI();
                SetDigitInputScope(MobileNumberTextBox);
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }
        }

        private void RenderUI()
        {
            try
            {
                if (Globals.IsRegisteredUser)
                {
                    //LiveLoginButton.Visibility = Visibility.Collapsed;
                    LiveAuthTokenPanel.Visibility = Visibility.Visible;

                    NameTextBox.Text = Globals.User.Name.GetValue();
                    LiveIdTextBox.Text = Globals.User.LiveEmail.GetValue();
                }
                if (CountryCodeListPicker.DataContext == null)
                    CountryCodeListPicker.DataContext = Globals.AllCountryCodes;

            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        private void SetDigitInputScope(TextBox textBoxControl)
        {
            try
            {
                InputScopeNameValue digitalInputNameValue = InputScopeNameValue.TelephoneNumber;
                textBoxControl.InputScope = new InputScope()
                {
                    Names = { new InputScopeName() { NameValue = digitalInputNameValue } }
                };
            }
            catch (Exception exception)
            {
                ExceptionHandler.ShowExceptionMessageAndLog(exception, true);
            }

        }

        #endregion
    }
}
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using Newtonsoft.Json;
using SOS.Phone;
using SOS.Phone.MembershipServiceRef;
using SOS.Phone.MVVM.Model;
using SOS.Phone.ServiceWrapper;
using SOS.Phone.Utilites.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Storage;
using Group = SOS.Phone.GroupServiceRef.Group;
using GroupServiceReference = SOS.Phone.GroupServiceRef;

namespace SOS.Phone
{
    public partial class Settings : PhoneApplicationPage
    {
        bool hideBuddyPopup = false; //flag to close buddypopup based on buddydetails popup as there is some problem in xaml, on closing one close button, underneth close button is also firing
        bool hideGroupDetailsPopup = false; //flag to close Group Data entry pop up as there is some problem in xaml, on closing one close button, underneth close button is also firing
        public Settings()
        {
            InitializeComponent();

            PopulateUIControls();
        }

        #region Navigation Event Handlers
        private ObservableCollection<ContactsModel> ContactModel;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                //BottomPanel.DataContext = Globals.CurrentProfile;
                stcPanelBtnAsync.DataContext = Globals.CurrentProfile;
                stcpnlLastSynced.DataContext = Globals.CurrentProfile;
                StcPnlPreferences.DataContext = Globals.CurrentProfile;
                PhoneNumberList.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 1000);
                EmailIdList.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 1000);

                string ToPage = "Unknown";
                if (NavigationContext.QueryString.TryGetValue("ToPage", out ToPage))
                {
                    switch (ToPage)
                    {
                        case "Buddies":
                            SettingsPivot.SelectedIndex = 1;
                            break;
                        case "Groups":
                            SettingsPivot.SelectedIndex = 2;
                            break;
                        case "Preferences":
                            SettingsPivot.SelectedIndex = 3;
                            break;
                        default:
                            SettingsPivot.SelectedIndex = 0;
                            break;
                    }
                }

                string FromPage = "Unknown";
                if (NavigationContext.QueryString.TryGetValue("FromPage", out FromPage))
                {
                    switch (FromPage)
                    {
                        //If FromPage is Register, remove back entry
                        case "Register":
                            if (NavigationService.BackStack != null && NavigationService.BackStack.Count() > 1)
                            {
                                NavigationService.RemoveBackEntry();
                                NavigationService.RemoveBackEntry();
                            }
                            submitEmergencyNumbersChanges();
                            break;
                        case "SOS":
                            string index = "0";
                            if (NavigationContext.QueryString.TryGetValue("index", out index))
                            {
                                SettingsPivot.SelectedIndex = int.Parse(index);
                            }
                            break;
                        default:

                            break;
                    }
                }

                Dispatcher.BeginInvoke(async () =>
                    {
                        RenderUI();
                        EnableEventHandlers();

                        //HealthUpdate code will be getting changed from service side. So commenting for now.
                        //if (FromPage == "MainPage")
                        //{
                        //    HealthUpdate healthUpdate = await Globals.GetSystemHealthAsync();
                        //    if (healthUpdate != null)
                        //    {
                        //        if (!healthUpdate.IsProfileActive)
                        //            
                        //  MessageBox.Show("Please update your contact information. This contact number has been invalidated.");
                        //        else if (healthUpdate.IsGroupModified)
                        //        {
                        //            SyncWithServer_Click(new object(), new RoutedEventArgs());
                        //        }
                        //    }
                        //}
                    });

            }
            catch (Exception ex)
            {
                //Consume Exception
            }
            finally
            {

            }
        }

        #endregion Navigation Event Handlers

        private void RenderUI()
        {
            LocationServiceButton.Source = new BitmapImage(new Uri(Globals.IsPhoneLocationServiceEnabled ? "/Assets/images/LocOnImage.png" : "/Assets/images/LocOffImage.png", UriKind.Relative));
            LocationServiceTextBlock.Text = Globals.IsPhoneLocationServiceEnabled ? "On" : "Off";

            SetDigitInputScope(MobileNumberTextBox);

            if (!Globals.IsRegisteredUser)// Not Registered user
            {
                ProfileRegisterPanel.Visibility = Visibility.Visible;
                GroupRegisterPanel.Visibility = Visibility.Visible;
                PostLocationConsentSwitch.Visibility = Visibility.Collapsed;
                TrackingConsentLabel.Visibility = Visibility.Collapsed;
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "Register";
            }
            else
            {
                ProfilePanel.Visibility = Visibility.Visible;
                PostLocationConsentSwitch.Visibility = Visibility.Visible;
                GroupPanel.Visibility = Visibility.Visible;
                TrackingConsentLabel.Visibility = Visibility.Visible;
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "UnRegister";
            }
            RenderEmergencyNumbers();
        }

        private void SetDigitInputScope(TextBox textBoxControl)
        {
            InputScopeNameValue digitalInputNameValue = InputScopeNameValue.TelephoneNumber;
            textBoxControl.InputScope = new InputScope()
            {
                Names = { new InputScopeName() { NameValue = digitalInputNameValue } }
            };
        }


        #region Enums
        private enum PivotItemIndex
        {
            Profile = 0,
            Buddies = 1,
            Groups = 2,
            Preferences = 3
        }
        #endregion Enums

        #region Private Members
        PhoneNumberChooserTask numberTask;
        private List<string> buddyContactUniqueKeys = new List<string>();
        #endregion Private Members

        #region LoadUserProfile

        private void DisableEventHandlers()
        {
            MobileNumberListPicker.SelectionChanged -= MobileNumberListPicker_SelectionChanged;
            DefaultCallerListPicker.SelectionChanged -= DefaultCallerListPicker_SelectionChanged;
            FBGroupListPicker.SelectionChanged -= FBGroupListPicker_SelectionChanged;
            LocationConsentSwitch.Checked -= LocationConsentSwitch_Checked;
            LocationConsentSwitch.Unchecked -= LocationConsentSwitch_Unchecked;
            PostLocationConsentSwitch.Checked -= PostLocationConsentSwitch_Checked;
            PostLocationConsentSwitch.Unchecked -= PostLocationConsentSwitch_Unchecked;
            SosTypeSMS.Checked -= SosTypeSMS_Checked;
            SosTypeSMS.Unchecked -= SosTypeSMS_Unchecked;

            SosTypeEmail.Checked -= SosTypeEmail_Checked;
            SosTypeEmail.Unchecked -= SosTypeEmail_Unchecked;

            SosFB.Checked -= sosFB_Checked;
            SosFB.Unchecked -= SosFB_Unchecked;

            NameTextBox.MouseLeave -= NameTextBox_MouseLeave;

            MessageTemplatePref.MouseLeave -= MessageTemplatePref_MouseLeave;
        }

        private void EnableEventHandlers()
        {
            MobileNumberListPicker.SelectionChanged += MobileNumberListPicker_SelectionChanged;
            DefaultCallerListPicker.SelectionChanged += DefaultCallerListPicker_SelectionChanged;
            FBGroupListPicker.SelectionChanged += FBGroupListPicker_SelectionChanged;

            LocationConsentSwitch.Checked += LocationConsentSwitch_Checked;
            LocationConsentSwitch.Unchecked += LocationConsentSwitch_Unchecked;
            PostLocationConsentSwitch.Checked += PostLocationConsentSwitch_Checked;
            PostLocationConsentSwitch.Unchecked += PostLocationConsentSwitch_Unchecked;

            SosTypeSMS.Checked += SosTypeSMS_Checked;
            SosTypeSMS.Unchecked += SosTypeSMS_Unchecked;

            SosTypeEmail.Checked += SosTypeEmail_Checked;
            SosTypeEmail.Unchecked += SosTypeEmail_Unchecked;

            SosFB.Checked += sosFB_Checked;
            SosFB.Unchecked += SosFB_Unchecked;

            NameTextBox.MouseLeave += NameTextBox_MouseLeave;

            MessageTemplatePref.MouseLeave += MessageTemplatePref_MouseLeave;
        }

        //Reason for not using MVVM to bind controls is, Local DB has to be synced/ updated along with VM objects 
        private void PopulateUIControls()
        {
            try
            {
                DisableEventHandlers();

                UserTableEntity user = Globals.User;
                ProfileTableEntity profile = Globals.CurrentProfile;

                if (Globals.IsRegisteredUser)
                {
                    NameTextBox.Text = user.Name.GetValue();
                    LiveIdTextBlock.Text = user.LiveEmail.GetValue();
                    MobileNumberListPicker.Items.Add(profile);
                    MobileNumberTextBox.Text = profile.MobileNumber.GetValue();
                    //LastSyncedProfileTextBox.Text = profile.LastSynced.HasValue ? profile.LastSynced.Value.ToString() : string.Empty;
                }

                if (Globals.User.FBAuthId.GetValue() != String.Empty)
                {
                    FBAuthTokenPanel.Visibility = System.Windows.Visibility.Visible;
                    FBLoginButton.Visibility = Visibility.Collapsed;
                    //FBLogoutButton.Visibility = Visibility.Visible;
                    LoadFBGroups(Globals.User.FBAuthId.GetValue());

                    FBGroup selectedGroup = new FBGroup() { id = profile.FBGroupId, name = profile.FBGroupName };

                    if (FBGroupListPicker.Items.Count > 1)//if groups loaded
                        FBGroupListPicker.SelectedItem = selectedGroup;
                    else
                        FBGroupListPicker.Items.Add(selectedGroup);
                }
                //else
                //{
                //    FBLogoutButton.Visibility = Visibility.Collapsed;
                //}

                BuddiesListBox.Items.Clear();
                DefaultCallerListPicker.Items.Clear();
                GroupsListBox.Items.Clear();

                foreach (var buddy in App.MyBuddies.Buddies)
                {
                    BuddiesListBox.Items.Add(buddy);
                    if (!string.IsNullOrEmpty(buddy.PhoneNumber))
                    {
                        DefaultCallerListPicker.Items.Add(buddy);
                        if (buddy.IsPrimeBuddy)
                            DefaultCallerListPicker.SelectedItem = buddy;
                    }
                }
                if (DefaultCallerListPicker.SelectedItem != null)
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SetPrimeBuddy, ((BuddyTableEntity)DefaultCallerListPicker.SelectedItem).PhoneNumber, true);

                foreach (var item in App.MyGroups.Groups)
                {
                    GroupsListBox.Items.Add(item);
                }
                SosTypeSMS.IsChecked = profile.CanSMS;
                SosTypeEmail.IsChecked = profile.CanEmail;
                SosFB.IsChecked = profile.CanFBPost;

                MessageTemplatePref.Text = Constants.MessageTemplateText;
                if (profile.MessageTemplate.GetValue() != String.Empty)
                    MessageTemplatePref.Text = profile.MessageTemplate.GetValue();

                LocationConsentSwitch.IsChecked = profile.LocationConsent;
                PostLocationConsentSwitch.IsChecked = profile.PostLocationConsent;
                if (Globals.IsRegisteredUser && Globals.IsDataNetworkAvailable)
                    App.MyProfiles.CheckPendingUpdatesFromServer();

                CountryCodeListPicker.DataContext = Globals.AllCountryCodes;

                if (Globals.IsRegisteredUser)
                {
                    string countryCode = Constants.CountryCode;

                    CountryCodes toSetIsdCodeToTextBox = CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == countryCode)) as CountryCodes;
                    CountryCodeTextBlock.Text = toSetIsdCodeToTextBox.CountryName + " (" + toSetIsdCodeToTextBox.IsdCode + ")";
                    CountryCodeListPicker.Visibility = Visibility.Collapsed;
                    CountryCodeTextBlock.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                //   MessageBox.Show(CustomMessage.RegisteredUserLoadingFailText);
            }
            finally
            {
                //EnableEventHandlers();
            }
        }


        #endregion LoadUserProfile

        #region Click Event Handlers

        private void btnAddBuddy_Click(object sender, RoutedEventArgs e)
        {
            ShowNumberChooser();
        }

        private void joinGroupButton_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                groupsPivotItem.Opacity = 0.3;
                txtFilterGroupText.Text = String.Empty;
                groupsSearchResult.DataContext = null;
                JoinGroupPopup.IsOpen = true;
                hideGroupDetailsPopup = false;
                txtFilterGroupText.Focus();
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }

        private void addBuddyButton_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                if (App.MyBuddies.Buddies.Count >= Constants.MaxNumberOfBuddies)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.Max5BuddiesValidationText, "basicWrap", "Oops!"));
                    return;
                }
                buddiesPivotItem.Opacity = 0.3;
                txtFilterText.Text = String.Empty;
                buddiesSearchResult.DataContext = null;
                AddBuddyPopup.IsOpen = true;
                hideBuddyPopup = false;
                txtFilterText.Focus();
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buddiesSearchResult.DataContext = null;

                Contacts cons = new Contacts();
                cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
                m_ProgressBar.Visibility = Visibility.Visible;
                cons.SearchAsync(txtFilterText.Text, FilterKind.DisplayName, "Buddies List Get");
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }


        private async void GroupSearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_ProgressBar.Visibility = Visibility.Visible;
                groupsSearchResult.DataContext = null;

                if (Globals.IsDataNetworkAvailable && Globals.IsRegisteredUser)
                {
                    string searchText = txtFilterGroupText.Text.Trim();
                    SOS.Phone.GroupServiceRef.GroupList lstGrpItems = await MembershipServiceWrapper.GetListOfGroups(searchText);
                    groupsSearchResult.DataContext = lstGrpItems.List;
                }
                m_ProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                m_ProgressBar.Visibility = Visibility.Collapsed;
                //Consume Exception
            }

            return;
        }

        private void FB_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FBLoginPopup.IsOpen = true;
                fbLoginBrowser.Navigate(Utility.GetLoginUri());
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }

        private void LocationServiceButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        }

        private string GetEmailId(IEnumerable<ContactEmailAddress> emails)
        {
            foreach (ContactEmailAddress e in emails)
            {
                if (e.EmailAddress.Trim() != string.Empty)
                    return e.EmailAddress;
            }
            return string.Empty;
        }

        private string GetPhoneNumber(IEnumerable<ContactPhoneNumber> numbers)
        {
            foreach (ContactPhoneNumber p in numbers)
            {
                if (p.PhoneNumber.Trim() != string.Empty)
                    return Utility.GetPlainPhoneNumber(p.PhoneNumber);
            }
            return string.Empty;
        }

        private string GetIsdCode(IEnumerable<ContactPhoneNumber> numbers)
        {
            foreach (ContactPhoneNumber p in numbers)
            {
                if (p.PhoneNumber.Trim() != string.Empty)
                    return Utility.GetPlainIsdCode(p.PhoneNumber);
            }
            return string.Empty;
        }

        private BuddyTableEntity GetBuddyFromContact(ContactsModel c)
        {
            BuddyTableEntity b = new BuddyTableEntity();
            b.Name = c.FullName.Trim();
            //b.Name = string.IsNullOrEmpty(Name) ? c.DisplayName : Name;
            b.Email = c.EmailAddress;
            b.PhoneNumber = (!string.IsNullOrEmpty(c.PhoneNumber) ? Constants.CountryCode : "") + c.PhoneNumber;
            b.State = BuddyState.Active;
            //b.BuddyImage = c.ImageStream;
            return b;
        }

        private void AddBuddyImage_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                if (App.MyBuddies.Buddies.Count >= Constants.MaxNumberOfBuddies)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.Max5BuddiesValidationText, "basicWrap", "Oops!"));
                    return;
                }
                ContactsModel selectedContact = ((System.Windows.FrameworkElement)(sender)).DataContext as ContactsModel;
                if (string.IsNullOrEmpty(selectedContact.PhoneNumber.Trim()))
                    SocialSiteContactsWarning.Visibility = Visibility.Visible;
                else
                    SocialSiteContactsWarning.Visibility = Visibility.Collapsed;

                if (selectedContact.IsdCode == string.Empty || selectedContact.IsdCode == Constants.CountryCode)
                {
                    PhoneNumberList.Visibility = Visibility.Visible;
                    EmailIdList.Visibility = Visibility.Visible;
                    AddBuddyDetailsPopup.DataContext = selectedContact;
                    AddBuddyDetailsPopup.IsOpen = true;
                    ContactNumber.MaxLength = Constants.MaxPhonedigits;

                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (string.IsNullOrEmpty(selectedContact.PhoneNumber))
                            Globals.DisplayToast(CustomMessage.AddingBuddyEmptyNumberText, "basicWrap", "Add buddy failed!");
                        else if (Globals.IsRegisteredUser)
                            Globals.DisplayToast(CustomMessage.AddingBuddyFromDifferentRegionInRegmode, "basicWrap", "Add buddy failed!");
                        else
                            Globals.DisplayToast(CustomMessage.AddingBuddyFromDifferentRegionInUnRegmode, "basicWrap", "Add buddy failed!");
                    });
                    return;
                }
                if (PhoneNumberList.Items.Count < 2) PhoneNumberList.Visibility = Visibility.Collapsed;
                if (EmailIdList.Items.Count < 2) EmailIdList.Visibility = Visibility.Collapsed;
                ChangeCountryCodeBtn.Visibility = !Globals.IsRegisteredUser ? Visibility.Visible : Visibility.Collapsed;
                CountryCodeTextBox.Text = Constants.CountryCode;
            }
            catch (Exception ex)
            {
                //Consume Exception
            }

        }

        private async void RemoveBuddyImage_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                BuddyTableEntity b = ((System.Windows.FrameworkElement)(sender)).DataContext as BuddyTableEntity;
                App.MyBuddies.DeleteBuddy(b);
                if (App.MyBuddies.IsSuccess)
                {

                    BuddiesListBox.Items.Remove(b);
                    await DeleteFromLocalFolder(b.Name);
                    DefaultCallerListPicker.Items.Remove(b);
                    if (Globals.IsRegisteredUser)
                        App.MyProfiles.UpdateIsDataSynced(false);
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast("Unable to remove Buddy!" + Constants.ReachSupportMessageText, "basicWrap", "Oops!"));
                }
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }
        private void JoinGroup(string strEnrollmentValue, Group group)
        {
            GroupTableEntity grpEntity = App.MyGroups.ConvertGroup(group);
            grpEntity.EnrollmentValue = strEnrollmentValue;
            grpEntity.MyProfileId = Globals.CurrentProfile.ProfileId;

            App.MyGroups.AddGroup(grpEntity);

            if (App.MyGroups.IsSuccess)
            {
                GroupsListBox.Items.Add(grpEntity);
                App.MyProfiles.UpdateIsDataSynced(false);
                if (JoinGroupDetailsPopup.IsOpen)
                {
                    JoinGroupDetailsPopup.IsOpen = false;
                }
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast("Unable to add Group!" + Constants.ReachSupportMessageText, "basicWrap", "Oops!"));
            }
        }
        private void JoinGroupImage_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Group group = ((System.Windows.FrameworkElement)(sender)).DataContext as Group;
                if (!GroupAlreadyExists(group.GroupID))
                {
                    switch (group.EnrollmentType)
                    {
                        case GroupServiceReference.Enrollment.None:
                            JoinGroup(string.Empty, group);
                            break;
                        case GroupServiceReference.Enrollment.AutoOrgMail:
                            JoinGroupDetailsPopup.IsOpen = true;
                            txtJoinGroupText.Text = string.Empty;
                            JoinGroupDetailsPopup.DataContext = group;
                            lblJoinGroupError.Visibility = Visibility.Collapsed;
                            lblJoinGroup.Text = Constants.GroupAutoOrgMailMessage;
                            break;
                        case GroupServiceReference.Enrollment.Moderator:
                            JoinGroupDetailsPopup.IsOpen = true;
                            txtJoinGroupText.Text = string.Empty;
                            JoinGroupDetailsPopup.DataContext = group;
                            lblJoinGroupError.Visibility = Visibility.Collapsed;
                            lblJoinGroup.Text = Constants.GroupModeratorMessage;
                            break;
                        default:
                            break;
                    };
                }
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }
        private void Details_JoinGroupClick(object sender, RoutedEventArgs e)
        {
            if (txtJoinGroupText.Text != string.Empty)
            {
                Group group = ((System.Windows.FrameworkElement)(sender)).DataContext as Group;
                if (group.EnrollmentType == GroupServiceReference.Enrollment.AutoOrgMail)
                {
                    string email = txtJoinGroupText.Text;
                    int lastIndex = email.LastIndexOf('@');
                    if (lastIndex != -1)
                    {
                        string pattern = @"^[a-zA-Z0-9._-]*$";
                        string name = email.Substring(0, lastIndex);
                        string domain = email.Substring(lastIndex);
                        if (name.Length >= 2 && Regex.IsMatch(name, pattern) && group.EnrollmentKey.ToUpper() == domain.ToUpper())
                        {
                            lblJoinGroupError.Visibility = Visibility.Collapsed;
                            JoinGroup(txtJoinGroupText.Text, group);
                        }
                        else
                        {
                            if (name.Length < 2)
                                lblJoinGroupError.Text = Constants.GroupAutoOrgMaillengthErrorMessage;
                            else
                                lblJoinGroupError.Text = Constants.GroupAutoOrgMailErrorMessage;

                            lblJoinGroupError.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (group.EnrollmentKey == email)// If the joining group is via Group Key
                        {
                            lblJoinGroupError.Visibility = Visibility.Collapsed;
                            JoinGroup(txtJoinGroupText.Text, group);
                        }
                        else
                        {
                            lblJoinGroupError.Text = Constants.GroupAutoOrgMailErrorMessage;
                            lblJoinGroupError.Visibility = Visibility.Visible;
                        }
                    }
                }
                else if (group.EnrollmentType == GroupServiceReference.Enrollment.Moderator)
                {
                    JoinGroup(txtJoinGroupText.Text, group);
                }
            }
            else
            {
                lblJoinGroupError.Text = Constants.GroupErrorMessage;
                lblJoinGroupError.Visibility = Visibility.Visible;
            }

        }

        private void RemoveGroupImage_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                GroupTableEntity selectedBG = ((System.Windows.FrameworkElement)(sender)).DataContext as GroupTableEntity;
                App.MyGroups.DeleteGroup(selectedBG.GroupId);
                if (App.MyGroups.IsSuccess)
                {
                    GroupsListBox.Items.Remove(selectedBG);
                    App.MyProfiles.UpdateIsDataSynced(false);
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast("Unable to remove Group!" + Constants.ReachSupportMessageText, "basicWrap", "Oops!"));
                }
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }


        #endregion Click Event Handlers

        #region Completed Event Handlers

        private void numberTask_Completed(object sender, PhoneNumberResult e)
        {
            try
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = e.DisplayName + " : " + e.PhoneNumber;
                    BuddiesListBox.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }

        private void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            try
            {
                string num = Constants.CountryCode;
                //Bind the results to the list box that displays them in the UI
                ContactModel = new ObservableCollection<ContactsModel>();
                foreach (var temp in e.Results)
                {
                    ContactsModel objContact = new ContactsModel();
                    objContact.DisplayName = temp.DisplayName ?? string.Empty;
                    objContact.FullName = (temp.CompleteName != null ? (temp.CompleteName.FirstName + " " + (!string.IsNullOrEmpty(temp.CompleteName.MiddleName) ? temp.CompleteName.MiddleName + " " : string.Empty) + temp.CompleteName.LastName) : (string.IsNullOrEmpty(temp.DisplayName) ? string.Empty : temp.DisplayName)).Trim();
                    //objContact.LastName = temp.CompleteName != null ? (temp.CompleteName.LastName != null ? temp.CompleteName.LastName : string.Empty) : string.Empty; ;
                    objContact.EmailAddress = GetEmailId(temp.EmailAddresses);
                    objContact.PhoneNumber = GetPhoneNumber(temp.PhoneNumbers);
                    objContact.IsdCode = GetIsdCode(temp.PhoneNumbers);
                    var IsdCodesList = temp.PhoneNumbers;
                    IEnumerable<ContactPhoneNumber> list = IsdCodesList as IEnumerable<ContactPhoneNumber>;
                    var IsdCodesListFiltered = GetIsdCodes(list);

                    objContact.EmailAddresses = new List<string>();
                    objContact.PhoneNumbers = new List<string>();
                    foreach (var t in temp.EmailAddresses)
                    {
                        objContact.EmailAddresses.Add(t.EmailAddress);
                    }

                    for (int i = 0; i < temp.PhoneNumbers.Count(); i++)
                    {
                        if (IsdCodesListFiltered[i] == Constants.CountryCode)
                        {
                            objContact.PhoneNumbers.Add(Utility.GetPlainPhoneNumber(temp.PhoneNumbers.ElementAt(i).ToString()));
                        }
                    }
                    objContact.ImageStream = temp.GetPicture();
                    ContactModel.Add(objContact);
                }
                buddiesSearchResult.DataContext = ContactModel;
                if (buddiesSearchResult.Items.Count == 0)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.BuddySearchFailText, "basicWrap", "Oops!"));
                }
                m_ProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (System.Exception)
            {
                m_ProgressBar.Visibility = Visibility.Collapsed;
                //That's okay, no results
            }
        }

        private List<string> GetIsdCodes(IEnumerable<ContactPhoneNumber> numbers)
        {
            List<string> listOfIsdCodes = new List<string>();
            foreach (ContactPhoneNumber p in numbers)
            {
                if (p.PhoneNumber.Trim() != string.Empty)
                    listOfIsdCodes.Add(Utility.GetPlainIsdCode(p.PhoneNumber));
            }
            if (listOfIsdCodes.Count() != 0)
            {
                return listOfIsdCodes;
            }
            else
                return null;
        }

        #endregion Completed Event Handlers

        #region Selection Changed Event Handlers
        private void SettingsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddBuddyPopup.IsOpen = false;
            AddBuddyDetailsPopup.IsOpen = false;
            JoinGroupPopup.IsOpen = false;
            JoinGroupDetailsPopup.IsOpen = false;
        }


        #endregion Selection Changed Event Handlers

        #region Save User Profile

        #endregion Save User Profile

        #region Tap Event Handlers
        private void CloseBuddy_Click(object sender, RoutedEventArgs e)
        {
            if (AddBuddyPopup.IsOpen && !hideBuddyPopup)
            {
                AddBuddyPopup.IsOpen = false;
                buddiesPivotItem.Opacity = 1.0;
            }
            hideBuddyPopup = false;
        }
        private void CloseGroup_Click(object sender, RoutedEventArgs e)
        {
            if (JoinGroupPopup.IsOpen && !hideGroupDetailsPopup)
            {
                JoinGroupPopup.IsOpen = false;
                buddiesPivotItem.Opacity = 1.0;
            }
            hideGroupDetailsPopup = false;
        }

        #endregion Tap Event Handlers

        #region Networking

        private void fbLoginBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string strLoweredAddress = e.Uri.OriginalString.ToLower();
            //if (!strLoweredAddress.Contains("error"))
            if (strLoweredAddress.StartsWith("http://www.facebook.com/connect/login_success.html?code="))
            {
                fbLoginBrowser.Navigate(Utility.GetTokenLoadUri(e.Uri.OriginalString.Substring(56)));
                return;
            }
            if (strLoweredAddress.StartsWith("https://m.facebook.com/connect/login_success.html?code="))
            {
                fbLoginBrowser.Navigate(Utility.GetTokenLoadUri(e.Uri.OriginalString.Substring(55)));
                return;
            }
            string fbAuthTokenString = fbLoginBrowser.SaveToString();
            if (fbAuthTokenString.Contains("http:\\/\\/www.facebook.com\\/connect\\/login_success.html?code="))
            {
                int start = fbAuthTokenString.IndexOf("login_success.html?code=");
                int end = fbAuthTokenString.IndexOf("\";</script>");
                fbLoginBrowser.Navigate(Utility.GetTokenLoadUri(fbAuthTokenString.Substring(start, end - start).Substring(24)));
            }
            if (fbAuthTokenString.Contains("access_token="))
            {
                int nPos = fbAuthTokenString.IndexOf("access_token");
                string strAuthPart = fbAuthTokenString.Substring(nPos + 13);
                nPos = strAuthPart.IndexOf("</pre>");
                strAuthPart = strAuthPart.Substring(0, nPos);
                //REMOVE EXPIRES IF FOUND!
                nPos = strAuthPart.IndexOf("&amp;expires=");
                if (nPos != -1)
                {
                    strAuthPart = strAuthPart.Substring(0, nPos);
                }
                App.CurrentUser.SaveUserSetting(UserViewModel.UserSetting.FBAuthID, strAuthPart);
                FBAuthTokenPanel.Visibility = System.Windows.Visibility.Visible;
                FBLoginPopup.IsOpen = false;
                //automaticall leave the page after login success
                //NavigationService.GoBack();

                FBLoginButton.Visibility = Visibility.Collapsed;
                //FBLogoutButton.Visibility = Visibility.Visible;
                LoadFBGroups(strAuthPart);
                return;
            }
        }

        #endregion Networking



        private void AddProfilePanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Register.xaml?Scenario=Register", UriKind.Relative));
        }


        #region Miscellaneous Helpers

        private void ShowNumberChooser()
        {
            numberTask = new PhoneNumberChooserTask();
            numberTask.Completed += numberTask_Completed;
            numberTask.Show();
        }

        private bool GroupAlreadyExists(string groupId)
        {
            bool retval = false;
            foreach (GroupTableEntity item in GroupsListBox.Items)
            {
                if (item.GroupId == groupId)
                {
                    retval = true;
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.GroupExistsAlready, "basicWrap", "Info!"));
                    break;
                }
            }
            return retval;
        }
        private bool BuddyAlreadyExists(string phoneNumbers, string EmailId)
        {
            bool retval = false;
            string phoneNumber = string.Empty;
            //foreach (ContactPhoneNumber phoneNumberItem in phoneNumbers)
            //{
            //    phoneNumber = Utility.GetUnformattedPhoneNumber(phoneNumberItem.PhoneNumber);
            //    break;
            //}
            phoneNumber = Utility.GetPlainPhoneNumber(phoneNumbers);
            if (phoneNumber == string.Empty)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.ContactNumberIsMustForBuddy, "basicWrap", "Oops!"));
                return true; //Do not add empty phone number buddy
            }

            foreach (BuddyTableEntity item in BuddiesListBox.Items)
            {
                if (phoneNumber == Utility.GetPlainPhoneNumber(item.PhoneNumber))
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.BuddyNumberExistsText, "basicWrap", "Oops!"));
                    return true;
                }
                if (!string.IsNullOrEmpty(EmailId) && !string.IsNullOrEmpty(item.Email) && EmailId == item.Email)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.BuddyEmailExistsText, "basicWrap", "Oops!"));
                    return true;
                }
            }
            return retval;
        }

        private bool ValidateEmailID(string email)
        {
            string errorText = string.Empty;
            int lastIndex = email.LastIndexOf('@');
            if (lastIndex != -1)
            {
                string pattern = @"^[a-zA-Z0-9._-]*$";
                string name = email.Substring(0, lastIndex);
                string domain = email.Substring(lastIndex);
                if (name.Length >= 2 && Regex.IsMatch(name, pattern))
                {
                    return true;
                }
                else
                {
                    if (name.Length < 2)
                        errorText = Constants.GroupAutoOrgMaillengthErrorMessage;
                    else
                        errorText = Constants.GroupAutoOrgMailErrorMessage.Replace("Org ", "");

                }
            }
            else
            {
                errorText = Constants.GroupAutoOrgMailErrorMessage.Replace("Org ", "");

            }
            if (!string.IsNullOrEmpty(errorText))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(errorText, "basicWrap", "Email Validation Failed"));
                return false;
            }
            return true;
        }

        private async void LoadFBGroups(string fbAccessToken)
        {
            //Call FB service and get the list of FB Groups and populate in List Picker
            try
            {
                if (fbAccessToken != string.Empty)
                {
                    m_ProgressBar_facebook.Visibility = System.Windows.Visibility.Visible;
                    Uri groupUri = Utility.GetLoadGroupsUri(fbAccessToken);

                    WebClient proxy = new WebClient();
                    proxy.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
                    string result = await proxy.DownloadStringTask(groupUri);
                    //string result1 = "{\"data\":[{\"id\":\"615202231839770\",\"name\":\"Guardian\"},{\"id\":\"436340783079523\",\"name\":\"Tech Forum\"},{\"id\":\"353934991338994\",\"name\":\"DiscoTech\"},{\"id\":\"160839310620155\",\"name\":\"Colleagues\"}],\"paging\":{\"next\":\"https:\\/\\/graph.facebook.com\\/769323960\\/Groups?fields=id,name&access_token=CAACEdEose0cBAGIDYpDPB1vUtFi3FzGsN4g7pVZCVWZB19mDj9ZAoUbWFoMZCwR6CH0TnlE1Ui7TScehevGbmMDSsQj9ZCZAU431zT0l2tLldZCm13yWIIPhrHSPquVWUxu3cepmblendg0TgmZAeUMRcqs4BEkbzR4ZD&limit=5000&offset=5000&__after_id=160839310620155\"}}";
                    FBGroupJsonWrapper fbGroups = JsonConvert.DeserializeObject<FBGroupJsonWrapper>(result);
                    foreach (var item in fbGroups.groups.data.ToList())
                    {
                        if (item.owner == null || (item.owner != null ? (item.owner.id != fbGroups.id) : true))
                            fbGroups.groups.data.Remove(item);
                    }
                    FBGroupListPicker.DataContext = fbGroups.groups.data;
                    m_ProgressBar_facebook.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception e)
            {
                m_ProgressBar_facebook.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #endregion Miscellaneous Helpers

        #region Save Settings To Local Storage

        private void MobileNumberListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfileTableEntity profEnt = (ProfileTableEntity)MobileNumberListPicker.SelectedItem;
            if (profEnt != null)
            {
                App.CurrentUser.SaveUserSetting(UserViewModel.UserSetting.CurrentProfileID, profEnt.ProfileId);
            }
        }

        private void EditMobileNumberImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Register.xaml?Scenario=MobileNumberUpdate", UriKind.Relative));
        }

        private void DeleteProfileImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Register.xaml?Scenario=Register", UriKind.Relative));
        }

        private void LoadFBGroupsImage_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.User.FBAuthId.GetValue() != string.Empty)
                LoadFBGroups(Globals.User.FBAuthId.GetValue());
        }

        private void LocationConsentSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.CurrentProfile.LocationConsent = false;
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.LocationServicePref, "false");
        }

        private void LocationConsentSwitch_Checked(object sender, RoutedEventArgs e)
        {
            Globals.CurrentProfile.LocationConsent = true;
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.LocationServicePref, "true");
        }

        private void DefaultCallerListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DefaultCallerListPicker.Items.Count == App.MyBuddies.Buddies.Count)
            {
                BuddyTableEntity selectedBuddy = (BuddyTableEntity)DefaultCallerListPicker.SelectedItem;
                if (selectedBuddy != null)
                {
                    App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.SetPrimeBuddy, selectedBuddy.PhoneNumber);
                }
            }
        }

        private void MessageTemplatePref_MouseLeave(object sender, MouseEventArgs e)
        {
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.MessageTemplatePref, MessageTemplatePref.Text.Trim());
        }

        private void NameTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBox tb1 = sender as TextBox;

            string text1 = tb1.Text.Trim();

            if (text1.Contains('<') || text1.Contains(';'))
            {
                text1 = text1.Replace("<", "");
                text1 = text1.Replace(";", "");
            }

            tb1.Text = text1;

            App.CurrentUser.SaveUserSetting(UserViewModel.UserSetting.Name, tb1.Text);
        }

        private void SosTypeSMS_Unchecked(object sender, RoutedEventArgs e)
        {
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.CanSendSMS, "false");
        }

        private void SosTypeEmail_Unchecked(object sender, RoutedEventArgs e)
        {
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.CanSendEmail, "false");
        }

        private void SosFB_Unchecked(object sender, RoutedEventArgs e)
        {
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.CanSendFBPost, "false");
        }

        private void SosTypeSMS_Checked(object sender, RoutedEventArgs e)
        {
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.CanSendSMS, "true");
        }

        private void SosTypeEmail_Checked(object sender, RoutedEventArgs e)
        {
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.CanSendEmail, "true");
        }

        private void sosFB_Checked(object sender, RoutedEventArgs e)
        {
            //if (FbGroup.Text == String.Empty)
            //{
            //    MessageBox.Show("Please register with facebook and provide a valid group name to post SOS messages!");
            //    FbGroup.Focus();
            //}
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.CanSendFBPost, "true");
        }

        private void FBGroupListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FBGroup item = (FBGroup)FBGroupListPicker.SelectedItem;
            if (item != null && item.id != null && item.name != null)
            {
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.FBGroupId, item.id.Trim());
                App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.FBGroupName, item.name.Trim());
            }
        }


        #endregion Save Settings To Local Storage

        private async void SyncWithServer_Click(object sender, RoutedEventArgs e)
        {
            string result = await SyncData();
            if (result == "PROFILENOTFOUND") NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            else if (result == "SUCCESS") PopulateUIControls();

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncStatusDisplay(result);
            });
        }

        private void SyncStatusDisplay(string result)
        {
            switch (result)
            {
                case "NETWORKUNAVAILABLE":
                    Globals.DisplayToast(CustomMessage.NetworkNotAvailableText, "basicWrap", "Oops!");
                    break;
                case "PROFILENOTFOUND":

                    Globals.DisplayToast(CustomMessage.ProfileNotFound, "basicWrap", "Oops!");
                    break;
                case "INVALIDPROFILE":
                    Globals.DisplayToast(CustomMessage.InvalidProfile, "basicWrap", "Oops!");
                    break;
                case "ERROR":
                    Globals.DisplayToast(CustomMessage.UnableToSyncProfile, "basicWrap", "Oops!");
                    break;
                case "SUCCESS":
                    Globals.DisplayToast(CustomMessage.SettingsSyncSuccessText, "basicWrap", "Success!");
                    break;

            }
        }

        private async Task<string> SyncData()
        {
            string statusMsg = string.Empty;
            try
            {
                if (!Globals.IsDataNetworkAvailable)
                {
                    return "NETWORKUNAVAILABLE";
                }
                this.m_ProgressBar.Visibility = System.Windows.Visibility.Visible;

                statusMsg = await App.CurrentUser.UpdateUserLocal2Server();
                if (statusMsg == "PROFILENOTFOUND")
                {
                    App.CurrentUser.UnregisterLocally();
                }
                this.m_ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception)
            {
                this.m_ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                statusMsg = "ERROR";
            }
            return statusMsg;
        }

        private void ContactNumber_KeyUp(object sender, KeyEventArgs e)
        {
            ContactsModel model = ((System.Windows.FrameworkElement)(sender)).DataContext as ContactsModel;
            model.PhoneNumber = ((TextBox)sender).Text;
        }

        private void PhoneNumberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContactsModel model = ((System.Windows.FrameworkElement)(sender)).DataContext as ContactsModel;
            model.PhoneNumber = ((string)PhoneNumberList.SelectedItem) == null ? string.Empty : (string)PhoneNumberList.SelectedItem;
        }

        private void EmailIdList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContactsModel model = ((System.Windows.FrameworkElement)(sender)).DataContext as ContactsModel;
            model.EmailAddress = ((string)EmailIdList.SelectedItem) == null ? string.Empty : (string)EmailIdList.SelectedItem;
        }

        private void EmailId_KeyUp(object sender, KeyEventArgs e)
        {
            ContactsModel model = ((System.Windows.FrameworkElement)(sender)).DataContext as ContactsModel;
            model.EmailAddress = ((TextBox)sender).Text;
        }

        private void FullName_KeyUp(object sender, KeyEventArgs e)
        {
            ContactsModel model = ((System.Windows.FrameworkElement)(sender)).DataContext as ContactsModel;
            string filteredFullName = ((TextBox)sender).Text;
            if (filteredFullName.Contains(';') || filteredFullName.Contains('<'))
            {
                filteredFullName = filteredFullName.Replace("<", "");
                filteredFullName = filteredFullName.Replace(";", "");
            }

            ((TextBox)sender).Text = filteredFullName;
            model.FullName = filteredFullName;
        }

        private void Details_CloseClisk(object sender, MouseEventArgs e)
        {
            if (AddBuddyDetailsPopup.IsOpen)
            {
                AddBuddyDetailsPopup.IsOpen = false;
                hideBuddyPopup = true;
                //buddiesPivotItem.Opacity = 1.0;
            }
        }

        private async void Add_DetailsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.MyBuddies.Buddies.Count >= Constants.MaxNumberOfBuddies)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.Max5BuddiesValidationText, "basicWrap", "Oops!"));
                    return;
                }
                ContactsModel selectedContact = ((System.Windows.FrameworkElement)(sender)).DataContext as ContactsModel;
                if (string.IsNullOrEmpty(selectedContact.FullName.Trim()))
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.NameEmptyValidationText, "basicWrap", "Oops!"));
                    return;
                }
                if (selectedContact.PhoneNumber.Length > Constants.MaxPhonedigits)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Globals.DisplayToast(CustomMessage.InvalidPhoneNumberText.Replace("10", Constants.MaxPhonedigits.ToString()), "basicWrap", "Oops!");
                    });
                    return;
                }
                if (!string.IsNullOrEmpty(selectedContact.EmailAddress) && !ValidateEmailID(selectedContact.EmailAddress)) //Removed email validation to allow Security Code based validation
                    return;

                if (!BuddyAlreadyExists(selectedContact.PhoneNumber, selectedContact.EmailAddress))
                {

                    BuddyTableEntity b = GetBuddyFromContact(selectedContact);
                    b.MyProfileId = Globals.CurrentProfile.ProfileId;
                    App.MyBuddies.AddBuddy(b);
                    if (App.MyBuddies.IsSuccess)
                    {
                        await SaveToLocalFolderAsync(selectedContact.ImageStream, b.Name);
                        BuddiesListBox.Items.Add(b);
                        if (!string.IsNullOrEmpty(b.PhoneNumber))
                            DefaultCallerListPicker.Items.Add(b);
                        if (DefaultCallerListPicker.Items.Count == 1)
                            DefaultCallerListPicker.SelectedItem = b;

                        if (Globals.IsRegisteredUser)
                            App.MyProfiles.UpdateIsDataSynced(false);

                        // SaveBuddyImage(b.Name, selectedContact.ImageStream);
                    }
                    else
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast("Unable to add Buddy!" + Constants.ReachSupportMessageText, "basicWrap", "Oops!"));
                    }
                    if (AddBuddyDetailsPopup.IsOpen)
                    {
                        AddBuddyDetailsPopup.IsOpen = false;
                        //hideBuddyPopup = true;
                        //buddiesPivotItem.Opacity = 1.0;
                    }
                }
                //buddiesSearchResult.Items.Remove(selectedContact);
            }
            catch (Exception ex)
            {
                //Consume Exception
            }
        }

        private void CloseDetailsGroup_Click(object sender, MouseEventArgs e)
        {
            if (JoinGroupDetailsPopup.IsOpen)
            {
                JoinGroupDetailsPopup.IsOpen = false;
                hideGroupDetailsPopup = true;
            }
        }

        private void PostLocationConsentSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.CurrentProfile.PostLocationConsent = false;
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.PostLocationServicePref, "false");
        }

        private void PostLocationConsentSwitch_Checked(object sender, RoutedEventArgs e)
        {
            Globals.CurrentProfile.PostLocationConsent = true;
            App.MyProfiles.SaveCurrentProfileSetting(ProfileViewModel.ProfileSetting.PostLocationServicePref, "true");
        }

        //private void FB_Logout_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Globals.IsDataNetworkAvailable && Globals.User.FBAuthId.GetValue() != string.Empty)
        //    {
        //        FacebookDeauthorizeApp(Globals.User.FBAuthId);
        //    }
        //}

        //public void FacebookDeauthorizeApp(string access_token)
        //{
        //    WebClient client = new WebClient();
        //    client.UploadStringCompleted += client_UploadStringCompleted;
        //    client.UploadStringAsync(Utility.GetLogoutUri(access_token), "DELETE", string.Empty);
        //    //client.DownloadStringAsync(Utility.GetLogoutUri(access_token));
        //}

        //public void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        //{
        //    Constants.state = "false";
        //    FBAuthTokenPanel.Visibility = System.Windows.Visibility.Collapsed;
        //    FBLoginButton.Visibility = Visibility.Visible;
        //    FBLogoutButton.Visibility = Visibility.Collapsed;
        //    Globals.User.FBAuthId = string.Empty;
        //    //FBGroupListPicker.Items.Clear();
        //}

        private async void UnRegister_Click(object sender, EventArgs e)
        {
            if (((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text == "Register")
            {
                NavigationService.Navigate(new Uri("/Pages/Register.xaml?Scenario=Register", UriKind.Relative));
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(CustomMessage.UnregisterConfirmationText, "Confirmation!", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        m_ProgressBar.Visibility = Visibility.Visible;
                        if (await MembershipServiceWrapper.UnRegisterUser())
                        {
                            App.CurrentUser.UnregisterLocally();

                            BuddiesListBox.Items.Clear();
                            ResetEmergencyNumbers();
                            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
                            Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnregisterSuccessText, "basicWrap", "Info!"));
                        }
                        else
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnregisterFailUnauthorizedText, "basicWrap", "Oops!"));
                        }
                    }
                    catch
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => Globals.DisplayToast(CustomMessage.UnregisterFailText, "basicWrap", "Oops!"));
                        return;
                    }
                    finally
                    {
                        m_ProgressBar.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Globals.CurrentProfile.IsDataSynced)
            {
                e.Cancel = true;
                Dispatcher.BeginInvoke(async () =>
                {
                    MessageBoxResult result = MessageBox.Show(Constants.SyncDataMessage, "Information", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        string syncStatus = await SyncData();
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                               {
                                   SyncStatusDisplay(syncStatus);
                               });
                    }
                    else
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                });
            }
            saveEmergencyNumbers();
        }

        private async void StopButton_Click(object sender, EventArgs e)
        {
            if (Globals.CurrentProfile.IsSOSOn)
            {
                App.MyProfiles.SaveCurrentProfileSetting(SOS.Phone.ProfileViewModel.ProfileSetting.SOSStatus, "false");
                Globals.InitiateStopSOSEventsAsync(false);
            }
            else
            {
                //Deletes all previous posts for the Profile
                var retrier = new Retrier<Task<bool>>();
                bool result = await retrier.TryWithDelay(
                                () => LocationServiceWrapper.StopPostingsAsync("0"), Constants.RetryMaxCount, 0);
            }
        }


        private void CountryCodeListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var countryDetails = (CountryCodes)CountryCodeListPicker.SelectedItem;
            if (countryDetails != null)
            {
                App.SosViewModel.Helplines = null;
                if ((countryDetails.IsdCode != Constants.CountryCode && countryDetails.CountryName != Constants.CountryName) || (countryDetails.IsdCode == Constants.CountryCode && countryDetails.CountryName != Constants.CountryName))
                {
                    var BuddiesToDelete = App.MyBuddies.Buddies.Count > 0 ? App.MyBuddies.Buddies.Where(c => !c.PhoneNumber.StartsWith(countryDetails.IsdCode)) : null;
                    if (BuddiesToDelete != null && BuddiesToDelete.Count() > 0)
                    {
                        if (MessageBox.Show("This would remove any buddies which do not belong to " + countryDetails.CountryName, "Buddy Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            var count = BuddiesToDelete.Count();
                            while (BuddiesToDelete.Count() > 0)
                            {
                                var buddy = BuddiesToDelete.ElementAt(0);
                                if (buddy != null)
                                    App.MyBuddies.DeleteBuddy(buddy);
                                if (App.MyBuddies.IsSuccess)
                                {

                                    BuddiesListBox.Items.Remove(BuddiesListBox.Items.First(b => (b as BuddyTableEntity).PhoneNumber == buddy.PhoneNumber));
                                    DefaultCallerListPicker.Items.Remove(DefaultCallerListPicker.Items.First(b => (b as BuddyTableEntity).PhoneNumber == buddy.PhoneNumber));
                                }
                            }
                            if (Globals.IsRegisteredUser)
                                App.MyProfiles.UpdateIsDataSynced(false);
                            Constants.CountryCode = countryDetails.IsdCode;
                            Constants.CountryName = countryDetails.CountryName;
                            Constants.AmbulanceContact = countryDetails.Ambulance;
                            Constants.PoliceContact = countryDetails.Police;
                            Constants.FireBrigadeContact = countryDetails.Fire;
                            Constants.MaxPhonedigits = int.Parse(countryDetails.MaxPhoneDigits);

                        }
                        else
                        {
                            CountryCodeListPicker.SelectedItem = CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == Constants.CountryCode));
                        }

                    }
                    else
                    {
                        Constants.CountryCode = countryDetails.IsdCode;
                        Constants.CountryName = countryDetails.CountryName;
                        Constants.AmbulanceContact = countryDetails.Ambulance;
                        Constants.PoliceContact = countryDetails.Police;
                        Constants.FireBrigadeContact = countryDetails.Fire;
                        Constants.MaxPhonedigits = int.Parse(countryDetails.MaxPhoneDigits);
                    }
                }
                PoliceTb.Text = Constants.PoliceContact;
                AmbulanceContactTb.Text = Constants.AmbulanceContact;
                FireContactTb.Text = Constants.FireBrigadeContact;
            }
        }

        void saveEmergencyNumbers()
        {
            Constants.PoliceContact = PoliceTb.Text;
            Constants.AmbulanceContact = AmbulanceContactTb.Text;
            Constants.FireBrigadeContact = FireContactTb.Text;
            submitEmergencyNumbersChanges();
        }

        void submitEmergencyNumbersChanges()
        {
            if (Globals.CurrentProfile.CountryCode != Constants.CountryCode ||
                        Globals.CurrentProfile.PoliceContact != Constants.PoliceContact ||
                        Globals.CurrentProfile.AmbulanceContact != Constants.AmbulanceContact ||
                        Globals.CurrentProfile.FireContact != Constants.FireBrigadeContact ||
                        Globals.CurrentProfile.CountryName != Constants.CountryName ||
                        Globals.CurrentProfile.MaxPhonedigits != Constants.MaxPhonedigits.ToString())
            {
                CountryCodes countryDetails = new CountryCodes();
                countryDetails.CountryName = Constants.CountryName;
                countryDetails.Ambulance = Constants.AmbulanceContact;
                countryDetails.Fire = Constants.FireBrigadeContact;
                countryDetails.Police = Constants.PoliceContact;
                countryDetails.IsdCode = Constants.CountryCode;
                countryDetails.MaxPhoneDigits = Constants.MaxPhonedigits.ToString();
                App.MyProfiles.SaveEmergencyNumbers(countryDetails);
            }
        }

        void RenderEmergencyNumbers()
        {
            CountryCodeListPicker.SelectedItem = CountryCodeListPicker.Items.First(c => ((c as CountryCodes).IsdCode == Constants.CountryCode));
            CountryCodeListPicker.UpdateLayout();
            PoliceTb.Text = Constants.PoliceContact;
            AmbulanceContactTb.Text = Constants.AmbulanceContact;
            FireContactTb.Text = Constants.FireBrigadeContact;
            // Look to see if the tile already exists and if so, don't try to create again.
            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=SOSTile"));
            if (TileToFind != null)
            {
                PinSos.IsChecked = true;
            }
            else
                PinSos.IsChecked = false;
        }

        void ResetEmergencyNumbers()
        {
            //CountryCodeListPicker.SelectedIndex=0;
            //Constants.CountryCode = "+91";
            //Constants.CountryName = "India";
            //Constants.AmbulanceContact = "108";
            //Constants.PoliceContact = "100";
            //Constants.FireBrigadeContact = "101";
        }

        private void ChangeCountryCode_Click(object sender, RoutedEventArgs e)
        {
            SettingsPivot.SelectedIndex = 3;
        }

        private void PinSosToStart_Checked(object sender, RoutedEventArgs e)
        {
            // Look to see if the tile already exists and if so, don't try to create again.
            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=SOSTile"));

            // Create the tile if we didn't find it already exists.
            if (TileToFind == null)
            {
                StandardTileData NewTileData = new StandardTileData
                {
                    BackgroundImage = new Uri("/Assets/sospintile.png", UriKind.Relative),
                    Title = "SOS",
                    BackTitle = "SOS",
                    BackContent = "Tap this, if you are threatened"
                };

                // Create the tile and pin it to Start. This will cause a navigation to Start and a deactivation of our application.
                ShellTile.Create(new Uri("/Pages/StartSOS.xaml?DefaultTitle=SOSTile", UriKind.Relative), NewTileData);

            }
        }

        private void PinSosToStart_Unchecked(object sender, RoutedEventArgs e)
        {
            // Look to see if the tile already exists and if so, don't try to create again.
            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("DefaultTitle=SOSTile"));
            if (TileToFind != null)
                TileToFind.Delete();
        }

        private void SaveBuddyImage(string name, Stream imageStream)
        {
            using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.DirectoryExists("MyBuddyImages"))
                {
                    myIsolatedStorage.CreateDirectory("MyBuddyImages");
                }

                var filePath = Path.Combine("MyBuddyImages", "newImage1" + ".jpg");

                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filePath, FileMode.Create, myIsolatedStorage))
                {
                    // var memoryStream= new MemoryStream();

                    //imageStream.CopyTo(memoryStream);
                    imageStream.Position = 0;
                    byte[] byteArray = new byte[imageStream.Length];
                    imageStream.Read(byteArray, 0, (int)imageStream.Length);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                }
            }
        }

        private async Task SaveToLocalFolderAsync(Stream file, string fileName)
        {
            //string filename = Path.GetFileName(filename);
            if (file != null)
            {
                var memoryStream = new MemoryStream();
                file.Position = 0;
                await file.CopyToAsync(memoryStream);

                StorageFolder localFolder = ApplicationData.Current.LocalFolder;

                StorageFile storageFile = await localFolder.CreateFileAsync(fileName + ".jpg", CreationCollisionOption.ReplaceExisting);
                using (Stream outputStream = await storageFile.OpenStreamForWriteAsync())
                {
                    await outputStream.WriteAsync(memoryStream.ToArray(), 0, (int)memoryStream.ToArray().Length);
                    outputStream.Close();
                }
            }
            //  StorageFile sampleFile = await localFolder.GetFileAsync("newImage2.jpg");
            //Stream imgout = await sampleFile.;
        }

        private async Task DeleteFromLocalFolder(string fileName)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            if (System.IO.File.Exists(string.Format(@"{0}\{1}", ApplicationData.Current.LocalFolder.Path, fileName + ".jpg")))
            {
                StorageFile storageFile = await localFolder.GetFileAsync(fileName + ".jpg");
                await storageFile.DeleteAsync();
            }
        }
    }

}

namespace PictureConv
{
    public class ContactPictureConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ContactsModel c = value as ContactsModel;
            if (c == null)
            {
                var b = value as BuddyTableEntity;
                if (b == null)
                    return new BitmapImage(new Uri("/Assets/Images/defaultImage.png", UriKind.Relative));
                else
                {

                    string filename = Path.GetFileName(b.Name + ".jpg");
                    string localFolderPath = ApplicationData.Current.LocalFolder.Path;
                    if (System.IO.File.Exists(string.Format(@"{0}\{1}", ApplicationData.Current.LocalFolder.Path, filename)))
                    {
                        var bitmap = new BitmapImage(new Uri(Path.Combine(localFolderPath, filename), UriKind.Absolute));
                        return bitmap;
                    }
                    else
                        return new BitmapImage(new Uri("/Assets/Images/defaultImage.png", UriKind.Relative));
                    //return Path.Combine(localFolderPath, filename);


                    //return new Uri("ms-appdata:///local/" + "newImage2" + ".jpg", UriKind.RelativeOrAbsolute);
                    // var file = ApplicationData.Current.LocalFolder.GetFileAsync("newImage2.jpg");
                    // bitmap.SetSource( file.OpenAsync(FileAccessMode.Read));
                    // var file = await Storage.KnownFolders.DocumentsLibrary.GetFileAsync(ImagePath);
                    //var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    //ImageSource = new BitmapImage();
                    //ImageSource.SetSource(fileStream);
                    //var bitmap = new BitmapImage(new Uri("ms-appdata:///local/" + "newImage2" + ".jpg", UriKind.Absolute));


                }
            }
            System.IO.Stream imageStream = c.ImageStream;

            if (null != imageStream)
            {
                imageStream.Seek(0, SeekOrigin.Begin);
                return Microsoft.Phone.PictureDecoder.DecodeJpeg(imageStream);
            }
            return new BitmapImage(new Uri("/Assets/Images/defaultImage.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }//End converter class

}

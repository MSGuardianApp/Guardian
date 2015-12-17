namespace SOS.Phone
{
    public static class CustomMessage
    {
        public const string UnableToConnectService = "Unable to connect to the Guardian server! Please try after sometime. If you continue to receive this message, please contact GuardianApp@outlook.com";
        public const string UnableToSyncProfile = "Sync profile to Guardian Server has failed, however changes have been saved locally. You can try syncing later from Profile Page.";
        public const string UnableToUpdateMobile = "Unable to update the mobile number in Guardian Server. Please try again later.";
        public const string ProfileNotFound = "We lost your profile in Guardian Server. Please re-register to be benefited from Registered user capabilities.";
        public const string InvalidProfile = "Unable to sync changes with Guardian Server! Your mobile number was claimed by someone else. Please update your mobile number. However, changes you have made were saved locally.";
        public const string ExitApplication = "Either Tracking or SOS is activated. Exiting app will deactivate them. You can click on Windows button to suspend Guardian and to do other tasks.";

        public const string SafeMessageConfirmationText = "A safe message has been sent to all your buddies. To send a personalized message to your buddies, press OK. Else press CANCEL.";
        public const string RegisteredUserSafeMessageConfirmationText = "A safe message has been sent to all your buddies. To send a personalized message to your buddies, press OK. Else press CANCEL.";

        public const string MapTapAndHoldText = "Tap and hold anywhere in the map to show a Pushpin. After that you can show route from your location to the destination.";
        public const string AddingBuddyEmptyNumberText = "You cannot add a buddy without phone number.";
        public const string NetworkNotAvailableText = "GPRS/ WIFI connection is not available to connect to Guardian Server. Please enable and try again.";
        public const string Max5BuddiesValidationText = "You have reached your limit of 5 buddies allowed for a profile. Please remove someone to add a new Buddy.";
        public const string NameEmptyValidationText = "Name cannot be empty. Please enter Buddy name.";
        public const string InvalidPhoneNumberText = "Incorrect contact number for your current country. Please validate the contact number length.";
        public const string UnregisterConfirmationText = "Are you sure you want to unregister? This will permanently delete all your data from Guardian Server and cannot be retreived.";
        public const string UnregisterSuccessText = "You are successfully unregistered with Guardian App! All your data has been permanently deleted and cannot be retrieved.";
        public const string UnregisterFailUnauthorizedText = "You are not authorized to unregister! Please contact Guardian team at GuardianApp@outlook.com";
        public const string UnregisterFailText = "Unable to unregister now! Please try after sometime. If you continue to receive this message, please contact GuardianApp@outlook.com";
        public const string BuddySearchFailText = "No matching contacts found. Please try with a different name.";
        public const string BuddyNumberExistsText = "Buddy with this Contact Number already exists.";
        public const string BuddyEmailExistsText = "Buddy with this Email Id already exists.";
        public const string UnableToSaveSettingText = "The setting was not saved. Please try again later. If you continue to receive this message, please contact GuardianApp@outlook.com";
        public const string StopSOSFailText = "Unable to connect to the Guardian server! While we retry, you can use other features of the app.";
        public const string StopSOSSuccessText = "SOS is turned off! Hope you are safe now. You can trun off tracking, when not needed.";
        public const string AppUpdateNotifyText = "There is an updated version for the app available in store. Press OK to update.";
        public const string AddingBuddyFromDifferentRegionInRegmode = "You cannot add a buddy from a different country.";
        public const string AddingBuddyFromDifferentRegionInUnRegmode = "You cannot add a buddy from a different country. Go to Preferences to change your country code.";

        public const string InvalidPhotoCapture = "You should be in Tracking/ SOS to upload evidence.";
        public const string EvidenceUploadFailText = "Failed to upload evidence to Guardian Cloud Server. But the photo is saved in your phone memory.";
        public const string RegisteredUserLoadingFailText = "An error occurred while loading the user data.";
        public const string RegisteredBuddiesDeleteText = "All your buddies from the previous country would be removed.";
        public const string SettingsSyncSuccessText = "Your settings have been successfully synced with the Guardian server.";


        public const string SendReportErrorText = "No Error report available.";
        public const string ProfileLoadedSuccessfullyText = "Your profile is loaded successfully.";
        public const string LiveConnectExceptionText = "Unable to connect to Microsoft Live Services. Please try registering/ login after some time.";
        public const string MobileNumberEditingText = "Profile already exist with this Mobile Number. Please provide a different mobile number.";
        public const string MobileNumberValidatingSMSToast = "You will shortly receive an SMS with Security Code to the specified Mobile Number. If you dont receive, you can either retry or contact GuardianApp@outlook.com";
        public const string MobileNumberValidatingEmailToast = "You will shortly receive an Email with Security Code for the specified Mobile Number";
        public const string ProfileEditedAndUpdatedSuccessfullyText = "Your profile is updated successfully.";
        public const string ProfileCreationFailText = "Some unexpected error occurred. Please try after sometime.";
        public const string IncorrectSecurityCode = "Incorrect Security Code. Please provide a valid security code.";
        public const string ProfileCreationSuccessText = "Your profile is created successfully";
        public const string InvalidUserNameWhileRegistering = "Please provide a valid name.";
        public const string InvalidMobileNumberWhileRegistering = "Please provide a valid mobile number.";
        public const string InvalidEnterpriseEmailWhileRegistering = "Please provide your valid enterprise email id.";
        public const string InvalidSecurityCodeWhileRegistering = "Please provide a valid mobile security code you have received via SMS.";
        public const string InvalidEmailSecurityCodeWhileRegistering = "Please provide a valid email security code you have received to your enterprise email.";
        public const string ReportTeaseToServerSuccessText = "Incident has been successfully reported. Appreciate your social responsibility.";
        public const string ContactNumberIsMustForBuddy = "A contact Number is required to add a Buddy.";
        public const string GroupExistsAlready = "Group Already Exists.";
        public const string MapTapHoldToGetDirections = "Tap and hold somewhere in the map to show a Pushpin. After that you can get directions to get there.";
        public const string MapTapHoldToShowRouteFromLocationToDestination = "Tap and hold somewhere in the map to show a Pushpin. After that you can see route from your location to the destination.";
        public const string TurnOffTrackingText = "You can turn off, when not needed";
        public const string TrackingIsOffText = "Guardian tracking is off.";
        public const string TrackingCannotBeOffText = "When you are in SOS, tracking cannot be switched off.";
        public const string TurnOffIfSafeText = "If you are safe, please turn SOS off.";
        public const string NoRouteToClear = "No route to clear.";
        public const string RefineSearchText = "Please refine your search.";
        public const string RanOutOfDestination = "We ran out of road for this destination.";
        public const string EnterDestinationForSearchText = "Please enter a destination.";
        public const string TrackingNotEnabledText = "Tracking is not enabled. You need to enable tracking to send link to track you";
        public const string NoActiveSessionText = "There is no active SOS or Tracking session. Your command will be ignored...";

        public const string UpgradedDataLoadFailText = "We are unable to load your registration information. Make sure you have internet connection and re-open the app to automatically load your registered profile [OR] Please login again to get connected to Guardian Server";
        public const string ProfileDeactivatedText = "Your current profile has been deactivated as someone claimed your mobile number. Please update the mobile number.";

        public const string EnterpriseSecurityTokenSMSInfoText = "(Validate will generate two security codes which will be send via SMS and email to the specified Mobile Number and Enterprise Email respectively)";
        public const string EnterpriseSecurityTokenEmailInfoText = "(Validate will generate two security codes which will be send via email to the Logged-in Email and specified Enterprise Email)";
        public const string SecurityTokenSMSInfoText = "(Validate will generate a one-time security code which will be send via SMS to specified mobile number)";
        public const string SecurityTokenEmailInfoText = "(Validate will generate a one-time security code which will be send via email to the Logged-in Email)";

        public const string UnableToGetLoginInfoText="Unable to receive information from the login session. Please press back button and login again ";
    }

}

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardian.Common;
using SOS.Model;
using SOS.Service.Utility;

namespace SOS.Service.Implementation
{
    internal class ServiceUtility
    {
        private static readonly string _smsSafeMessage = Common.Resources.Messages.SMSSafeMessage;
        private static readonly string _emailSafeMessage = Common.Resources.Messages.EmailSafeMessage;
        private static readonly string _facebookSafeMessage = Common.Resources.Messages.FacebookSafeMessage;
        private static readonly string _emailSafeSubject = Common.Resources.Messages.EmailSafeSubject;
        private static readonly string _facebookSafeSubject = Common.Resources.Messages.FacebookSafeSubject;

        private static string DecryptMobileNumbers(string encryptedMobileNumbers)
        {
            string[] encryptedNumberList = encryptedMobileNumbers.Split(',');
            var decryptedNumbers = new StringBuilder();
            foreach (string number in encryptedNumberList)
                decryptedNumbers.Append(Utility.Security.Decrypt(number) + ",");

            return decryptedNumbers.ToString().TrimEnd(',');
        }

        public static async Task SendSafeNotificationAsync(LiveSession session)
        {
            if (!string.IsNullOrEmpty(session.SMSRecipientsList) || !string.IsNullOrEmpty(session.EmailRecipientsList) ||
                !string.IsNullOrEmpty(session.FBGroupID) || !string.IsNullOrEmpty(session.FBAuthID))
            {
                string decryptedBuddyNumbers = string.Empty, mobileNumber = string.Empty;
                mobileNumber = Utility.Security.Decrypt(session.MobileNumber);
                try
                {
                    //Send Safe SMS to Buddies
                    if (Config.SendSms && string.IsNullOrEmpty(session.SMSRecipientsList))
                    {
                        try
                        {
                            if (!session.SMSRecipientsList.StartsWith("DM"))
                                decryptedBuddyNumbers = "DM" + DecryptMobileNumbers(session.SMSRecipientsList);

                            SMS.SendSMS(decryptedBuddyNumbers.Substring(2), GetSafeSMSBody(session.Name));
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(
                                String.Format(
                                    "Error while sending Safe SMS for user with Mobile Number : {0}, ErrorMessage: {1}",
                                    mobileNumber, ex.Message));
                        }
                    }

                    //Send Email to buddies
                    if (!string.IsNullOrEmpty(session.EmailRecipientsList))
                    {
                        try
                        {
                            Email.SendEmail(session.EmailRecipientsList.Split(',').ToList(),
                                GetSafeEmailBody(session.Name, mobileNumber),
                                GetSafeEmailSubject(session.Name));
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(
                                String.Format("Error while sending email for Profile: {0}, ErrorMessage: {1}",
                                    session.ProfileID, ex.Message));
                        }
                    }

                    //Post on FB account
                    if (!String.IsNullOrEmpty(session.FBGroupID) && !String.IsNullOrEmpty(session.FBAuthID) &&
                        session.LastFacebookPostTime.Value.AddMinutes(Config.FacebookPostGap) <= DateTime.UtcNow)
                    {
                        try
                        {
                            FaceBookPosting.FBPosting(session.FBAuthID,
                                GetSafeFacebookMessage(session.Name),
                                GetSafeFacebookCaption(session.Name), null, session.FBGroupID);
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(
                                String.Format(
                                    "Error while posting on FB Group for the Profile: {0}, ErrorMessage: {1}",
                                    session.ProfileID, ex.Message));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(
                        String.Format(
                            "Error while posting to friends for GeoTag with Identifier: {0}, ErrorMessage: {1}",
                            session.SessionID, ex.Message));
                }
                finally
                {
                }
            }
        }

        public static string GetSafeSMSBody(string UserName)
        {
            return _smsSafeMessage.Replace("{name}", UserName);
        }

        public static string GetSafeEmailBody(string UserName, string UserPhoneNumber)
        {
            return _emailSafeMessage.Replace("{name}", UserName).Replace("{phone}", UserPhoneNumber);
        }

        public static string GetSafeEmailSubject(string UserName)
        {
            return _emailSafeSubject.Replace("{name}", UserName);
        }

        public static string GetSafeFacebookMessage(string UserName)
        {
            return _facebookSafeMessage.Replace("{name}", UserName);
        }

        public static string GetSafeFacebookCaption(string UserName)
        {
            return _facebookSafeSubject.Replace("{name}", UserName);
        }
    }
}
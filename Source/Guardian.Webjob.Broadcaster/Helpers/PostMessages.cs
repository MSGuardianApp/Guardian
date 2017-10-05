using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOS.Service.Utility;
using Entity = SOS.AzureStorageAccessLayer.Entities;
using System.Threading.Tasks;
using SOS.Model;
using Guardian.Common;
using Guardian.Common.Configuration;
using System.Diagnostics;

namespace Guardian.Webjob.Broadcaster
{
    internal static class PostMessages
    {
        public static bool SendSMS = false;
        private static Dictionary<string, string> TinyUris = new Dictionary<string, string>();

        private static string GetTinyUri(string profileID, string Token)
        {
            return Shortify.CreateDynamicLocateMeURI(profileID, Token);
        }

        private static string DecryptMobileNumbers(string encryptedMobileNumbers)
        {
            string[] encryptedNumberList = encryptedMobileNumbers.Split(',');
            StringBuilder decryptedNumbers = new StringBuilder();
            foreach (var number in encryptedNumberList)
                decryptedNumbers.Append(Security.Decrypt(number) + ",");

            return decryptedNumbers.ToString().TrimEnd(',');
        }

        public static List<LiveSession> SendSOSNotifications(List<LiveSession> sessions, Settings settings)
        {
            Parallel.ForEach(sessions, session =>
            {
                if (!string.IsNullOrEmpty(session.SMSRecipientsList) || !string.IsNullOrEmpty(session.EmailRecipientsList)
                || !string.IsNullOrEmpty(session.FBGroupID) || !string.IsNullOrEmpty(session.FBAuthID))
                {
                    string tinyUri = string.Empty, mobileNumber = string.Empty;
                    string address = string.Empty;
                    try
                    {
                        if (!string.IsNullOrEmpty(session.Lat) && !string.IsNullOrEmpty(session.Long))
                        {
                            try
                            {
                                address = BingService.GetAddressByPointAsync(session.Lat, session.Long).Result;
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError($"Error while retreiving Bing Address for Profile: {session.ProfileID}, ErrorMessage: {ex.Message}");
                            }
                        }

                        if (string.IsNullOrWhiteSpace(session.TinyUri))
                            session.TinyUri = GetTinyUri(session.ProfileID.ToString(), session.SessionID);

                        tinyUri = session.TinyUri;
                        mobileNumber = Security.Decrypt(session.MobileNumber);

                        //Send SMS notifications to Buddies
                        if (settings.SendSms && !string.IsNullOrEmpty(session.SMSRecipientsList)
                        && (!session.LastSMSPostTime.HasValue || session.LastSMSPostTime.Value.AddMinutes(settings.SMSPostGap) <= DateTime.UtcNow))//DATEADD(minute,@SMSInterval,LastSMSPostTime) <= GETDATE() 
                        {
                            try
                            {
                                if (!session.SMSRecipientsList.StartsWith("DM"))
                                    session.SMSRecipientsList = "DM" + DecryptMobileNumbers(session.SMSRecipientsList);

                                SMS.SendSMS(session.SMSRecipientsList.Substring(2), Utility.GetSMSBody(tinyUri, session.Name, address, mobileNumber));
                                session.LastSMSPostTime = System.DateTime.UtcNow;
                                session.NoOfSMSSent++;
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError($"Error while sending SMS for user with Mobile Number : {mobileNumber}, ErrorMessage: {ex.Message}");
                            }
                        }

                        //Send Email to buddies
                        if (!string.IsNullOrEmpty(session.EmailRecipientsList)
                        && (!session.LastEmailPostTime.HasValue || session.LastEmailPostTime.Value.AddMinutes(settings.EmailPostGap) <= DateTime.UtcNow))
                        {
                            try
                            {
                                Email.SendEmail(session.EmailRecipientsList.Split(',').ToList(),
                                    Utility.GetEmailBody(tinyUri, session.Name, address, mobileNumber, session.LastCapturedDate.Value),
                                    Utility.GetEmailSubject(session.Name));
                                session.LastEmailPostTime = DateTime.UtcNow;
                                session.NoOfEmailsSent++;
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError(String.Format("Error while sending email for Profile: {0}, ErrorMessage: {1}", session.ProfileID, ex.Message));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(String.Format("Error while posting to friends for GeoTag with Identifier: {0}, ErrorMessage: {1}", session.SessionID, ex.Message));
                    }
                }
            });
            return sessions;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOS.Service.Utility;
using Entity = SOS.AzureStorageAccessLayer.Entities;
using System.Threading.Tasks;
using SOS.Model;
using SOS.ConfigManager;
namespace SOS.WorkerRole.Broadcaster
{
    internal static class PostMessages
    {
        public static TimeSpan SubGroupAllocationIntervalInMinutes;
        public static bool SendSMS;
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

        public async static Task<List<LiveSession>> SendSOSNotifications(List<LiveSession> sessions)
        {
            //List<LiveSession> processedSessions = new List<LiveSession>();

            Parallel.ForEach(sessions, session =>
            {
                if (!string.IsNullOrEmpty(session.SMSRecipientsList) || !string.IsNullOrEmpty(session.EmailRecipientsList) || !string.IsNullOrEmpty(session.FBGroupID) || !string.IsNullOrEmpty(session.FBAuthID))
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
                                address = string.Format("Latitude : {0} , Longitude : {1}", session.Lat, session.Long);
                                System.Diagnostics.Trace.TraceError(String.Format("Error while retreiving Bing Address for Profile: {0}, ErrorMessage: {1}", session.ProfileID, ex.Message));
                            }
                        }

                        if (string.IsNullOrWhiteSpace(session.TinyUri))
                            session.TinyUri = GetTinyUri(session.ProfileID.ToString(), session.SessionID);

                        tinyUri = session.TinyUri;
                        mobileNumber = Security.Decrypt(session.MobileNumber);

                        //Send SMS notifications to Buddies
                        if (Config.SendSms && !string.IsNullOrEmpty(session.SMSRecipientsList) && (!session.LastSMSPostTime.HasValue || session.LastSMSPostTime.Value.AddMinutes(Config.SMSPostGap) <= DateTime.UtcNow))//DATEADD(minute,@SMSInterval,LastSMSPostTime) <= GETDATE() 
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
                                System.Diagnostics.Trace.TraceError(String.Format("Error while sending SMS for user with Mobile Number : {0}, ErrorMessage: {1}", mobileNumber, ex.Message));
                            }
                        }

                        //Send Email to buddies
                        if (!string.IsNullOrEmpty(session.EmailRecipientsList) && (!session.LastEmailPostTime.HasValue || session.LastEmailPostTime.Value.AddMinutes(Config.EmailPostGap) <= DateTime.UtcNow))
                        {
                            try
                            {
                                Email.SendEmail(session.EmailRecipientsList.Split(',').ToList(),
                                    Utility.GetEmailBody(tinyUri, session.Name, address, mobileNumber, session.LastCapturedDate.Value),
                                    Utility.GetEmailSubject(session.Name));
                                session.LastEmailPostTime = System.DateTime.UtcNow;
                                session.NoOfEmailsSent++;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.TraceError(String.Format("Error while sending email for Profile: {0}, ErrorMessage: {1}", session.ProfileID, ex.Message));
                            }
                        }

                        ////Post on FB account
                        //if (!String.IsNullOrEmpty(session.FBGroupID) && !String.IsNullOrEmpty(session.FBAuthID) && (!session.LastFacebookPostTime.HasValue || session.LastFacebookPostTime.Value.AddMinutes(Config.FacebookPostGap) <= DateTime.UtcNow))
                        //{
                        //    try
                        //    {
                        //        FaceBookPosting.FBPosting(session.FBAuthID,
                        //            Utility.GetFacebookMessage(tinyUri, session.Name, address, mobileNumber, session.LastCapturedDate.Value),
                        //            Utility.GetFacebookCaption(session.Name), tinyUri, session.FBGroupID);
                        //        session.LastFacebookPostTime = System.DateTime.UtcNow;
                        //        session.NoOfFBPostsSent++;
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        System.Diagnostics.Trace.TraceError(String.Format("Error while posting on FB Group for the Profile: {0}, ErrorMessage: {1}", session.ProfileID, ex.Message));
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(String.Format("Error while posting to friends for GeoTag with Identifier: {0}, ErrorMessage: {1}", session.SessionID, ex.Message));
                    }
                    finally
                    {
                    }
                }
            });
            return sessions;
        }
    }
}

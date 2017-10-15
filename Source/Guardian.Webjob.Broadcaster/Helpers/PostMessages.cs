using Guardian.Common.Configuration;
using SOS.Model;
using SOS.Service.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian.Webjob.Broadcaster
{
    internal class PostMessages
    {
        Settings settings;

        //public bool SendSMS = false;

        public PostMessages(Settings settings)
        {
            this.settings = settings;
        }
        
        private string GetTinyUri(string profileID, string Token)
        {
            return new Shortify(settings).CreateDynamicLocateMeURI(profileID, Token).Result;
        }

        private string DecryptMobileNumbers(string encryptedMobileNumbers)
        {
            string[] encryptedNumberList = encryptedMobileNumbers.Split(',');
            StringBuilder decryptedNumbers = new StringBuilder();
            foreach (var number in encryptedNumberList)
                decryptedNumbers.Append(Security.Decrypt(number) + ",");

            return decryptedNumbers.ToString().TrimEnd(',');
        }

        public async Task<List<LiveSession>> SendSOSNotifications(List<LiveSession> sessions, Settings settings)
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
                                address = new BingService(settings).GetAddressByPointAsync(session.Lat, session.Long).Result;
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

                                new SMS(settings).SendSMS(session.SMSRecipientsList.Substring(2), Utility.GetSMSBody(tinyUri, session.Name, address, mobileNumber));
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
                                new Email(settings).SendEmail(session.EmailRecipientsList.Split(',').ToList(),
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

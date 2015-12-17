using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using SendGrid;
//using SendGridMail;
//using SendGridMail.Transport;
using System.Diagnostics;
using SOS.ConfigManager;

namespace SOS.Service.Utility
{
    public static class Email
    {
        private const string DisplayName = "Guardian";
        private const string FromAddress = "guardianapp@outlook.com";

        private static string sendGridUserID;
        private static string sendGridPassword;

        private static readonly string GuardianPortalUri = Config.GuardianPortalUri;      

        static Email()
        {
            sendGridUserID = Config.sendGridUserID;
            sendGridPassword = Config.sendGridPassword;
        }

        public static bool SendGroupValidationMail(string to, string validationKey, string profileId, string validationType)
        {
            string messageBody;
            string messageSubject;

            if (validationType == "GroupMember")
            {
                messageBody = ConfigManager.Resources.Messages.EmailGroupValidationMsgTmp;
                messageSubject = ConfigManager.Resources.Messages.EmailGroupValidationSubj;
            }
            else
            {
                messageBody = ConfigManager.Resources.Messages.EmailMarshalValidationMsgTmp;
                messageSubject = ConfigManager.Resources.Messages.EmailMarshalValidationSubj;
            }

            string message = string.Format(messageBody, validationKey, profileId, GuardianPortalUri);
            var toList = new List<string>();
            toList.Add(to);
            return SendEmail(toList, message, messageSubject);
        }

        public static bool SendEmail(List<string> To, string Body, string Subject)
        {
            try
            {
                SendEmailUsingSendGrid(To, Subject, Body);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }

        public static bool SendEmailUsingSendGrid(List<string> to, string subject, string body)
        {
            //create a new message object 
            var message = new SendGridMessage();

            //set the message recipients 
            foreach (var recipient in to)
                message.AddTo(recipient);

            message.From = new MailAddress(FromAddress, DisplayName);
            message.Html = body;
            message.Subject = subject;

            //create an instance of the Web transport mechanism 
            var transportInstance = new Web(new NetworkCredential(sendGridUserID, sendGridPassword));

            //send the mail 
            transportInstance.DeliverAsync(message);
            return true;
        }

        public static bool SendEmailBuddyNotification(string emailTo, string profileUserName, string profileMobileNumber, string subscribeUri, string UnsubscribeURI)
        {
            string messageBody = ConfigManager.Resources.Messages.BuddyNotificationEmailBody;
            string messageSubject = ConfigManager.Resources.Messages.BuddyNotificationEmailSubject;

            string body = string.Format(messageBody, profileUserName, profileMobileNumber, UnsubscribeURI);
            string subjectLine = string.Format(messageSubject, profileUserName, profileMobileNumber);
            List<string> tos = new List<string>();
            tos.Add(emailTo);
            SendEmail(tos, body, subjectLine);

            return true;
        }

        public static bool SendUnRegisterEmailNotification(string emailTo)
        {
            string messageBody = ConfigManager.Resources.Messages.UnRegisterNotificationEmailBody;
            string messageSubject = ConfigManager.Resources.Messages.UnRegisterNotificationEmailSubject;

            List<string> tos = new List<string>();
            tos.Add(emailTo);
            return SendEmail(tos, messageBody, messageSubject);
        }

    }
}

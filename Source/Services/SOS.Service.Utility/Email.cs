using Guardian.Common.Configuration;
using Guardian.Common.Resources;
using SendGrid;
using System;
using System.Collections.Generic;
//using SendGridMail;
//using SendGridMail.Transport;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace SOS.Service.Utility
{
    public class Email
    {
        private const string DisplayName = "Guardian";
        private const string FromAddress = "guardianapp@outlook.com";

        private string sendGridUserID;
        private string sendGridPassword;

        private readonly string guardianPortalUri;

        public Email(Settings settings)
        {
            sendGridUserID = settings.SendGridUserID;
            sendGridPassword = settings.SendGridPassword;
            guardianPortalUri = settings.GuardianPortalUri;
        }

        public bool SendGroupValidationMail(string to, string validationKey, string profileId, string validationType)
        {
            string messageBody;
            string messageSubject;

            if (validationType == "GroupMember")
            {
                messageBody = Messages.EmailGroupValidationMsgTmp;
                messageSubject = Messages.EmailGroupValidationSubj;
            }
            else
            {
                messageBody = Messages.EmailMarshalValidationMsgTmp;
                messageSubject = Messages.EmailMarshalValidationSubj;
            }

            string message = string.Format(messageBody, validationKey, profileId, guardianPortalUri);
            var toList = new List<string>();
            toList.Add(to);
            return SendEmail(toList, message, messageSubject);
        }

        public bool SendEmail(List<string> To, string Body, string Subject)
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

        public bool SendEmailUsingSendGrid(List<string> to, string subject, string body)
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

        public bool SendEmailBuddyNotification(string emailTo, string profileUserName, string profileMobileNumber, string subscribeUri, string UnsubscribeURI)
        {
            string messageBody = Messages.BuddyNotificationEmailBody;
            string messageSubject = Messages.BuddyNotificationEmailSubject;

            string body = string.Format(messageBody, profileUserName, profileMobileNumber, UnsubscribeURI);
            string subjectLine = string.Format(messageSubject, profileUserName, profileMobileNumber);
            List<string> tos = new List<string>();
            tos.Add(emailTo);
            SendEmail(tos, body, subjectLine);

            return true;
        }

        public bool SendUnRegisterEmailNotification(string emailTo)
        {
            string messageBody = Messages.UnRegisterNotificationEmailBody;
            string messageSubject = Messages.UnRegisterNotificationEmailSubject;

            List<string> tos = new List<string>();
            tos.Add(emailTo);
            return SendEmail(tos, messageBody, messageSubject);
        }

    }
}

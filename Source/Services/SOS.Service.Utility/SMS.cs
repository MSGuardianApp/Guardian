using System;
using System.Threading.Tasks;
using System.Net;
using Guardian.Common.Configuration;
using Guardian.Common.Resources;
using Twilio;

namespace SOS.Service.Utility
{
    public class SMS
    {
        Settings settings;

        private static string _smsServiceUserID;
        private static string _smsServicePassword;
        private static string _intlsmsServiceUserID;
        private static string _intlsmsServicePassword;
        private static string _smsPhoneValidMsg = Messages.PhoneValidationSMS;
        private static string _smsPhoneReValidMsg = Messages.PhoneReValidationSMS;
        private static string _smsDefaultFromNumb;

        private const string _smsuri = "http://www.myvaluefirst.com/smpp/sendsms?";
        public SMS(Settings settings)
        {
            this.settings = settings;

            _smsServiceUserID = settings.SMSServiceUserID;
            _smsServicePassword = settings.SMSServicePassword;
            _intlsmsServiceUserID = settings.IntlSMSServiceUserID;
            _intlsmsServicePassword = settings.IntlSMSServicePassword;
            _smsDefaultFromNumb = settings.SMSDefaultFromNumber;
        }

        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="ToNumbers">Comma separated phone numbers</param>
        /// <param name="fromNumber">Number of user on whose behalf message is being sent</param>
        /// <param name="Message">Contents of SMS</param>
        /// <returns>Success</returns>
        public bool SendSMS(string ToNumbers, string Message, bool IsInternational = false)
        {

            var _uriParameterString = string.Format("category=bulk&username={0}&password={1}&to={2}&from={3}&text={4}", IsInternational ? _intlsmsServiceUserID : _smsServiceUserID, IsInternational ? _intlsmsServicePassword : _smsServicePassword, ToNumbers, _smsDefaultFromNumb, Message);
            _uriParameterString = Uri.EscapeUriString(_uriParameterString);
            _uriParameterString = _smsuri + _uriParameterString.Replace("+", "%2B");

            HttpWebRequest smsReq = (HttpWebRequest)WebRequest.Create(_uriParameterString);
            HttpWebResponse smsResp = (HttpWebResponse)smsReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(smsResp.GetResponseStream());

            string response = respStreamReader.ReadToEnd();//"Invalid Receiver"
            respStreamReader.Close();
            smsResp.Close();

            return true;
        }

        public bool SendSMSViaTwilio(string ToNumbers, string Message)
        {
            string AccountSid = _smsServiceUserID;
            string AuthToken = _smsServicePassword;
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var toNumbers = ToNumbers.Split(',');
            Parallel.ForEach(toNumbers, number =>
                    {
                        var response = twilio.SendMessage(_smsDefaultFromNumb, number, Message);
                    });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToNumber"></param>
        /// <param name="Token"></param>
        /// <param name="IsReSend"></param>
        /// <returns></returns>
        public bool SendPhoneValidatorMessage(string ToNumber, string Token, bool IsReSend, bool IsInternational = false)
        {
            string Message = (!IsReSend) ? _smsPhoneValidMsg : _smsPhoneReValidMsg;
            string Last4Dig = ToNumber.Substring(ToNumber.Length - 4, 4);
            Message = string.Format(Message, Token, Last4Dig);

            return new SMS(settings).SendSMS(ToNumber, Message, IsInternational);
        }

        public bool SendSMSBuddyNotification(string toNumber, string profileUserName, string profileMobileNumber, string subscribeURI, string UnsubscribeURI)
        {
            string smsBody = Messages.BuddyNotificationSMSBody;
            string message = string.Format(smsBody, profileUserName, profileMobileNumber, UnsubscribeURI);
            new SMS(settings).SendSMS(toNumber, message);
            return true;
        }
    }
}

using System;

namespace SOS.Model.DTO
{
    public class LiveSessionLite 
    {
        public long ProfileID { get; set; }
        public string SessionID { get; set; }
        public DateTime? LastSMSPostTime { get; set; }
        public DateTime? LastEmailPostTime { get; set; }
        public DateTime? LastFacebookPostTime { get; set; }
        public string SMSRecipientsList { get; set; }
        public string TinyUri { get; set; }
        public short? NoOfSMSSent { get; set; }
        public short? NoOfEmailsSent { get; set; }
        public short? NoOfFBPostsSent { get; set; }
    }
}
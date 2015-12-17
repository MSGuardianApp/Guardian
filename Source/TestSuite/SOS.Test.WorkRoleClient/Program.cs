using System;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using SOS.Service.Interfaces.DataContracts;

namespace SOS.Test.WorkRoleClient
{
    class Program
    {
        public static void ActivateSosPost2Server()
        {
            try
            {
                GeoTag geoTag = new GeoTag()
                {
                    Alt = "",
                    GeoDirection = Direction.something,
                    SessionID = "1.228113154639812",
                    Lat = "17.435943700000000000",
                    Long = "78.341673099999980000",
                    Speed = 20,
                    TimeStamp = DateTime.Now.Ticks
                };

                const string ActivateSosServiceURL = "http://guardianservice.cloudapp.net/GeoUpdate.svc/PostMyLocation";
                string json = string.Empty;

                //Serialize the GeoTag class and post the data
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoTag));
                using (MemoryStream mem = new MemoryStream())
                {
                    ser.WriteObject(mem, geoTag);
                    string data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

                    WebClient webClient = new WebClient();
                    webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(ActivateSos_Complete);
                    webClient.Headers["Content-type"] = "application/json";
                    webClient.Encoding = Encoding.UTF8;
                    Uri uri = new Uri(ActivateSosServiceURL);
                    webClient.UploadStringAsync(uri, "POST", data);
                }

            }
            catch { }
        }

        private static void ActivateSos_Complete(object sender, UploadStringCompletedEventArgs e)
        {
            
        }

        static void Main(string[] args)
        {
            bool IsSOSOn = false;
            try
            {
                //Write
                //ActivateSosPost2Server();

                string URL = "http://guardianservice.cloudapp.net/LocationService.svc/GetBuddiesToLocate/2";
                string URL1 = "http://guardianservice.cloudapp.net/MembershipService.svc/GetAllSOSMembers/rajinikanth";
                //Read
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.CacheControl] = "no-cache;";
                string st = client.DownloadString(new Uri(URL));

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ProfileLiteList));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(st));
                ProfileLiteList profile = (ProfileLiteList)ser.ReadObject(ms);

                //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ProfileLiteList));
                //var list = (ProfileLiteList)ser.ReadObject(st);
                string str = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            int sendEmail = 0, sendSMS = 0, sendFbPosting = 0;
            //if (sendSMS == 1)
            //{
            //    try
            //    {

            //        //Utility.SendSMSTwilio();
            //        //Utility.SendSMSUbaid("site2sms", "9949091097", "venkat", "TEST: I'm in serious trouble. Urgently need your help! I'm currently @ Microsoft, Gachibowli, Hyderabad, AP, India 500082. Track me: http://lvry.vs/xbl (dummy url)", "7702690004");
            //        //Utility.SendSMSUbaid("site2sms", "9949091097", "venkat", "TEST: I'm in serious trouble. Urgently need your help! I'm currently @ Microsoft, Gachibowli, Hyderabad, AP, India 500082. Track me: http://lvry.vs/xbl (dummy url)", "9985121428");
            //        //int returnCode = Utility.SendSMSUbaid("160by2","9949091097", "venkat", "from VR. from 160by2. let me know, if you receive it", "9989025002");//
            //        //Utility.SendSMSViaWay2SMS("9949091097", "venkat", "Guardian test SMS from Worker Role", "9949091097");

            //        Utility.SendSMSviaVFirst("TEST via vFirst: SOS: Urgent help needed! I'm @ Microsoft, Gachibowli, Hyderabad, AP, India 500082.", "9949091097", "9989025002, 9502751602");//Track me:http://lvry.vs/xbl 9985121428, 7702690004,
            //        Console.WriteLine("SMS has been sent successfully");

            //        //Utility.SendSMS("9949091097", "venkat", "I love you", "9676223499");
            //        //Utility.SendSMS("9949091097", "venkat", "Guardian test SMS from Worker Role", "9949091097");
            //        //Console.WriteLine("SMS has been sent successfully");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Error occurred while sending SMS. Details: " + ex.Message);
            //    }
            //}

            //if (sendEmail == 1)
            //{
            //    try
            //    {
            //        //Utility.SendMail();
            //        //Console.WriteLine("Email has been sent successfully");
            //        FaceBookPosting();
            //        //Utility.SendMailViaLiveAccount();
            //        //Utility.SendMailViaGmailAccount();
            //        //Console.WriteLine("Email has been sent successfully");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Error occurred while sending email. Details: " + ex.Message);
            //    }
            //}

            //if (sendFbPosting == 1)
            //{
            //    try
            //    {
            //        FaceBookPosting();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Error occurred while sending FB posting. Details: " + ex.Message);
            //    }
            //}
        }
        //static void FaceBookPosting()
        //{
        //    try
        //    {
        //        Utility.FBPosting("AAAGZA0ItwKTsBAPOPVB2gJk5kmKXPpXI5tk88LYIcyTT0ZAZA7nlztjjkdbD7MCJWeVJpjK0ZCtcefSvZCZBKYL9iYVnJ9zSwjfM7ssZBI49dkZAZA1hcE0L8", "Wow we are done", "http://goo.gl/maps/S7OIf");
        //        Console.WriteLine("Facebook posting has been successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error occurred while sending email. Details: " + ex.Message);
        //    }
        //}
    }
}

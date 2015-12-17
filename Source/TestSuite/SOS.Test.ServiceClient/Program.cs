using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.InBound;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Test.ServiceClient
{

    class Program
    {
        //const string ServiceUrl = "http://newguardianservice.cloudapp.net/";
        const string ServiceUrl = "http://127.0.0.7:81/";
        static void Main(string[] args)
        {
            MethodContainer();
            Console.ReadLine();
        }

        static async void MethodContainer()
        {
            //ActivateSosPost2Server();

            EncryptDecrypt();
            //ActivateSosPost2ServerSync();
            //Incident();
            //SaveProfileSync();

            //SaveProfile();
        }
        private async static Task<string> SaveProfile()
        {
            ProfileBasicInfo p = new ProfileBasicInfo();
            p.Name = "vr";
            p.MobileNumber = "9949091097";

            string result = "";
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ProfileBasicInfo));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, p);
                string data = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                //data = "{\"Buddies\":null,\"Email\":null,\"Groups\":null,\"MobileNumber\":\"9949091097\",\"Name\":\"vr\",\"PrimeBuddy\":null,\"ProfileID\":null,\"UserID\":null}";
                WebClient wc = new WebClient();
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                //wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                //wc.UploadStringAsync(new Uri(ServiceUrl + "MembershipService.svc/SaveProfileBasicInfo"), "POST", data);
                result = await wc.UploadStringTask(new Uri(ServiceUrl + "MembershipService.svc/SaveProfileBasicInfo"), data);

            }

            return result;
        }

        private static string SaveProfileSync()
        {
            ProfileBasicInfo p = new ProfileBasicInfo();
            p.Name = "vr";
            p.MobileNumber = "9949091097";

            string result = "";
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ProfileBasicInfo));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, p);
                string data = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                //data = "{\"Buddies\":null,\"Email\":null,\"Groups\":null,\"MobileNumber\":\"9949091097\",\"Name\":\"vr\",\"PrimeBuddy\":null,\"ProfileID\":null,\"UserID\":null}";
                WebClient wc = new WebClient();
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
                result = wc.UploadString(new Uri(ServiceUrl + "MembershipService.svc/SaveProfileBasicInfo"), "POST", data);

            }

            return result;
        }

        static void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine(e.Error.Message);
            }
        }

        #region
        public async static Task<string> ActivateSosPost2Server()
        {
            string result = string.Empty;
            try
            {
                GeoTag geoTag = new GeoTag()
                {
                    Alt = "",
                    GeoDirection = Direction.something,
                    SessionID = "0.228113154639812",
                    Lat = "17.435943700000000000",
                    Long = "78.341673099999980000",
                    Speed = 20,
                    TimeStamp = DateTime.Now.Ticks,
                    ProfileID = 1
                };

                const string ActivateSosServiceURL = ServiceUrl + "GeoUpdate.svc/PostMyLocation";


                //Serialize the GeoTag class and post the data
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoTag));
                using (MemoryStream mem = new MemoryStream())
                {
                    ser.WriteObject(mem, geoTag);
                    string data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    //data = "{\"Alt\":\"\",\"GeoDirection\":0,\"Identifier\":\"1.228113154639812\",\"Lat\":\"17.435943700000000000\",\"Long\":\"78.341673099999980000\",\"Speed\":20,\"TimeStamp\":\"\\/Date(1362830850860+0530)\\/\"}";

                    WebClient webClient = new WebClient();
                    //webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(ActivateSos_Complete);
                    webClient.Headers["Content-type"] = "application/json";
                    webClient.Encoding = Encoding.UTF8;
                    Uri uri = new Uri(ActivateSosServiceURL);
                    result = webClient.UploadString(uri, data);
                }

            }
            catch { }
            return result;
        }

        public static Task<string> ActivateSosPost2ServerSync()
        {
            Task<string> result = null;
            try
            {
                GeoTag geoTag = new GeoTag()
                {
                    Alt = "",
                    GeoDirection = Direction.something,
                    SessionID = "1.336173859039812",
                    Lat = "17.435943700000000000",
                    Long = "79.121673099999980000",
                    Speed = 10,
                    TimeStamp = DateTime.Now.Ticks,
                    ProfileID = 1,

                };

                //geoTag.MediaContent = 

                System.IO.FileStream _FileStream = new FileStream(@"C:\Users\nabansal\Downloads\navin.jpg", FileMode.Open);
                BinaryReader _BinaryReader = new BinaryReader(_FileStream);
                long _TotalBytes = new FileInfo(@"C:\Users\nabansal\Downloads\navin.jpg").Length;
                geoTag.MediaContent = _BinaryReader.ReadBytes((Int32)_TotalBytes);

                _FileStream.Close();
                _FileStream.Dispose();
                _BinaryReader.Close();

                const string ActivateSosServiceURL = ServiceUrl + "GeoUpdate.svc/PostMyLocation";



                //Serialize the GeoTag class and post the data
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoTag));
                using (MemoryStream mem = new MemoryStream())
                {
                    ser.WriteObject(mem, geoTag);
                    string data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    //data = "{\"Alt\":\"\",\"GeoDirection\":0,\"Identifier\":\"1.228113154639812\",\"Lat\":\"17.435943700000000000\",\"Long\":\"78.341673099999980000\",\"Speed\":20,\"TimeStamp\":\"\\/Date(1362830850860+0530)\\/\"}";

                    WebClient webClient = new WebClient();
                    //webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(sos_completed);
                    webClient.Headers["Content-type"] = "application/json";
                    webClient.Encoding = Encoding.UTF8;
                    Uri uri = new Uri(ActivateSosServiceURL);
                    result = webClient.UploadStringTask(uri, data);
                }

            }
            catch { }
            return result;
        }

        private static void sos_completed(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                string str = e.Error.Message;
            }
        }

        private static string EncryptDecrypt()
        {
            string Data = "+2398020394023940";
            string EncryptedData = SOS.Service.Utility.Security.Encrypt(Data);
            string DecryptedData = SOS.Service.Utility.Security.Decrypt(EncryptedData);
            return DecryptedData;
        }

        private static string Incident()
        {
            Task<string> result = null;
            try
            {
                GeoTag geoTag = new GeoTag()
                {
                    Alt = "",
                    GeoDirection = Direction.something,
                    SessionID = "1.336173859039812",
                    Lat = "17.435943700000000000",
                    Long = "79.121673099999980000",
                    Speed = 10,
                    TimeStamp = DateTime.Now.Ticks,
                    ProfileID = 1,

                };

                //geoTag.MediaContent = 

                System.IO.FileStream _FileStream = new FileStream(@"C:\Users\nabansal\Downloads\navin.jpg", FileMode.Open);
                BinaryReader _BinaryReader = new BinaryReader(_FileStream);
                long _TotalBytes = new FileInfo(@"C:\Users\nabansal\Downloads\navin.jpg").Length;
                geoTag.MediaContent = _BinaryReader.ReadBytes((Int32)_TotalBytes);

                _FileStream.Close();
                _FileStream.Dispose();
                _BinaryReader.Close();

                const string ActivateSosServiceURL = ServiceUrl + "GeoUpdate.svc/ReportTease";

                //Serialize the GeoTag class and post the data
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoTag));
                using (MemoryStream mem = new MemoryStream())
                {
                    ser.WriteObject(mem, geoTag);
                    string data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    //data = "{\"Alt\":\"\",\"GeoDirection\":0,\"Identifier\":\"1.228113154639812\",\"Lat\":\"17.435943700000000000\",\"Long\":\"78.341673099999980000\",\"Speed\":20,\"TimeStamp\":\"\\/Date(1362830850860+0530)\\/\"}";

                    WebClient webClient = new WebClient();
                    //webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(sos_completed);
                    webClient.Headers["Content-type"] = "application/json";
                    webClient.Encoding = Encoding.UTF8;
                    Uri uri = new Uri(ActivateSosServiceURL);
                    result = webClient.UploadStringTask(uri, data);
                }

            }
            catch { }
            return result.ToString();
        }

        #endregion
    }
}

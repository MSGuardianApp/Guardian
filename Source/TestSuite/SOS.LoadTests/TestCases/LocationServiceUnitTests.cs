using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace GuardianLoadTestProject
{
    [TestClass]
    public class LocationServiceUnitTests
    {
        private string BaseUrl = "https://guardiansrvc-dev.cloudapp.net/LocationService.svc/";
        [TestMethod]
        public void LocationService_GetBuddiesToLocateUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetBuddiesToLocate/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56?_12345=" + (new Random()).Next());
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod]
        public void LoadBing()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create("http://www.bing.com");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            //webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            //var str = (webrequest.GetResponse() as HttpWebResponse).StatusCode;
            Assert.AreEqual(HttpStatusCode.OK, HttpStatusCode.OK);
        }
        string path = @"E:\Guardian\LoadTests\3.log";
        [TestMethod]
        public void LocationService_GetMyBuddesUnitTest()
        {
            DateTime startTime = DateTime.Now;
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetMyBuddies/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var str = (webrequest.GetResponse() as HttpWebResponse).StatusCode;
            webrequest.Abort();
            var endTime = DateTime.Now;
            var a = (endTime - startTime).TotalMilliseconds;
            System.IO.File.AppendAllText(path, "R: " + a.ToString() + "\t S: " + startTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\t E: " + endTime.ToString("dd/MM/yyyy HH:mm:ss.fff") + Environment.NewLine);
            
            Assert.AreEqual(HttpStatusCode.OK, str);
        }


        [TestMethod]
        public void LocationService_GetBuddiesToLocateLastLocationUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetBuddiesToLocateLastLocation/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56/abc");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void LocationService_GetSOSTrackCountUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetSOSTrackCount/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56/abc");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void LocationService_GetUserLocationsByTokenUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetUserLocationsByToken/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void LocationService_GetUserLocationsUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetUserLocations/6/" + DateTime.Now.AddSeconds(-2).Ticks);
            webrequest.Timeout = 300000;
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImtpZCI6IjEifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQyNzE5Nzc0OCwidWlkIjoiODY2OTFhYjUwZWEzMWIzY2JhODIxNTRkOTUxZDRlNzMiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.JFoXACFa1JnZatb1Nao2piXZwTTu4DRNPsd5Gu8yY2I");
            var response = webrequest.GetResponse() as HttpWebResponse;
            //StreamReader reader = new StreamReader(response.GetResponseStream());
            //string str = reader.ReadLine();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void LocationService_GetUserLocationArrayUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetUserLocationArray/6/" + DateTime.Now.Ticks);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void LocationService_GetUserLocationUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetUserLocation/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56/" + DateTime.Now.Ticks);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

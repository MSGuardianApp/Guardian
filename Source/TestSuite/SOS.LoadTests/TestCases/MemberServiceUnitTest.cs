using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;
using SOS.Service.Interfaces.DataContracts;
using System.Text;
using System.IO;
using System.Collections.Generic;
using SOS.Service.Interfaces.DataContracts.OutBound;

namespace GuardianLoadTestProject
{
    [TestClass]
    public class MemberServiceUnitTests
    {
        private string BaseUrl = "https://guardiansrvc-dev.cloudapp.net/MembershipService.svc/";

        [TestMethod]
        public void GetProfileByProfileIDUnitTest()
        {
            HttpWebRequest 
                webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetProfileByProfileID/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56");
            webrequest.Timeout = 300*1000;
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetProfileLiteByProfileIDUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetProfileLiteByProfileID/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetProfilesForLiveIDUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "GetProfilesForLiveID");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void UnBuddyUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "UnBuddy/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CreatePhoneValidatorUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "CreatePhoneValidator");
            webrequest.Method = "POST";
            PhoneValidation phoneValidation = new PhoneValidation();
            phoneValidation.AuthenticatedLiveID = "harsh4u89@outlook.com";
            phoneValidation.Name = "Sarita Jain";
            phoneValidation.PhoneNumber = "8106968383";
            phoneValidation.RegionCode = "+91";
            phoneValidation.SecurityToken = "12345";

            string postData = JsonConvert.SerializeObject(phoneValidation);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            Stream dataStream = webrequest.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CreateProfileUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "CreateProfile");
            webrequest.Method = "POST";
            Profile profile = new Profile();
            profile.CanArchive = true;
            profile.CanMail = true;
            profile.CanPost = true;
            profile.CanSMS = true;
            profile.DataInfo = new List<ResultInfo>();
            ResultInfo info = new ResultInfo();
            info.Message = "Hi";
            profile.DataInfo.Add(info);

            profile.Email = "harjain@outlook.com";
            profile.IsSOSOn = true;
            profile.IsTrackingOn = true;
            profile.IsValid = true;
            profile.LiveDetails = new LiveCred();
            profile.LocateBuddies = new List<ProfileLite>();
            profile.LocationConsent = true;
            profile.MobileNumber = "8106968383";
            profile.Name = "Harsh Jain";
            //profile.PhoneSetting = new DeviceSetting();
            //profile.PhoneSetting.CanEmail = true;
            //profile.PhoneSetting.CanSMS = true;
            //profile.PhoneSetting.DeviceID = "123";
            //profile.PhoneSetting.PlatForm="wp8.1";
            profile.RegionCode = "+91";
            profile.UserID = 1;
            profile.ProfileID = 1;

            string postData = JsonConvert.SerializeObject(profile);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            Stream dataStream = webrequest.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [TestMethod]
        public void UpdateProfilePhoneUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "UpdateProfilePhone");
            webrequest.Method = "POST";
            Profile profile = new Profile();
            profile.CanArchive = true;
            profile.CanMail = true;
            profile.CanPost = true;
            profile.CanSMS = true;
            profile.DataInfo = new List<ResultInfo>();
            ResultInfo info = new ResultInfo();
            info.Message = "Hi";
            profile.DataInfo.Add(info);

            profile.Email = "harjain@outlook.com";
            profile.IsSOSOn = true;
            profile.IsTrackingOn = true;
            profile.IsValid = true;
            profile.LiveDetails = new LiveCred();
            profile.LocateBuddies = new List<ProfileLite>();
            profile.LocationConsent = true;
            profile.MobileNumber = "8106968383";
            profile.Name = "Harsh Jain";
            //profile.PhoneSetting = new DeviceSetting();
            //profile.PhoneSetting.CanEmail = true;
            //profile.PhoneSetting.CanSMS = true;
            //profile.PhoneSetting.DeviceID = "123";
            //profile.PhoneSetting.PlatForm="wp8.1";
            profile.RegionCode = "+91";
            profile.UserID = 1;
            profile.ProfileID = 1;

            string postData = JsonConvert.SerializeObject(profile);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            Stream dataStream = webrequest.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void UpdateProfileUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "UpdateProfile");
            webrequest.Method = "POST";
            Profile profile = new Profile();
            profile.CanArchive = true;
            profile.CanMail = true;
            profile.CanPost = true;
            profile.CanSMS = true;
            profile.DataInfo = new List<ResultInfo>();
            ResultInfo info = new ResultInfo();
            info.Message = "Hi";
            profile.DataInfo.Add(info);

            profile.Email = "harjain@outlook.com";
            profile.IsSOSOn = true;
            profile.IsTrackingOn = true;
            profile.IsValid = true;
            profile.LiveDetails = new LiveCred();
            profile.LocateBuddies = new List<ProfileLite>();
            profile.LocationConsent = true;
            profile.MobileNumber = "8106968383";
            profile.Name = "Harsh Jain";
            //profile.PhoneSetting = new DeviceSetting();
            //profile.PhoneSetting.CanEmail = true;
            //profile.PhoneSetting.CanSMS = true;
            //profile.PhoneSetting.DeviceID = "123";
            //profile.PhoneSetting.PlatForm="wp8.1";
            profile.RegionCode = "+91";
            profile.UserID = 1;
            profile.ProfileID = 1;

            string postData = JsonConvert.SerializeObject(profile);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            Stream dataStream = webrequest.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CheckPendingUpdatesUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "CheckPendingUpdates/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56/" + DateTime.Now.Ticks);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CheckPendingUpdatesNoCacheUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "CheckPendingUpdates/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56/" + DateTime.Now.Ticks + "/" + DateTime.Now.Ticks);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void UnRegisterUserUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "UnRegisterUser");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void SubscribeBuddyActionUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "SubscribeBuddyAction/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56//7a7078ea9439d2f7ed0cf35cf0ffa167/1");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [TestMethod]
        public void UnAssignMarshalFromListTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "UnAssignMarshalFromList/4fcb79ed-bcdb-4a2d-afe0-9d8371df1b56//7a7078ea9439d2f7ed0cf35cf0ffa167/1");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            webrequest.Headers.Add("LiveUserID", "7a7078ea9439d2f7ed0cf35cf0ffa167");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


    }
}

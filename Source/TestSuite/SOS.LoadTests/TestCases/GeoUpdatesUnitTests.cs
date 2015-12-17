using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOS.Service.Interfaces.DataContracts;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using model = SOS.Model;
using SOS.ConfigManager;
using System.Data;
using System.Data.SqlClient;

namespace GuardianLoadTestProject
{
    [TestClass]
    public class GeoUpdatesUnitTests
    {
        string BaseUrl = "https://guardiansrvc-dev.cloudapp.net/GeoUpdate.svc/";
        [TestMethod]
        public void GeoUpdates_PostMyLocationUnitTest()
        {
            try
            {
                GeoTags geoTag = new GeoTags
                {
                    Id = "3a9bbae1-f0ef-4658-a3f8-14546be6fa0c",
                    PID = 6,
                    Lat = new string[] { "17.435943700000000000", "17.435943700000000000", "17.435943700000000000", "17.435943700000000000" },
                    Long = new string[] { "79.121673099999980000", "79.121673099999980000", "79.121673099999980000", "79.121673099999980000" },
                    TS = new long[] { DateTime.Now.AddMilliseconds(1).Ticks, DateTime.Now.AddMilliseconds(2).Ticks, DateTime.Now.AddMilliseconds(3).Ticks, DateTime.Now.AddMilliseconds(4).Ticks },
                    IsSOS = new bool[] { true, true, true, true },
                    LocCnt = 4,
                    Alt = new string[]{null,null,null,null},
                    Spd = new int[] { 0, 10, 20, 30 }
                };

                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "PostMyLocation");
                webrequest.Method = "POST";
             
                string postData = JsonConvert.SerializeObject(geoTag);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
              
                Stream dataStream = webrequest.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();               
                webrequest.ContentType = "application/json";                
                webrequest.Headers.Add("AuthID", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImtpZCI6IjEifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQyNzE5Nzc0OCwidWlkIjoiODY2OTFhYjUwZWEzMWIzY2JhODIxNTRkOTUxZDRlNzMiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.JFoXACFa1JnZatb1Nao2piXZwTTu4DRNPsd5Gu8yY2I");
                
                var response = webrequest.GetResponse() as HttpWebResponse;
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            catch (Exception ex) { }
        }

        [TestMethod]
        public void GeoUpdates_PostLocationWithMediaUnitTest()
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
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "PostLocationWithMedia");
            webrequest.Method = "POST";

            string postData = JsonConvert.SerializeObject(geoTag);
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
        public void GeoUpdates_StopPostingsUnitTest()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "StopPostings/0ab7d27c-74d3-427f-9760-c692b7002365/0/" + DateTime.Now.Ticks.ToString());
            webrequest.Method = "POST";
            webrequest.ContentType = "application/json";
            webrequest.ContentLength = 0;
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //[TestMethod]
        //public void GeoUpdates_StopAllPostingsUnitTest()
        //{
        //    HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "StopAllPostings/2/" + DateTime.Now.Ticks.ToString());
        //    webrequest.Method = "POST";
        //    webrequest.ContentType = "application/json";
        //    webrequest.ContentLength = 0;
        //    webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
        //    var response = webrequest.GetResponse() as HttpWebResponse;
        //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //}

        [TestMethod]
        public void GeoUpdates_ReportTeaseUnitTest()
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
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "ReportTease");
            webrequest.Method = "POST";

            string postData = JsonConvert.SerializeObject(geoTag);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            Stream dataStream = webrequest.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            webrequest.ContentType = "application/json";
            webrequest.Headers.Add("AuthID", "eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQwOTU4OTQ4NywidWlkIjoiZjJhYTc2MTJjYTA1MGU1NmE4ODY5NDI0NGNlYmE2ZmEiLCJhdWQiOiJndWFyZGlhbnBvcnRhbC1kZXYuY2xvdWRhcHAubmV0IiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDAxMEE2MjciLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0MDEwQTYyNyJ9.zcpf0Y5HoAggrhx0x7Gq_x_TE69YOSkCAjzlFqKIpC0");
            var response = webrequest.GetResponse() as HttpWebResponse;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

    
        private void InsertUsersAndProfilesUsingSQLClient()
        {
            string cs = Config.AzureSQLConnectionString;
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                con = new SqlConnection(cs);
                for (int index = 0; index < 500000; index++)
                {
                    model.User user = new model.User()
                    {

                        UserID = index + 1,
                        Name = "Guardian" + index,
                        Email = string.Format("Guardian{0}@live.com", index),
                        MobileNumber = SOS.Service.Utility.Security.Encrypt("123456768"),
                        LastModifiedBy = "User" + index,
                        LastModifiedDate = DateTime.Now,


                    };
                 
                    cmd = new SqlCommand("RadomUserInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = user.Name;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = user.Email;
                    cmd.Parameters.Add("@MobileNumber", SqlDbType.NVarChar).Value = user.MobileNumber;
                    cmd.Parameters.Add("@FBAuthID", SqlDbType.NVarChar).Value = user.FBAuthID;
                    cmd.Parameters.Add("@FBID", SqlDbType.NVarChar).Value = user.FBID;
                    cmd.Parameters.Add("@LiveID", SqlDbType.NVarChar).Value = user.LiveID;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = user.CreatedBy;
                    cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = user.CreatedDate;
                    cmd.Parameters.Add("@LastModifiedDate", SqlDbType.DateTime).Value = user.LastModifiedDate;
                    cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = user.LastModifiedBy;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    
//                    cmd = new SqlCommand(@"INSERT INTO  [dbo].[User] (Name,Email ,MobileNumber,FBAuthID,FBID,LiveID,LiveAuthID,LiveAccessToken,LiveRefreshToken,CreatedBy,CreatedDate,LastModifiedDate,LastModifiedBy)
//                                                           VALUES('" + user.Name + "','" + user.Email + "','" + user.MobileNumber + "','" + user.FBAuthID + "','" + user.FBID
//                                                                     + "','" + user.LiveID + "','" + user.LiveAuthID + "','" + user.LiveAccessToken + "','" + user.LiveRefreshToken
//                                                                     + "','" + user.CreatedBy + "','" + user.CreatedDate + "','" + user.LastModifiedBy + "','" + user.LastModifiedDate+ "')", con);                   

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { }

            finally
            {
                con.Close();

            }

            try
            {
                con = new SqlConnection(cs);
                for (int index = 0; index < 100000; index++)
                {
                    model.Profile profile = new model.Profile()
                    {
                        UserID = 34134132434,
                        ProfileID = 5,
                        MobileNumber = "123456788",
                        CanEmail = false,
                        CanPost = false,
                        IsValid = false,
                        RegionCode = "+91",
                        CanSMS = false,
                        LocationConsent = false,
                        LastModifiedBy = "ramg",
                        LastModifiedDate = DateTime.Now,
                    };

                    cmd = new SqlCommand("RadomProfileInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@UserID", SqlDbType.BigInt).Value = profile.UserID;
                    cmd.Parameters.Add("@MobileNumber", SqlDbType.NVarChar).Value = profile.MobileNumber;
                    cmd.Parameters.Add("@RegionCode", SqlDbType.NVarChar).Value = profile.RegionCode;
                    cmd.Parameters.Add("@DeviceID", SqlDbType.NVarChar).Value = profile.DeviceID;
                    cmd.Parameters.Add("@DeviceType", SqlDbType.Int).Value = profile.DeviceType;
                    cmd.Parameters.Add("@FBGroup", SqlDbType.NVarChar).Value = profile.FBGroup;
                    cmd.Parameters.Add("@FBGroupID", SqlDbType.NVarChar).Value = profile.FBGroupID;
                    cmd.Parameters.Add("@CanPost", SqlDbType.Bit).Value = profile.CanPost;
                    cmd.Parameters.Add("@CanSMS", SqlDbType.Bit).Value = profile.CanSMS;
                    cmd.Parameters.Add("@CanEmail", SqlDbType.Bit).Value = profile.CanEmail;
                    cmd.Parameters.Add("@SecurityToken", SqlDbType.NVarChar).Value = profile.SecurityToken;
                    cmd.Parameters.Add("@LocationConsent", SqlDbType.Bit).Value = profile.LocationConsent;
                    cmd.Parameters.Add("@IsValid", SqlDbType.Bit).Value = profile.IsValid;
                    cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = profile.CreatedDate;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = profile.CreatedBy;
                    cmd.Parameters.Add("@LastModifiedDate", SqlDbType.DateTime).Value = profile.LastModifiedDate;
                    cmd.Parameters.Add("@LastModifiedBy", SqlDbType.NVarChar).Value = profile.LastModifiedBy;


                    con.Open();
                    cmd.ExecuteNonQuery();

                    //                    cmd = new SqlCommand(@"INSERT INTO  [dbo].[User] (Name,Email ,MobileNumber,FBAuthID,FBID,LiveID,LiveAuthID,LiveAccessToken,LiveRefreshToken,CreatedBy,CreatedDate,LastModifiedDate,LastModifiedBy)
                    //                                                           VALUES('" + user.Name + "','" + user.Email + "','" + user.MobileNumber + "','" + user.FBAuthID + "','" + user.FBID
                    //                                                                     + "','" + user.LiveID + "','" + user.LiveAuthID + "','" + user.LiveAccessToken + "','" + user.LiveRefreshToken
                    //                                                                     + "','" + user.CreatedBy + "','" + user.CreatedDate + "','" + user.LastModifiedBy + "','" + user.LastModifiedDate+ "')", con);                   

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { }

            finally
            {
                con.Close();

            }

        }


        [TestMethod]
        public void GeoUpdates_SwitchOnSOSviaSMSUnitTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GeoUpdates_GetIncidentsUnitTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetIncidentsbyDatesUnitTest()
        {
            throw new NotImplementedException();
        }
    }
}

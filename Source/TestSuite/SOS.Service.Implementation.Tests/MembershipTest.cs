using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOS.Service.Interfaces.DataContracts;
using inbound = SOS.Service.Interfaces.DataContracts.InBound;
using SOS.Service.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ImplementationTest
{
    [TestClass]
    public class MembershipTest
    {
        [TestMethod]
        public void SaveProfileBasicInfo()
        {
            System.Collections.Generic.List<SOS.Service.Interfaces.DataContracts.InBound.ProfileBuddyInfo> Buddies = new System.Collections.Generic.List<SOS.Service.Interfaces.DataContracts.InBound.ProfileBuddyInfo>();
            Buddies.Add(new inbound.ProfileBuddyInfo() { Name = "venkat", MobileNumber = "11111", Email = "vrreddy@live.com" });
            Buddies.Add(new inbound.ProfileBuddyInfo() { Name = "rajesh", MobileNumber = "22222", Email = "rajesh@live.com" });
            Buddies.Add(new inbound.ProfileBuddyInfo() { Name = "renu", MobileNumber = "33333", Email = "renu@live.com" });
            Buddies.Add(new inbound.ProfileBuddyInfo() { Name = "dad", MobileNumber = "44444", Email = "dad@live.com" });
            inbound.ProfileBasicInfo pbi = new inbound.ProfileBasicInfo()
            {
                Buddies = Buddies,
                Email = "sharath@live.com",
                MobileNumber = "7702690004",
                Name = "sharath",
                PrimeBuddy = new inbound.ProfileBuddyInfo() { Name = "renu", MobileNumber = "33333", Email = "renu@live.com" },
            };
            Assert.Equals(pbi, new inbound.ProfileBuddyInfo());
            MemberService ms = new MemberService();


            try
            {
                //ms.SaveProfileBasicInfo(pbi);
            }
            catch (IndexOutOfRangeException ioex)
            {

            }
            catch (Exception ex) { }

            System.Collections.Generic.List<SOS.Service.Interfaces.DataContracts.InBound.ProfileBuddyInfo> Buddies1 = new System.Collections.Generic.List<SOS.Service.Interfaces.DataContracts.InBound.ProfileBuddyInfo>();
            Buddies1.Add(new inbound.ProfileBuddyInfo() { Name = "venkat", MobileNumber = "11111", Email = "vrreddy@live.com" });
            Buddies1.Add(new inbound.ProfileBuddyInfo() { Name = "sharath", MobileNumber = "7702690004", Email = "sharath@live.com" });
            Buddies1.Add(new inbound.ProfileBuddyInfo() { Name = "renu", MobileNumber = "33333", Email = "renu@live.com" });
            Buddies1.Add(new inbound.ProfileBuddyInfo() { Name = "aarthi", MobileNumber = "55555", Email = "aarthi@live.com" });
            inbound.ProfileBasicInfo pbi1 = new inbound.ProfileBasicInfo()
            {
                Buddies = Buddies1,
                Email = "rajesh@live.com",
                MobileNumber = "22222",
                Name = "rajesh",
                PrimeBuddy = new inbound.ProfileBuddyInfo() { Name = "aarthi", MobileNumber = "55555", Email = "aarthi@live.com" },
            };

            // ms.SaveProfileBasicInfo(pbi1);
        }

        [TestMethod]
        public void PhoneValidTest()
        {
            MemberService MS = new MemberService();

            PhoneValidation ph = new PhoneValidation()
            {
                AuthenticatedLiveID = "chandran.sharath@live.com",
                Name = "sharath",
                RegionCode = "+91",
                PhoneNumber = "+917702690004"
            };

            MS.CreatePhoneValidator(ph);
        }

        [TestMethod]
        public void CreateBareProfileTest()
        {
            MemberService MS = new MemberService();

            Profile Pr = new Profile()
            {
                Name = "RamServiceTest",
                RegionCode = "+91",
                MobileNumber = "+9999999999",
                SecurityToken = "53184",
                LiveDetails = new LiveCred()
                {
                    LiveID = "ram.service@live.com"
                }

            };

            Task.Run(async () =>
            {
                await MS.CreateProfile(Pr);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void CreateProfileUnitTest()
        {
            Profile profile = new Profile();
            profile.CanArchive = true;
            profile.CanMail = true;
            profile.CanPost = true;
            profile.CanSMS = true;

            profile.Email = "ram.service@outlook.com";
            profile.IsSOSOn = true;
            profile.IsTrackingOn = true;
            profile.IsValid = true;
            profile.LiveDetails = new LiveCred();
            profile.LocateBuddies = new List<ProfileLite>();
            profile.LocationConsent = true;
            profile.MobileNumber = "8989898989";
            profile.Name = "Ram Service";
            profile.PhoneSetting = new DeviceSetting();
            profile.PhoneSetting.CanEmail = true;
            profile.PhoneSetting.CanSMS = true;
            profile.PhoneSetting.DeviceID = "123";
            profile.PhoneSetting.PlatForm = "wp8.1";
            profile.RegionCode = "+91";
            profile.UserID = 4;
            //profile.ProfileID = 3;

            MemberService MS = new MemberService();
            var res = MS.CreateProfile(profile).Result;
            Assert.AreEqual("Created.", res.DataInfo[0].Message);
        }

        [TestMethod]
        public void CreateProfileUnitTestWithBuddies()
        {
            Profile profile = new Profile();
            profile.CanArchive = true;
            profile.CanMail = true;
            profile.CanPost = true;
            profile.CanSMS = true;

            profile.Email = "ramgopal.service@outlook.com";
            profile.IsSOSOn = true;
            profile.IsTrackingOn = true;
            profile.IsValid = true;
            profile.LiveDetails = new LiveCred();
            profile.LocateBuddies = new List<ProfileLite>();
            profile.LocationConsent = true;
            profile.MobileNumber = "3453245234";
            profile.Name = "Ram Service";
            profile.PhoneSetting = new DeviceSetting();
            profile.PhoneSetting.CanEmail = true;
            profile.PhoneSetting.CanSMS = true;
            profile.PhoneSetting.DeviceID = "123";
            profile.PhoneSetting.PlatForm = "wp8.1";
            profile.RegionCode = "+91";
            //profile.UserID = 4;
            //profile.ProfileID = 3;

            profile.MyBuddies = new List<Buddy>();
            profile.MyBuddies.Add(new Buddy()
            {
                Name = "venkat",
                MobileNumber = "5245245245",
                Email = "ram@live.com"
            });

            profile.MyBuddies.Add(new Buddy()
            {
                Name = "gopal",
                MobileNumber = "874598475028",
                Email = "gopal@live.com"
            });

            profile.MyBuddies.Add(new Buddy()
            {
                Name = "reddy",
                MobileNumber = "874598475028",
                Email = "reddy@live.com"
            });

            MemberService MS = new MemberService();
            var res = MS.CreateProfile(profile).Result;
            Assert.AreEqual("Created.", res.DataInfo[0].Message);
        }

        [TestMethod]
        public void CreateProfileUnitTestWithBuddiesAndGroups()
        {
            Profile profile = new Profile();
            profile.CanArchive = true;
            profile.CanMail = true;
            profile.CanPost = true;
            profile.CanSMS = true;

            profile.Email = "reddy.ramservice@outlook.com";
            profile.IsSOSOn = true;
            profile.IsTrackingOn = true;
            profile.IsValid = true;
            profile.LiveDetails = new LiveCred();
            profile.LocateBuddies = new List<ProfileLite>();
            profile.LocationConsent = true;
            profile.MobileNumber = "111222233333";
            profile.Name = "reddys Service";
            profile.PhoneSetting = new DeviceSetting();
            profile.PhoneSetting.CanEmail = true;
            profile.PhoneSetting.CanSMS = true;
            profile.PhoneSetting.DeviceID = "123";
            profile.PhoneSetting.PlatForm = "wp8.1";
            profile.RegionCode = "+91";
            //profile.UserID = 4;
            //profile.ProfileID = 3;

            profile.MyBuddies = new List<Buddy>();
            profile.MyBuddies.Add(new Buddy()
            {
                Name = "arjun",
                MobileNumber = "12341341",
                Email = "arjun@live.com"
            });

            profile.MyBuddies.Add(new Buddy()
            {
                Name = "chiru",
                MobileNumber = "223433423223",
                Email = "chiru@live.com"
            });

            profile.MyBuddies.Add(new Buddy()
            {
                Name = "Charan",
                MobileNumber = "34341343431134",
                Email = "mahesh@live.com"
            });


            List<Group> GrpList = new List<Group>();

            GrpList.Add(new Group()
            {
                GroupID = "1",
                EnrollmentType = Enrollment.AutoOrgMail,
                EnrollmentValue = "ramgroup@microsoft.com",
                GroupName = "SOCHYD",
                EnrollmentKey = "@microsoft.com",
                ToRemove = false
            });
            GrpList.Add(new Group()
            {
                GroupID = "1",
                EnrollmentType = Enrollment.AutoOrgMail,
                EnrollmentValue = "gopalgroup@microsoft.com",
                GroupName = "SOCHYD",
                EnrollmentKey = "@microsoft.com",
                ToRemove = false
            });
            GrpList.Add(new Group()
            {
                GroupID = "1",
                EnrollmentType = Enrollment.AutoOrgMail,
                EnrollmentValue = "reddygroup@microsoft.com",
                GroupName = "SOCHYD",
                EnrollmentKey = "@microsoft.com",
                ToRemove = false
            });
            profile.AscGroups = GrpList;

            MemberService MS = new MemberService();
            var res = MS.CreateProfile(profile).Result;
            Assert.AreEqual("Created.", res.DataInfo[0].Message);
        }

        [TestMethod]
        public void UpdateProfileUnitTest()
        {
            Profile profile = new Profile();
            profile.CanArchive = true;
            profile.CanMail = true;
            profile.CanPost = true;
            profile.CanSMS = true;

            profile.Email = "ramgopal.service@outlook.com";
            profile.IsSOSOn = true;
            profile.IsTrackingOn = true;
            profile.IsValid = true;
            profile.LiveDetails = new LiveCred();
            profile.LocateBuddies = new List<ProfileLite>();
            profile.LocationConsent = true;
            profile.MobileNumber = "3453245234";
            profile.Name = "reddy Service";
            profile.PhoneSetting = new DeviceSetting();
            profile.PhoneSetting.CanEmail = true;
            profile.PhoneSetting.CanSMS = true;
            profile.PhoneSetting.DeviceID = "123";
            profile.PhoneSetting.PlatForm = "wp8.1";
            profile.RegionCode = "+91";
            profile.UserID = 1341341;
            profile.ProfileID = 134134134;


            MemberService MS = new MemberService();
            var res = MS.UpdateProfile(profile).Result;
            Assert.AreEqual("Profile Updated Successfully.", res.DataInfo[0].Message);
        }

        [TestMethod]
        public void UpdateProfileWithGroup()
        {
            MemberService MS = new MemberService();
            List<Group> GrpList = new List<Group>();
            GrpList.Add(new Group() { GroupID = "1", EnrollmentType = Enrollment.AutoOrgMail, EnrollmentValue = "sharathr@microsoft.com", GroupName = "SOCHYD", EnrollmentKey = "@microsoft.com", ToRemove = true });
            Profile Pr = new Profile()
            {
                ProfileID = 100,
                Name = "ram group",
                RegionCode = "+91",
                MobileNumber = "3413413241",
                // SecurityToken = "20279",
                LiveDetails = new LiveCred()
                {
                    LiveID = "ram.group@live.com"
                },
                AscGroups = GrpList
            };

            //var test = MS.UpdateProfile(Pr);
            var test = MS.UpdateProfile(Pr);
        }

        [TestMethod]
        public void UpdateProfileToAddGroup()
        {
            MemberService MS = new MemberService();
            List<Group> GrpList = new List<Group>();
            GrpList.Add(new Group() { GroupID = "7", EnrollmentType = Enrollment.None, EnrollmentValue = "guardiandevdemo@hotmail.com", GroupName = "Guardian Dev Demo", EnrollmentKey = "@microsoft.com", ToRemove = false });
            Profile Pr = new Profile()
            {
                ProfileID = 4,
                Name = "Ramgopal Reddy",
                RegionCode = "+91",
                MobileNumber = "3413413241",
                Email = "ramgopalreddy.r@hotmail.com",
                // SecurityToken = "20279",
                LiveDetails = new LiveCred()
                {
                    LiveID = "ram.group@live.com"
                },
                AscGroups = GrpList
            };

            //var test = MS.UpdateProfile(Pr);
            var test = MS.UpdateProfile(Pr).Result;
            Assert.AreEqual(4, test.ProfileID);
        }

        [TestMethod]
        public void UpdateProfileToRemoveGroup()
        {
            MemberService MS = new MemberService();
            List<Group> GrpList = new List<Group>();
            GrpList.Add(new Group()
            {
                GroupID = "7",
                EnrollmentType = Enrollment.None,
                EnrollmentValue = "guardiandevdemo@hotmail.com",
                GroupName = "Guardian Dev Demo",
                EnrollmentKey = "@microsoft.com",
                ToRemove = true
            });
            Profile Pr = new Profile()
            {
                ProfileID = 4,
                Name = "Ramgopal Reddy",
                RegionCode = "+91",
                MobileNumber = "3413413241",
                Email = "ramgopalreddy.r@hotmail.com",
                // SecurityToken = "20279",
                LiveDetails = new LiveCred()
                {
                    LiveID = "ram.group@live.com"
                },
                AscGroups = GrpList
            };

            //var test = MS.UpdateProfile(Pr);
            var test = MS.UpdateProfile(Pr).Result;
            Assert.AreEqual(4, test.ProfileID);
        }

        [TestMethod]
        public void UnregisterUserTest()
        {
            MemberService MS = new MemberService();
            var res = MS.UnRegisterUser().Result;
            Assert.AreEqual(0, Convert.ToInt32(res.ResultType));
        }
        [TestMethod]
        public void UpdateProfileWithBuddy()
        {
            MemberService MS = new MemberService();
            Profile Pr = new Profile()
            {
                ProfileID = 10014,
                UserID = 0,
                Name = "krishna",
                RegionCode = "+91",
                MobileNumber = "+3413413",
                //SecurityToken = "20279",
                LiveDetails = new LiveCred()
                {
                    LiveID = "krishna@live.com"
                },
                MyBuddies = new List<Buddy>()
            };

            Pr.MyBuddies.Add(new Buddy()
            {
                Name = "krish",
                MobileNumber = "542452525",
                Email = "krish@live.com"
            });

            var test = MS.UpdateProfile(Pr).Result;
        }

        [TestMethod]
        public void GetProfilesForLiveTest()
        {
            MemberService MS = new MemberService();

            var rs = MS.GetProfilesForLiveID();
        }

        [TestMethod]
        public void GetProfileByProfileIDTest()
        {
            MemberService MS = new MemberService();
            var res = MS.GetProfileByProfileID("3").Result;
            Assert.AreEqual(3, res.ProfileID);
        }

        [TestMethod]
        public void GetProfilesForLiveIDTest()
        {
            MemberService MS = new MemberService();
            var res = MS.GetProfilesForLiveIDAsync("ramgopal").Result;
            Assert.AreEqual(3, res.List[0].ProfileID);
        }

        [TestMethod]
        public void GetPendingUpdatesTest()
        {
            MemberService MS = new MemberService();

            HealthUpdate update = MS.CheckPendingUpdates("4", DateTime.Now.AddDays(-20).ToString(), DateTime.Now.Ticks.ToString()).Result;
            Assert.AreEqual(false, update.IsProfileActive);
        }
    }
}

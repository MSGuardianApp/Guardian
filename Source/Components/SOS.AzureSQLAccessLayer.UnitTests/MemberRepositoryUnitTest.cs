using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SOS.Model;
using SOS.Service.Utility;

namespace SOS.AzureSQLAccessLayer.UnitTests
{
    [TestClass]
    public class MemberRepositoryUnitTest
    {
        [TestMethod]
        public void SaveOrUpdateUserTest()
        {
            using (MemberRepository MP = new MemberRepository())
            {
                User user = new User();
                user.UserID = 7;
                user.Name = "Gopal";
                user.Email = "gopal@live.com";
                user.MobileNumber = Security.Encrypt("123456768");
                user.LastModifiedBy = "ramgopal";
                user.LastModifiedDate = DateTime.Now;

                Task.Run(async () =>
                {
                    await MP.SaveUserAsync(user);
                }).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public void SaveOrUpdateProfileTest()
        {
            using (MemberRepository MP = new MemberRepository())
            {
                Profile profile = new Profile();
                profile.UserID = 34134132434;
                profile.ProfileID = 5;
                profile.MobileNumber = "123456788";
                profile.CanEmail = false;
                profile.CanPost = false;
                profile.IsValid = false;
                profile.RegionCode = "+91";
                profile.CanSMS = false;
                profile.LocationConsent = false;
                profile.LastModifiedBy = "ramg";
                profile.LastModifiedDate = DateTime.Now;

                Task.Run(async () =>
                {
                    await MP.SaveOrUpdateProfileAsync(profile);
                }).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public void GetUserByMailIDTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                User user = MR.GetUserByMailIDAsync("ramg@live.com").Result;
                Assert.AreEqual("ramg@live.com", user.Email);
            }
        }

        [TestMethod]
        public void GetProfileByMobileTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                Profile profile = MR.GetProfileByMobileAsync("12345").Result;
                Assert.AreEqual(3, profile.ProfileID);
            }
        }

        [TestMethod]
        public void GetProfileTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                Profile profile = MR.GetProfileAsync(1).Result;
                Assert.AreEqual(1, profile.ProfileID);
            }
        }

        [TestMethod]
        public void GetUserTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                User user = MR.GetUserByUserIDAsync(1).Result;
                string mno = Security.Decrypt(user.MobileNumber);
                Assert.AreEqual(1, user.UserID);
            }
        }

        [TestMethod]
        public void GetBuddiesForProfileIDTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                List<Buddy> buddies = MR.GetBuddiesForProfileIDAsync(3).Result;
                Assert.AreEqual(4, buddies[0].BuddyID);
            }
        }

        [TestMethod]
        public void GetBuddyProfilesForUserIDTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                List<Buddy> buddies = MR.GetBuddyProfilesForUserIDAsync(4).Result;
                Assert.AreEqual(5, buddies[0].ProfileID);
            }
        }

        [TestMethod]
        public void AddBuddyTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                Buddy buddy = new Buddy();
                buddy.ProfileID = 5;
                buddy.UserID = 4;
                buddy.BuddyName = "ramgopal";
                buddy.MobileNumber = Security.Encrypt("123456789");
                buddy.Email = "ramgopal@live.com";
                buddy.IsPrimeBuddy = true;
                buddy.State = BuddyState.Active;
                buddy.CreatedBy = "hey gopala";
                buddy.CreatedDate = DateTime.Now;

                Task.Run(async () =>
                {
                    await MR.AddBuddyAsync(buddy);
                }).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public void DeleteWhileUnregisterUseTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                Task.Run(async () =>
                {
                    await MR.DeleteWhileUnregisterUserAsync(10011);
                }).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public void RemoveBuddyRelationTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                Task.Run(async () =>
                {
                    await MR.RemoveBuddyRelationAsync(5, 4);
                }).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public void GetPendingUpdatesTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                HealthUpdate update = MR.GetPendingUpdatesAsync(3).Result;
                Assert.AreEqual(false, update.IsProfileActive);
            }
        }

        [TestMethod]
        public void SubscribeBuddyForProfileAction()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                User usr = MR.SubscribeBuddyForProfileActionAsync(5, 4, 0, "86000823-38f2-4df3-9c38-6de1944c4d5d").Result;
                Assert.AreEqual("Gopal", usr.Name);
            }
        }

        [TestMethod]
        public void SubscribeBuddyForProfileAction2()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                User usr = MR.SubscribeBuddyForProfileActionAsync(12341341341415, 413413411341, 0, "86000823-38f2-4df3-9c38-6de1944c4d5d").Result;
                Assert.IsNull(usr);
            }
        }

        [TestMethod]
        public void GetGroupMembershipForProfileTest()
        {
            using (GroupRepository MR = new GroupRepository())
            {
                List<GroupMembership> grp = MR.GetGroupMembershipForProfile(5).Result;
                Assert.AreEqual(9, grp[0].GroupID);
            }
        }

        [TestMethod]
        public void UpdateGroupMembershipTest()
        {
            using (GroupRepository GR = new GroupRepository())
            {
                GR.UpdateGroupMembership(123121, 3131231223).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public void GetMembersByNameFromGroupIDTest()
        {
            using (MemberRepository MR = new MemberRepository())
            {
                List<LiveUserStatus> grp = MR.GetFilteredGroupMembers(1,"R").Result;
                Assert.AreEqual("Ramgopal Reddy", grp[0].Name);
            }
        }

        [TestMethod]
        public void GetMarshalProfilesByGroupIDTest()
        {
            using (GroupRepository GR = new GroupRepository())
            {
                List<Profile> prfs = GR.GetLiveMarshalsByGroupID(1).Result;
                Assert.AreEqual("Ramgopal Reddy", prfs[0].User.Name);
            }
        }

        [TestMethod]
        public void ValidateAndSaveMarshalTest()
        {
            using (GroupRepository GR = new GroupRepository())
            {
                SOS.AzureSQLAccessLayer.Entities.MarshalStatusInfo marshalinfo = GR.ValidateAndSaveMarshal(1, "ram@live.com", "234234234", false).Result;
                Assert.AreEqual(1, marshalinfo.Code);
            }
        }

    }
}

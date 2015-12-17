using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOS.Model;
using System.Data.Entity;

namespace SOS.AzureSQLAccessLayer.UnitTests
{
    [TestClass]
    public class ContextUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var ctx = new GuardianContext())
            {

                User u = new User();
                u.Email = "vrreddy.d@hotmail.com";
                u.Name = "VR";
                ctx.Users.Add(u);
                ctx.SaveChanges();

                Profile pr = new Profile();
                pr.User = u;
                pr.RegionCode = "+91";
                ctx.Profiles.Add(pr);

                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void DeleteUsingEFTest()
        {
            using (var ctx = new GuardianContext())
            {
                //User u = new User() { Name = "VR" };//Not working
                User u = new User() { UserID=2 };// working
                ctx.Entry(u).State = EntityState.Deleted;
                ctx.SaveChanges();

            }
        }
    }
}

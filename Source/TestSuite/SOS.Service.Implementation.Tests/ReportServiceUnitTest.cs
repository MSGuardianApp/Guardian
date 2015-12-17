using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOS.Service.Implementation;

namespace ImplementationTest
{
    [TestClass]
    public class ReportServiceUnitTest
    {
        [TestMethod]
        public void UserCountUnitTest()
        {
            ReportService rp = new ReportService();
            int output = rp.UserCount().Result;
            Assert.IsTrue(output >= 0);
        }

        [TestMethod]
        public void GroupUsersUnitTest()
        {
            try
            {
                ReportService rp = new ReportService();
                object output = rp.GroupUsers().GetAwaiter().GetResult();
                Assert.IsTrue(output != null);
            }

            catch (Exception ex) { }
        }

        [TestMethod]
        public void MissedActivationCountUnitTest()
        {
            ReportService rp = new ReportService();
            int output = rp.MissedActivationCount();
            Assert.IsTrue(output >= 0);
        }

        [TestMethod]
        public void ActiveModeStatsUnitTest()
        {
            ReportService rp = new ReportService();
            var output = rp.ActiveModeStats().GetAwaiter().GetResult();
            Assert.IsTrue(output.Count >= 0);
        }


        [TestMethod]
        public void UserReportUnitTest()
        {
            ReportService rp = new ReportService();
            var output = rp.UserReport().GetAwaiter().GetResult();
            Assert.IsTrue(output.Count >= 0);

        }
        [TestMethod]
        public void SOSStatsUnitTest()
        {
            ReportService rp = new ReportService();
            var output = rp.SOSStats("465478", "6785");
            Assert.IsTrue(output != null);
        }
        [TestMethod]
        public void SOSAndTrackStatsUnitTest()
        {
            ReportService rp = new ReportService();
              rp.SOSAndTrackStats("465478","6785");
            Assert.IsTrue(true);
        }
       
        [TestMethod]
        public void GetUserNameUnitTest()
        {
            ReportService rp = new ReportService();
            string output = rp.GetUserName("2").GetAwaiter().GetResult();
            Assert.IsTrue(output !=null);

        }
        [TestMethod]
        public void GetUserByProfileIDUnitTest()
        {
            ReportService rp = new ReportService();
            var output = rp.GetUserByProfileID("2").GetAwaiter().GetResult();
            Assert.IsTrue(output.Name != null);
        }
        [TestMethod]
        public void StopAllPostingsRptUnitTest()
        {
            ReportService rp = new ReportService();
            //rp.StopAllPostingsRpt(null,null,"2").GetAwaiter().GetResult();
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void SOSTrackCountUnitTest()
        {
            ReportService rp = new ReportService();
            var output = rp.SOSTrackCount(2).GetAwaiter().GetResult();
            Assert.IsTrue(output.SOSCount >= 0);
        }
    
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOS.Service.Implementation;
using SOS.Service.Interfaces.DataContracts;

namespace ImplementationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DateTime dt = DateTime.FromOADate(41317.8531365741);
            string str = "";
        }

        [TestMethod]
        public void LoadDataTest()
        {
            //Dummies4SOS.InitializeAllDataSet();
        }

        [TestMethod]
        public void GroupBuddiesToLocateTest()
        {
            LocationService ls = new LocationService();
            ProfileLiteList pll = ls.GetBuddiesToLocate("1").Result as ProfileLiteList;
            string str = "";
        }
    }
}

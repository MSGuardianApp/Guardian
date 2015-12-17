using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOS.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SOS.AzureSQLAccessLayer.UnitTests
{
    [TestClass]
    public class LocationRepositoryUnitTests
    {

        [TestMethod]
        public void RemoveLiveLocationDatawithValidInput()
        {
            long ProfileID = 1;
            using (LocationRepository _locationRep = new LocationRepository())
            {
                Task.Run(async () =>
                {
                    await _locationRep.RemoveLiveLocationData(ProfileID, DateTime.Now.Ticks);
                }).GetAwaiter().GetResult();
            }
        }
        [TestMethod]
        public void RemoveLiveLocationDataForTokenwithValidInput()
        {
            long ProfileID = 3; ;
            string Token = "ind";
            using (LocationRepository _locationRep = new LocationRepository())
            {
                Task.Run(async () =>
                {
                    await _locationRep.RemoveLiveLocationData(ProfileID, DateTime.Now.Ticks, Token);
                }).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public void RemoveLiveLocationDataBareTokenIDwithValidInput()
        {
            long ProfileID = 1;
            string BareToken = "ind";
            using (LocationRepository _locationRep = new LocationRepository())
            {
                Task.Run(async () =>
                    {
                        await _locationRep.RemoveLiveLocationData(ProfileID, DateTime.Now.Ticks, BareToken);
                    }).GetAwaiter().GetResult();
            }
        }

        //[TestMethod]
        //public void RemoveLiveLocationDataBeforeClientTimewithValidInput()
        //{
        //    long BeforeClientTime = 6355987;
        //    using (LocationRepository _locationRep = new LocationRepository())
        //    {
        //        Task.Run(async () =>
        //            {
        //                await _locationRep.RemoveLiveLocationOnTimeStamp(1, BeforeClientTime);
        //            }).GetAwaiter().GetResult();
        //    }

        ////}

        [TestMethod]
        public void GetLocationDataByTokenUnitTest()
        {
            bool _isLocation = false;

            using (LocationRepository _locationRep = new LocationRepository())
            {
                //var x = _locationRep.GetMemberStatusFromLiveInfo(1, 1234567).Result;
                IEnumerable<LiveLocation> locations = _locationRep.GetLocationDataByToken(1, "ind", 6355987).Result;

                while (locations.GetEnumerator().MoveNext())
                {

                    LiveLocation location = locations.GetEnumerator().Current;
                    _isLocation = true;
                }
                Assert.AreEqual(_isLocation, true);
            }
        }



        [TestMethod]
        public void GetLocationDataUnitTest()
        {

            using (LocationRepository _locationRep = new LocationRepository())
            {
                List<LiveLocation> locations = (List<LiveLocation>)_locationRep.GetLocationData(1, 6355987).Result;

                Assert.AreEqual(locations.Count > 0 ? true : false, true);
            }
        }

        [TestMethod]
        public void GetAllLocationDataUnitTest()
        {


            using (LocationRepository _locationRep = new LocationRepository())
            {
                List<LiveLocation> locations = (List<LiveLocation>)_locationRep.GetAllLocationData(6355987).Result;

                Assert.AreEqual(locations.Count > 0 ? true : false, true);
            }
        }

        [TestMethod]
        public void IsSosAlreadyOnUnitTest()
        {

            using (LocationRepository _locationRep = new LocationRepository())
            {
                bool output = _locationRep.IsSosAlreadyOn(1, "ind", 6355987).Result;

                Assert.AreEqual(output, true);
            }
        }



    }
}
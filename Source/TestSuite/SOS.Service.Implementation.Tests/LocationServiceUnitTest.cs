using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOS.Service.Implementation;
using SOS.Model;

namespace ImplementationTest
{
    [TestClass]
    public class LocationServiceUnitTest
    {
        [TestMethod]
        public void PostLiveLocationData()
        {
            LiveLocation loc = new LiveLocation()
            {
                ProfileID=1,
                SessionID="336173859039816",
                IsSOS=true,
                ClientDateTime = DateTime.UtcNow,
                ClientTimeStamp=DateTime.UtcNow.Ticks,
                Long="18.34",
                Lat="12.345"
            };
            LocationService locService = new LocationService();
            locService.PostMyLocation(loc).GetAwaiter().GetResult();

        }
    }
}

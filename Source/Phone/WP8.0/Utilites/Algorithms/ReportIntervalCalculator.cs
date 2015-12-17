using System;

namespace SOS.Phone.Algorithms
{
    /// <summary>
    /// No movement - Do not capture - KeepLive, 5 min
    /// Slow Walking - < 03 km/hr- Avg 00.8 mts/sec - Capture location for every 10 meters - 12 secs - 5 Loc/min
    /// Walking - 03-06 km/hr    - Avg 01.7 mts/sec - Capture location for every 17 meters - 10 secs - 6 Loc/min
    /// Running - 06-15 km/hr    - Avg 04 mts/sec - Capture location for every 32 meters - 08 secs - 8 Loc/min
    /// Cycle   - 15-35 km/hr    - Avg 08 mts/sec - Capture location for every 48 meters - 06 secs - 10 Loc/min 
    /// Bike    - 35-60 km/hr    - Avg 14 mts/sec - Capture location for every 52 meters - 04 secs - 15 Loc/min
    /// Car     - 60-160 km/hr   - Avg 35 mts/sec - Capture location for every 70 meters - 02 secs - 30 Loc/min
    /// Flight  - >160 km/hr     - Mostly, GPS will not be available
    /// </summary>
    public static class ReportIntervalCalculator
    {
        /// <summary>
        /// Calculates next Report interval for GeoLocator to raise PositionChanged event
        /// Purpose of this algorithm is to optimize battery and to reduce traffic to service
        /// to capture minimum, yet to have detailed level of user movements 
        /// </summary>
        /// <param name="prevSpeed">Speed at which previous Location was captured</param>
        /// <param name="newSpeed">Speed at which new Location is captured</param>
        /// <param name="timeDiffBtwLocations">Time difference between prev location and new location, in secs</param>
        /// <param name="distanceBtwLocations">Distance between prev location and new location, in meters</param>
        /// <returns>New interval to capture location, in secs</returns>
        public static uint NextReportInterval(double newSpeed)//long prevSpeed, long timeDiffBtwLocations, long distanceBtwLocations)
        {
            uint nextReportInterval = 60;

            //Calculate Next Location Capture interval based on speed
            if (newSpeed > 60)//km per hr
                nextReportInterval += 0;
            else if (newSpeed > 35)
                nextReportInterval += 2;
            else if (newSpeed > 15)
                nextReportInterval += 4;
            else if (newSpeed > 6)
                nextReportInterval += 6;
            else if (newSpeed > 3)
                nextReportInterval += 8;
            else if (newSpeed > 0)
                nextReportInterval += 10;

            //Apply Direction parameter to refine the capture interval //TODO

            return (uint)TimeSpan.FromSeconds(nextReportInterval).TotalMilliseconds;
        }
    }
}

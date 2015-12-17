using System;
using System.Device.Location;
using System.Diagnostics;

namespace SOS.Phone.Algorithms
{
    /// <summary>
    /// Purpose of this GPS Filter is to filter out Locations noted by mistake, with extreme noise
    /// 0th Level - If the new location capture is after 10min of prev capture, do not filter
    /// 1st Level Filtering - If new location is beyond the expected/ projected distance based on prev recordings 
    /// 2nd Level filtering - If direction changes, reduce projected speed based on direction change and apply 1st Level
    /// 180 km/hr - 50 mts/sec
    /// </summary>
    /*
      Formulae 1: 0-60 mph in 8 secs / 0-96.5 kmph in 8 secs -> 100mts
        a60 = V/t = (60*44/30)/8 = 11 ft/s²
        d60 = V²/2a = 88²/22 = 352 ft 
        The net accelerating force will vary as (1 - sinΘ), and since x = V²/2a,
        d54/8° = d60*(54/60)²/(1 - sin8°) = 331.22 ft = 100mts
     
      At 200 kmph, 8 secs 400mts max
      
     * 1. Any point exceeding 500mts to previous points is invalid point
     * 2. If prev point speed is 10 and current point speed is 40 - Avg 25kmph -> 7 mts/sec 
     * 3. 0-100-0 in 8 secs -> max 50 mts; Anything above 50mts is invalid, when it is 0 
     * 4. At 200 kmph, per sec 55mts i.e 444mts max in 8 secs 
     
     */
    public static class GPSFilter
    {
        /// <summary>
        /// Identifies new captured location is valid to consider or should be ignored(noise)
        /// </summary>
        /// <param name="prevSpeed">Speed at which previous Location was captured</param>
        /// <param name="newSpeed">Speed at which new Location is captured</param>
        /// <param name="timeDiffBtwLocations">Time difference between prev location and new location, in secs</param>
        /// <param name="distanceBtwLocations">Distance between prev location and new location, in meters</param>
        /// <returns>Returns True, if new location is valid else False</returns>
        public static bool IsValidLocation(GeoLocation prevStartLocation, GeoLocation startLocation, GeoLocation endLocation, double distance)
        {
            try
            {
                GeoCoordinate prevStart = new GeoCoordinate(Convert.ToDouble(prevStartLocation.Lat), Convert.ToDouble(prevStartLocation.Long));
                GeoCoordinate start = new GeoCoordinate(Convert.ToDouble(startLocation.Lat), Convert.ToDouble(startLocation.Long));
                GeoCoordinate end = new GeoCoordinate(Convert.ToDouble(endLocation.Lat), Convert.ToDouble(endLocation.Long));

                var timeInSeconds = Math.Round(endLocation.TimeStamp.Subtract(startLocation.TimeStamp).TotalSeconds, 0);

                if (timeInSeconds <= 0)
                    return false;

                if (timeInSeconds > 60 * 40)// If reading is greater than 40min, consider any reading
                    return true; // flight scenario

                Debug.WriteLine("1. Distance: " + distance.ToString() + " - TimeInSeconds: " + timeInSeconds + " - Result: " + (!(distance > 55 * (timeInSeconds + 1))).ToString());
                //Very high level validation - Considering max speed of the user is 200kmph 
                //i.e @ 55mts/sec + 5mts buffer as the distanceto function doesnt got by road but, by point to point calculation
                if (distance > 60 * timeInSeconds)
                    return false;

                var prevTimeInSeconds = Math.Round(startLocation.TimeStamp.Subtract(prevStartLocation.TimeStamp).TotalSeconds, 0);

                var speed = Utility.CalculateSpeed(start, end, timeInSeconds); //50 - 10
                var prevSpeed = Utility.CalculateSpeed(prevStart, start, prevTimeInSeconds); //100 - 20
                //50*(10/30)+100*(20/30) = 16.7+66.7 = 83.4
                var avgSpeed = prevSpeed * (prevTimeInSeconds / (timeInSeconds + prevTimeInSeconds)) + speed * (timeInSeconds / (timeInSeconds + prevTimeInSeconds));
                if (distance > (avgSpeed * timeInSeconds))// buffer. But, buffer should be based on current speed
                    return false;

                return true;

                //svar prevDistance = prevStart.GetDistanceTo(start);

                ////var avgSpeed = (prevDistance + distance) / (prevTimeInSeconds + timeInSeconds);// meters/ sec
                //var avgSpeed = (prevSpeed + speed) / 2;

                //Debug.WriteLine("2. Prev.Distance: " + prevDistance.ToString() + " - Prev.TimeInSeconds: " + prevTimeInSeconds + " - AvgSpeed: " + avgSpeed + " - Result: " + !(distance > (avgSpeed * (timeInSeconds + 1))));
                //if (distance > (avgSpeed * (timeInSeconds + 1)))// Additional 1 sec is buffer.
                //    return false;

                //Previous point can be greater than 7.5 secs, if previous one was skipped

                //if prev speed and current speed are same, consider it as valid point
            }
            catch
            {
            }
            return true;
        }

        private static long PredictedDistance()
        {
            return 0;
        }


    }

    public static class KalmanFilter
    {
        public static GeoLocation CorrectedPosition(GeoLocation currentLocation, GeoLocation previousEstimate)
        {
            return new GeoLocation();
        }
    }

    public class KalmanLatLong
    {
        private float MinAccuracy = 1;

        private float Q_metres_per_second;
        private long TimeStamp_milliseconds;
        private double lat;
        private double lng;
        private float variance; // P matrix.  Negative means object uninitialised.  NB: units irrelevant, as long as same units used throughout

        public KalmanLatLong(float Q_metres_per_second) { this.Q_metres_per_second = Q_metres_per_second; variance = -1; }

        public long get_TimeStamp() { return TimeStamp_milliseconds; }
        public double Lat() { return lat; }
        public double Long() { return lng; }
        public float get_accuracy() { return (float)Math.Sqrt(variance); }

        public void SetState(double lat, double lng, float accuracy, long TimeStamp_milliseconds)
        {
            this.lat = lat; this.lng = lng; variance = accuracy * accuracy; this.TimeStamp_milliseconds = TimeStamp_milliseconds;
        }

        /// <summary>
        /// Kalman filter processing for lattitude and longitude
        /// </summary>
        /// <param name="lat_measurement_degrees">new measurement of lattidude</param>
        /// <param name="lng_measurement">new measurement of longitude</param>
        /// <param name="accuracy">measurement of 1 standard deviation error in metres</param>
        /// <param name="TimeStamp_milliseconds">time of measurement</param>
        /// <returns>new state</returns>
        public void Process(double lat_measurement, double lng_measurement, float accuracy, long TimeStamp_milliseconds)
        {
            if (accuracy < MinAccuracy) accuracy = MinAccuracy;
            if (variance < 0)
            {
                // if variance < 0, object is unitialised, so initialise with current values
                this.TimeStamp_milliseconds = TimeStamp_milliseconds;
                lat = lat_measurement; lng = lng_measurement; variance = accuracy * accuracy;
            }
            else
            {
                // else apply Kalman filter methodology

                long TimeInc_milliseconds = TimeStamp_milliseconds - this.TimeStamp_milliseconds;
                if (TimeInc_milliseconds > 0)
                {
                    // time has moved on, so the uncertainty in the current position increases
                    variance += TimeInc_milliseconds * Q_metres_per_second * Q_metres_per_second / 1000;
                    this.TimeStamp_milliseconds = TimeStamp_milliseconds;
                    // TO DO: USE VELOCITY INFORMATION HERE TO GET A BETTER ESTIMATE OF CURRENT POSITION
                }

                // Kalman gain matrix K = Covarariance * Inverse(Covariance + MeasurementVariance)
                // NB: because K is dimensionless, it doesn't matter that variance has different units to lat and lng
                float K = variance / (variance + accuracy * accuracy);
                // apply K
                lat += K * (lat_measurement - lat);
                lng += K * (lng_measurement - lng);
                // new Covarariance  matrix is (IdentityMatrix - K) * Covarariance 
                variance = (1 - K) * variance;
            }
        }
    }

}

//using DotNetMatrix;

//namespace YourNameSpace here
//{
//    public sealed class KalmanFilter
//    {
//        //System matrices
//        public GeneralMatrix X0, P0;

//        public GeneralMatrix F { get; private set; }
//        public GeneralMatrix B { get; private set; }
//        public GeneralMatrix U { get; private set; }
//        public GeneralMatrix Q { get; private set; }
//        public GeneralMatrix H { get; private set; }
//        public GeneralMatrix R { get; private set; }

//        public GeneralMatrix State { get; private set; } 
//        public GeneralMatrix Covariance { get; private set; }  

//        public KalmanFilter(GeneralMatrix f, GeneralMatrix b, GeneralMatrix u, GeneralMatrix q, GeneralMatrix h,
//                            GeneralMatrix r)
//        {
//            F = f;
//            B = b;
//            U = u;
//            Q = q;
//            H = h;
//            R = r;
//        }

//        public void Predict()
//        {
//            X0 = F*State + (B*U);
//            P0 = F*Covariance*F.Transpose() + Q;
//        }

//        public void Correct(GeneralMatrix z)
//        {
//            GeneralMatrix s = H*P0*H.Transpose() + R;
//            GeneralMatrix k = P0*H.Transpose()*s.Inverse();
//            State = X0 + (k*(z - (H*X0)));
//            GeneralMatrix I = GeneralMatrix.Identity(P0.RowDimension, P0.ColumnDimension);
//            Covariance = (I - k*H)*P0;
//        }
//    }
//}

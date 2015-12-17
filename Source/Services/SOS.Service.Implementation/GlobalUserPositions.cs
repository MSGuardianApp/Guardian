using System.Collections.Generic;
using System.Linq;
using SOS.Service.Interfaces.DataContracts;

namespace SOS.Service.Implementation
{
    internal static class GlobalUserPositions
    {
        private static Dictionary<string, List<GeoTag>> _TrackingDetails;

        private static Dictionary<string, List<GeoTag>> _SOSDetails;

        // private static Dictionary<string, List<GeoTag>>

        internal static Dictionary<string, List<GeoTag>> TrackingDetails
        {
            get
            {
                if (_TrackingDetails == null)
                {
                    _TrackingDetails = new Dictionary<string, List<GeoTag>>();
                    //Load from Storage entries using a different thread X
                    //Check current validity of entries in thread X.
                }
                return _TrackingDetails;
            }

            private set
            {
                if (_TrackingDetails == null)
                {
                    _TrackingDetails = new Dictionary<string, List<GeoTag>>();
                }
                _TrackingDetails = value;

                //Load from Storage entries using a different thread X
                //Check current validity of entries in thread X.
            }
        }

        internal static Dictionary<string, List<GeoTag>> SOSDetails
        {
            get
            {
                if (_SOSDetails == null)
                {
                    _SOSDetails = new Dictionary<string, List<GeoTag>>();
                    //Load from Storage entries using a different thread X
                    //Check current validity of entries in thread X.
                }
                return _SOSDetails;
            }
            set
            {
                if (_SOSDetails == null)
                {
                    _SOSDetails = new Dictionary<string, List<GeoTag>>();
                }
                _SOSDetails = value;

                //Load from Storage entries using a different thread X
                //Check current validity of entries in thread X.
            }
        }


        internal static void UpdateTrack(string Token, GeoTag GTag)
        {
            List<GeoTag> tGTags;
            if (TrackingDetails.TryGetValue(Token, out tGTags))
            {
                if (GTag.TimeStamp < tGTags.Last().TimeStamp)
                {
                    tGTags.Add(GTag);
                    tGTags = tGTags.OrderBy(x => x.TimeStamp).ToList();
                }
                else
                    tGTags.Add(GTag);
            }
            else
            {
                tGTags = new List<GeoTag>();
                tGTags.Add(GTag);
                TrackingDetails.Add(Token, tGTags);
                //BroadCaster
                //1.1 Queue command
                //1.2 DB Delegate update DB - should go to track Table
                //if (Dummies4SOS.Members == null) Dummies4SOS.InitializeAllDataSet();
                //Profile pr = Dummies4SOS.Members.Find(x => x.SOSToken == Token);
                //if (pr != null)
                //{
                //    pr.IsTrackingOn = true;
                //}
                //Triangulation
            }
        }

        internal static void UpdateSOS(string Token, GeoTag GTag)
        {
            List<GeoTag> tGTags;
            if (SOSDetails.TryGetValue(Token, out tGTags))
            {
                if (GTag.TimeStamp < tGTags.Last().TimeStamp)
                {
                    tGTags.Add(GTag);
                    tGTags = tGTags.OrderBy(x => x.TimeStamp).ToList();
                }
                else
                    tGTags.Add(GTag);
            }
            else
            {
                tGTags = new List<GeoTag>();
                tGTags.Add(GTag);
                SOSDetails.Add(Token, tGTags);
                //BroadCaster
                //1.1 Queue command
                //1.2 DB Delegate update DB - should go to SOS table
                //if (Dummies4SOS.Members == null) Dummies4SOS.InitializeAllDataSet();
                //Profile pr = Dummy.Members.Find(x => x.SOSToken == Token);
                //if (pr != null)
                //{
                //    pr.IsSOSOn = true;
                //    // 2.1 QueueCommand
                //    //2.2 SOS WF. => admin alert, SMS Alert..    
                //}
                //else
                //{
                //    //1.3 Resolve token
                //    // 1.4 Queue COmmand
                //    //1.5 SOS WF =>  admin alert, SMS Alert..                        
                //}
                //Triangulation
            }
        }

        internal static List<GeoTag> FetchTrack(string Token)
        {
            List<GeoTag> tGTags = null;
            TrackingDetails.TryGetValue(Token, out tGTags);
            return tGTags;
        }

        internal static List<GeoTag> FetchSOS(string Token)
        {
            List<GeoTag> tGTags = null;
            SOSDetails.TryGetValue(Token, out tGTags);
            return tGTags;
        }

        internal static void RemoveSOS(string Token)
        {
            if (SOSDetails.ContainsKey(Token)) //TODO:Can Remove for Optimize
                SOSDetails.Remove(Token);
        }

        internal static void RemoveTracking(string Token)
        {
            if (TrackingDetails.ContainsKey(Token)) //TODO:Can Remove for Optimize
                TrackingDetails.Remove(Token);
        }
    }
}
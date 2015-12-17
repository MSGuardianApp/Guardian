using SOS.Service.Interfaces.DataContracts.OutBound;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class GeoTag
    {
        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public bool? IsSOS { get; set; }

        [DataMember]
        public string Lat { get; set; }
        [DataMember]
        public string Long { get; set; }
        [DataMember]
        public string Alt { get; set; }
        [DataMember]
        public Direction GeoDirection { get; set; }
        [DataMember]
        public int Speed { get; set; }

        [DataMember]
        public long TimeStamp { get; set; }

        [DataMember]
        public long ProfileID { get; set; }

        [DataMember]
        public string Command { get; set; }

        [DataMember]
        public byte[] MediaContent { get; set; }

        [DataMember]
        public string MediaUri { get; set; }       

        [DataMember]
        public string AdditionalInfo { get; set; }

        [DataMember]
        public double Accuracy { get; set; }
    }

    [DataContract]
    [KnownType(typeof(GeoTag))]
    public class IncidentTag : GeoTag
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }
    }


    [DataContract]
    [KnownType(typeof(List<GeoTag>))]
    [KnownType(typeof(GeoTag))]
    public class GeoTagList : IResult
    {
        List<GeoTag> _List;


        [DataMember]
        public List<GeoTag> List { 
            get {
                if (_List == null)
                {
                    _List = new List<GeoTag>();
                }
                return _List;
            }
            set { _List = value; }
        }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }

    }

    [DataContract]
    [KnownType(typeof(GeoTags))]
    public class GeoTags
    {
        /// <summary>
        /// Identifier (Token)
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// ProfileID 
        /// </summary>
        [DataMember]
        public long PID { get; set; }

        /// <summary>
        /// Array of Latitudes, indexes and counts to match with other array objects 
        /// </summary>
        [DataMember]
        public string[] Lat { get; set; }

        /// <summary>
        /// Array of Longitude, indexes and counts to match with other array objects 
        /// </summary>
        [DataMember]
        public string[] Long { get; set; }

        /// <summary>
        /// Array of Altitude, indexes and counts to match with other array objects 
        /// </summary>
        [DataMember]
        public string[] Alt { get; set; }

        /// <summary>
        /// Array of Speed, indexes and counts to match with other array objects 
        /// </summary>
        [DataMember]
        public int[] Spd { get; set; }

        /// <summary>
        /// Array of Client Timestamp Ticks, indexes and counts to match with other array objects 
        /// </summary>
        [DataMember]
        public long[] TS { get; set; }

        /// <summary>
        /// Message Type '0' for track, '1' for SOS
        /// </summary>
        [DataMember]
        public bool[] IsSOS { get; set; }

        /// <summary>
        /// Count of the Lat and Longs in this.
        /// </summary>
        [DataMember]
        public int LocCnt{ get; set; }

        [DataMember]
        public double[] Accuracy { get; set; }
    }

    [DataContract]
    public enum Direction
    {
        something,
    }
}

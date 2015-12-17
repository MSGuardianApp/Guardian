using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOS.Service.Interfaces.DataContracts.InBound
{
    [DataContract]
    [KnownType(typeof (ProfileBuddyInfo))]
    [KnownType(typeof(Group))]
    [KnownType(typeof(List<ProfileBuddyInfo>))]
    [KnownType(typeof(List<Group>))]
    public class ProfileBasicInfo
    {
        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string ProfileID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public List<ProfileBuddyInfo> Buddies { get; set; }

        [DataMember]
        public ProfileBuddyInfo PrimeBuddy { get; set; }

        [DataMember]
        public List<Group> Groups { get; set; }

        [DataMember]
        public string SecurityKey { get; set; }

    }


    [DataContract]
    public class ProfileFBInfo
    {
        [DataMember]
        public string ProfileID { get; set; }

        [DataMember]
        public string FBID { get; set; }

        [DataMember]
        public string FBAuthID { get; set; }

        [DataMember]
        public string GroupName{ get; set; }

    }

    [DataContract]
    public class ProfileLiveInfo
    {
        [DataMember]
        public string ProfileID { get; set; }

        /// <summary>
        /// Equal to the Email ID of the Profile. Constraint to be implemented in the clients.
        /// </summary>
        [DataMember]
        public string LiveID { get; set; }

        [DataMember]
        public string ArchiveFolder { get; set; }

        [DataMember]
        public bool CanArchive{ get; set; }

    }


    [DataContract]
    public class ProfileBuddyInfo
    {
        [DataMember]
        public string BuddyRelationID { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }     
    }
}

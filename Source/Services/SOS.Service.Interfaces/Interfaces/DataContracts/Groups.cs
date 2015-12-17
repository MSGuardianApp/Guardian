using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SOS.Service.Interfaces.DataContracts.OutBound;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]
    public class Group
    {
        [DataMember]
        public string GroupID { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string GroupLocation { get; set; }

        [DataMember]
        public string EnrollmentKey { get; set; }

        [DataMember]
        public string EnrollmentValue { get; set; }

        [DataMember]
        public bool ToRemove { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public LiveCred LiveInfo { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public List<Profile> Members { get; set; }

        public List<String> Tags { get; set; }

        [DataMember]
        public GroupType Type { get; set; }

        [DataMember]
        public Enrollment EnrollmentType { get; set; }

        [DataMember]
        public bool IsValidated { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public int? ParentGroupID { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public bool NotifySubgroups { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string ShapeFileID { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string SubGroupIdentificationKey { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string GeoLocation { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public bool AllowGroupManagement { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public bool ShowIncidents { get; set; }
    }


    [DataContract]
    public class GroupList : IResult
    {

        [DataMember]
        public List<Group> List { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
    }

    [DataContract]
    public class GroupMembers : IResult
    {
        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public List<string> Profiles { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
    }

    public enum GroupType
    {
        Public = 0,
        Private = 1,
        Social = 2,
    }
    /// <summary>
    /// For Locally dividing the members associated for monitoring purpose
    /// </summary>
    public class SubVGroup
    { }

    /// <summary>
    /// has multiple groups could be social or governmetn or institute => accesstypes will vary for admin
    /// </summary>
    public class Organization
    { }

    public enum Enrollment
    {
        None = 0,
        AutoOrgMail = 1,
        Moderator = 2,
    }

    [DataContract]
    public class AdminUser
    {
        [DataMember]
        public int AdminID { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string GroupIDCSV { get; set; }

        [DataMember]
        public string LiveUserID { get; set; }

    }

}

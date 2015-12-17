using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SOS.Service.Interfaces.DataContracts.OutBound;

namespace SOS.Service.Interfaces.DataContracts
{
    [DataContract]    
    [KnownType(typeof(ProfileLite))]
    [KnownType(typeof(Buddy))]
    public class User:IResult
    {
        
        [DataMember]
        public long UserID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get ; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public LiveCred LiveDetails { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string FBID { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string FBAuthID { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public string RegionCode { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
       
    }

    [DataContract]
    public class ComChannels
    {
        [DataMember]
        public string ToastChannel { get; set; }

        [DataMember]
        public string ToastExpiry { get; set; }
    }

    [DataContract]
    
    [KnownType(typeof(User))]
    [KnownType(typeof(Profile))]   
    public class ProfileLite : User, IResult
    {
        //public ProfileLite() { }

        [DataMember]
        public long ProfileID { get; set; }

        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public List<GeoTag> LastLocs { get; set; }

        [DataMember]
        public bool IsTrackingOn { get; set; }

        [DataMember]
        public bool IsSOSOn { get; set; }

        [DataMember]
        public string TinyURI { get; set; }

        

       
      
    }

     [DataContract]
    public class ProfileLiteList:IResult
    {
         [DataMember]
         public List<ProfileLite> List { get; set; }

         [DataMember]
         public List<ResultInfo> DataInfo { get; set; }
    }

     [DataContract]
     public class ProfileList : IResult
     {
         [DataMember]
         public List<Profile> List { get; set; }

         [DataMember]
         public List<ResultInfo> DataInfo { get; set; }
     }

     [DataContract]
     public class Contact : IResult
     {
         [DataMember]
         public string PhoneNumber { get; set; }

         [DataMember]
         public string MailID { get; set; }

         [DataMember]
         public List<ResultInfo> DataInfo { get; set; }
     }

    [DataContract]
    public class Profile:ProfileLite, IResult
    {
        [DataMember]
        public string EnterpriseEmailID { get; set; }

        [DataMember]
        public string EnterpriseSecurityToken { get; set; }

        [DataMember]
        public string SecurityToken { get; set; }
        
        [DataMember]
        public string PrimeGroupID { get; set; }

        [DataMember]
        public List<Group> AscGroups { get; set; }

        [DataMember]
        public List<ProfileLite> LocateBuddies { get; set; }

        [DataMember]
        public List<Buddy> MyBuddies { get; set; }

        [DataMember]
        public bool CanArchive { get; set; }

        [DataMember]
        public string FBGroupName { get; set; }

        [DataMember]
        public string FBGroupID { get; set; }

        [DataMember]
        public bool CanPost { get; set; }

        [DataMember]
        public bool CanMail { get; set; }

        [DataMember]
        public bool CanSMS { get; set; }

        [DataMember]
        public bool IsValid { get; set; }

        [DataMember]
        public DeviceSetting PhoneSetting { get; set; }

        [DataMember]
        public PortalSetting SiteSetting { get; set; }

        [DataMember]
        public bool LocationConsent { get; set; }

        [DataMember]
        public string NotificationUri { get; set; }

    }

    [DataContract]
    public class MarshallToAdd
    {
        [DataMember]
        public string GroupID { get; set; }

        [DataMember]
        public string LiveMail { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }
    }
   
    [DataContract]
    public class Marshal : ProfileLite
    {
        [DataMember]
        public List<ProfileLite> LocateBuddies { get; set; }

        [DataMember]
        public bool IsValidated { get; set; }
    }

    [DataContract]
    public class MarshalList : IResult
    {
        [DataMember]
        public List<Marshal> List { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo{get;set;}
    }

    [DataContract]
    public class Buddy : User, IResult
    {
        [DataMember]
        public long BuddyID { get; set; }

        //[DataMember]
        //public ProfileLite Profile{ get; set; }

        [DataMember]
        public BuddyState State { get; set; }

        [DataMember]
        public bool ToRemove { get; set; }

        [DataMember]
        public bool IsPrimeBuddy { get; set; }

        [DataMember]
        public Guid? SubscribtionID { get; set; }
    }

    public enum BuddyState
    {
        Active = 1,
        Suspended = 2,
        Blocked = 3,
        Marshal = 4
    }

    [DataContract]
    public class BuddyList : IResult
    {
        [DataMember]
        public List<Buddy> List { get; set; }
        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }
    }


    [DataContract]
    public class Admin : IResult
    {
        [DataMember]
        public int AdminID { get; set; }

        [DataMember]
        public string GroupIDCSV { get; set; }
                
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public bool AllowGroupManagement { get; set; }

        [DataMember]
        public List<Group> Groups { get; set; }

        [DataMember]
        public List<ResultInfo> DataInfo { get; set; }

    }

    [DataContract]
    public class HealthUpdate
    {
        [DataMember]
        public bool IsProfileActive { get; set; }

        [DataMember]
        public bool IsGroupModified { get; set; }

        [DataMember]
        public string ServerVersion { get; set; }
    }

    public class CacheUser
    {
        public bool IsValid { get; set; }
        public bool IsExpired { get; set; }

        public string LiveUserID { get; set; }

        public char UserType { get; set; }

        public string SOSUserID { get; set; }

    }
}

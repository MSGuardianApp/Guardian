namespace SOS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Profile")]
    public partial class Profile
    {
        public Profile()
        {
            Buddies = new HashSet<Buddy>();
            GroupMarshals = new HashSet<GroupMarshal>();
            GroupMemberships = new HashSet<GroupMembership>();
            LiveLocations = new HashSet<LiveLocation>();
        }

        public long ProfileID { get; set; }

        public long UserID { get; set; }

        [StringLength(4000)]
        public string MobileNumber { get; set; }

        [StringLength(10)]
        public string RegionCode { get; set; }

        [StringLength(100)]
        public string DeviceID { get; set; }

        public DeviceType? DeviceType { get; set; }

        [StringLength(100)]
        public string FBGroup { get; set; }

        [StringLength(100)]
        public string FBGroupID { get; set; }

        public bool CanPost { get; set; }

        public bool CanSMS { get; set; }

        public bool CanEmail { get; set; }

        [StringLength(10)]
        public string SecurityToken { get; set; }

        public bool LocationConsent { get; set; }

        public bool IsValid { get; set; }

        public string EnterpriseSecurityToken { get; set; }
        
        public string EnterpriseEmailID { get; set; }

        public string NotificationUri { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(50)]
        public string LastModifiedBy { get; set; }

        public virtual ICollection<Buddy> Buddies { get; set; }

        public virtual ICollection<GroupMarshal> GroupMarshals { get; set; }

        public virtual ICollection<GroupMembership> GroupMemberships { get; set; }

        public virtual ICollection<LiveLocation> LiveLocations { get; set; }

        public virtual User User { get; set; }
    }
}

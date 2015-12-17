namespace SOS.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Buddy")]
    public partial class Buddy
    {
        public long BuddyID { get; set; }

        public long ProfileID { get; set; }

        public long UserID { get; set; }

        [StringLength(50)]
        public string BuddyName { get; set; }

        [StringLength(4000)]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public bool IsPrimeBuddy { get; set; }

        public BuddyState State { get; set; }

        public Guid? SubscribtionId { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(50)]
        public string LastModifiedBy { get; set; }       

        public virtual Profile Profile { get; set; }

        public virtual User User { get; set; }
    }
}

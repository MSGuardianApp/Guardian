namespace SOS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        public User()
        {
            Buddies = new HashSet<Buddy>();
            Profiles = new HashSet<Profile>();
        }

        public long UserID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(4000)]
        public string MobileNumber { get; set; }

        [StringLength(4000)]
        public string FBAuthID { get; set; }

        [StringLength(4000)]
        public string FBID { get; set; }

        [StringLength(4000)]
        public string LiveID { get; set; }        

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(50)]
        public string LastModifiedBy { get; set; }

        public virtual ICollection<Buddy> Buddies { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}

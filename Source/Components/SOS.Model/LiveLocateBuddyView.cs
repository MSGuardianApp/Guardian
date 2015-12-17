
namespace SOS.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("LiveLocateBuddyView")]

    public partial class LiveLocateBuddyView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProfileID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Lat { get; set; }

        [StringLength(50)]
        public string Long { get; set; }

        public bool? IsSOS { get; set; }

        public long ClientTimeStamp { get; set; }

        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }


}


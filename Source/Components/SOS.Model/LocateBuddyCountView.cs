namespace SOS.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("LocateBuddyCountView")]
    public partial class LocateBuddyCountView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserID { get; set; }

        public int? SosCount { get; set; }

        public int? TrackCount { get; set; }
    }
}

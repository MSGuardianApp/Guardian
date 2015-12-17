namespace SOS.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LiveLocation")]
    public partial class LiveLocation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProfileID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string SessionID { get; set; }

        [Key]
        [Column(Order = 2)]
        public long ClientTimeStamp { get; set; }

        public DateTime ClientDateTime { get; set; }

        public bool? IsSOS { get; set; }

        [StringLength(50)]
        public string Lat { get; set; }

        [StringLength(50)]
        public string Long { get; set; }

        [StringLength(10)]
        public string Alt { get; set; }

        public int Speed { get; set; }

        [StringLength(250)]
        public string MediaUri { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Accuracy { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public virtual Profile Profile { get; set; }
    }
}

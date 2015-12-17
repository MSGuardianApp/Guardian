namespace SOS.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LiveSession")]
    public partial class LiveSession
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProfileID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string SessionID { get; set; }

        public bool IsSOS { get; set; }

        [StringLength(50)]
        public string Lat { get; set; }

        [StringLength(50)]
        public string Long { get; set; }

        public DateTime? LastCapturedDate { get; set; }

        public long? ClientTimeStamp { get; set; }

        [StringLength(100)]
        public string ProcessingInstanceId { get; set; }

        [Index]
        public Guid? ProcessKey { get; set; }

        [StringLength(20)]
        public string Command { get; set; }

        [StringLength(50)]
        public string ExtendedCommand { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime? SessionEndTime { get; set; }

        public bool? InSOS { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string MobileNumber { get; set; }

        public int? LastSubGroupID { get; set; }

        public DateTime? LastSMSPostTime { get; set; }

        public DateTime? LastEmailPostTime { get; set; }

        public DateTime? LastFacebookPostTime { get; set; }

        [StringLength(2000)]
        public string SMSRecipientsList { get; set; }

        [StringLength(1000)]
        public string EmailRecipientsList { get; set; }

        [StringLength(100)]
        public string FBGroupID { get; set; }

        [StringLength(4000)]
        public string FBAuthID { get; set; }

        [StringLength(250)]
        public string TinyUri { get; set; }

        public short? NoOfSMSRecipients { get; set; }

        public short? NoOfEmailRecipients { get; set; }

        public short? NoOfSMSSent { get; set; }

        public short? NoOfEmailsSent { get; set; }

        public short? NoOfFBPostsSent { get; set; }

        public string DispatchInfo { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public bool IsEvidenceAvailable { get; set; }

        public virtual Profile Profile { get; set; }
    }
}

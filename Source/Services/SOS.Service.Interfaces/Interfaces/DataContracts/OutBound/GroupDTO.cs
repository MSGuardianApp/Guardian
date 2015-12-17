namespace SOS.Service.Interfaces.DataContracts.OutBound
{
    public class GroupDTO
    {
        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }

        public GroupType GroupType { get; set; }

        public Enrollment EnrollmentType { get; set; }

        public string EnrollmentKey { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string LiveID { get; set; }

        public int? ParentGroupID { get; set; }

        public bool NotifySubgroups { get; set; }

        public string ShapeFileID { get; set; }

        public string GroupKey { get; set; }

        public string GeoLocation { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string ParentGroupName { get; set; }

        public bool AllowGroupManagement { get; set; }

        public bool ShowIncidents { get; set; }
    }
}

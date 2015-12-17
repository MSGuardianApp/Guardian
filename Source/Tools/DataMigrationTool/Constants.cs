namespace SOS.OPsTools
{
    public class ConstantOps
    {
        public const string ConfigurationTableName = "Configuration";

        //public const string GroupTableName = "Group";
        //public const string GroupAdminsTableName = "GroupAdmins";

        //public const string IncidentsTableName = "Incidents";

        //public const string PhoneValidationTableName = "PhoneValidation";
        //public const string GroupMemberValidatorTableName = "GroupMemberValidator";
        //public const string GroupMarshalValidatorTableName = "GroupMarshalValidator";

        //public const string LocationHistoryTableName = "LocationHistory";
        //public const string SessionHistoryTableName = "SessionHistory";

        public const string PartitionKey = "PartitionKey";
        public const string RowKey = "RowKey";


        public const string UserTableName = "User";
        public const string ProfileTableName = "Profile";
        public const string BuddyTableName = "Buddy";

        public const string GroupMembershipTableName = "GroupMembership";
        public const string GroupMarshalRelationTableName = "GroupMarshalRelation";

        //Source Storage
        public const string GroupSrcTableName = "Group";
        public const string GroupAdminsSrcTableName = "GroupAdmins";        
        public const string HistoryGeoLocationSrcTableName = "HistoryGeoLocation";
        public const string TeaseSrcTableName = "TeaseReport";        
        public const string GroupMemberValidatorSrcTableName = "GroupMemberValidator";
        public const string PhoneValidationSrcTableName = "PhoneValidation";

        //Destination Storage
        public const string GroupDestTableName = "Group";
        public const string GroupAdminsDestTableName = "GroupAdmins";       
        public const string HistoryGeoLocationDestTableName = "LocationHistory";
        public const string TeaseDestTableName = "Incident";        
        public const string GroupMemberValidatorDestTableName = "GroupMemberValidator";
        public const string PhoneValidatorDestTableName = "PhoneValidation";

    }
}

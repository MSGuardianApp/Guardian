namespace SOS.AzureStorageAccessLayer
{
    public class Constants
    {
        public const string ConfigurationTableName = "Configuration";

        public const string GroupTableName = "Group";
        public const string GroupAdminsTableName = "GroupAdmins";

        public const string IncidentsTableName = "Incidents";

        public const string PhoneValidationTableName = "PhoneValidation";
        public const string GroupMemberValidatorTableName = "GroupMemberValidator";
        public const string GroupMarshalValidatorTableName = "GroupMarshalValidator";

        public const string LocationHistoryTableName = "LocationHistory";
        public const string SessionHistoryTableName = "SessionHistory";

        public const string PartitionKey = "PartitionKey";
        public const string RowKey = "RowKey";

        public const string ParentGroupID = "ParentGroupID";


        //public const string UserTableName = "User";
        //public const string ProfileTableName = "Profile";
        //public const string BuddyTableName = "Buddy";

        //public const string GroupMembershipTableName = "GroupMembership";
        //public const string GroupMarshalRelationTableName = "GroupMarshalRelation";
        
        //public const string LocationQueueName = "locationqueue";
    }
}

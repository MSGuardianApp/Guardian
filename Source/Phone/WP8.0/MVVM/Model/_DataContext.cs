using System.Data.Linq;

namespace SOS.Phone
{
    public class SOSDataContext : DataContext
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/GuardianDatabase.sdf";


        // Pass the connection string to the base class.
        public SOSDataContext(string connectionString)
            : base(connectionString)
        { }

        //My(User) Data
        public Table<UserTableEntity> UserTable;

        //My Profiles
        public Table<ProfileTableEntity> MyProfilesTable;

        //My Groups
        public Table<GroupTableEntity> MyGroupsTable;

        //My Buddies
        public Table<BuddyTableEntity> MyBuddiesTable;

        //Buddies who added me as Buddy
        public Table<LocateBuddyTableEntity> LocateBuddiesTable;


    }
}

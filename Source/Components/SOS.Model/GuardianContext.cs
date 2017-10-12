using Guardian.Common.Configuration;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SOS.Model
{

    [Serializable]
    [DbConfigurationType(typeof(GuardianDbConfiguration))]
    public class GuardianContext : DbContext
    {
        public GuardianContext(string connectionString)
            : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            //Database.Log = Console.WriteLine;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Profile>()
            //    .HasRequired(p => p.User)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Buddy> Buddies { get; set; }
        public virtual DbSet<LiveLocation> LiveLocations { get; set; }
        public virtual DbSet<LiveSession> LiveSessions { get; set; }
        public virtual DbSet<GroupMarshal> GroupMarshals { get; set; }
        public virtual DbSet<GroupMembership> GroupMemberships { get; set; }

        //Views
        public virtual DbSet<LiveGroupMemberView> LiveGroupMemberViews { get; set; }
        public virtual DbSet<LiveLocateBuddyView> LiveLocateBuddiesViews { get; set; }
        public virtual DbSet<LocateBuddyCountView> LocateBuddyCountViews { get; set; }
        public virtual DbSet<LocateBuddyView> LocateBuddiesViews { get; set; }
    }
}

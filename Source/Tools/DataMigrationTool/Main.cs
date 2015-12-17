using SOS.AzureStorageAccessLayer;
using SOS.AzureStorageAccessLayer.Entities;
using SOS.Service.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOS.Model;
using SOS.AzureSQLAccessLayer;
using Opstool = SOS.OPsTools;
using OpstoolEntity = SOS.OPsTools.Entities;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Web;
using System.Threading;
using System.Configuration;


namespace Tools
{
    public partial class Main : Form
    {

        private Random _Random;

        OPsLogger opsLogger = OPsLogger.GetInstance("Logger_Migration.txt");


        Dictionary<Guid, long> MapUsers = null;
        Dictionary<Guid, long> MapProfiles = null;

        //SQL Repository Classes
        Opstool.MainRepository _Repository = new Opstool.MainRepository();
        //Storage Classes
        Opstool.MainStorageAccess _MainStorageAccess = new Opstool.MainStorageAccess();
        // static ManualResetEvent manualReset = new ManualResetEvent(false);

        public Main()
        {
            InitializeComponent();
            pgbMigration.Style = ProgressBarStyle.Marquee;
            pgbMigration.Visible = false;
            _Random = new Random();


        }

        private void MigrateUser(DateTime LastRunTime)
        {

            ///User Migration
            List<OpstoolEntity.User> StorageUsers = _MainStorageAccess.GetAllStorageUsers(LastRunTime);

            // List<User> ModelUsers = _Repository.GetAllUser().Result;

            MapUsers = _Repository.GetAllMapUsers().Result;

            //Filtering Method--1

            List<OpstoolEntity.User> FilteredStorageUsers = null;

            if (MapUsers.Count > 0)
            {
                var CommonStorageUsers = from storageUser in StorageUsers
                                         join mapUser in MapUsers on storageUser.UserID equals mapUser.Key.ToString()
                                         select storageUser;

                FilteredStorageUsers = StorageUsers.Except(CommonStorageUsers).ToList();
            }

            else
                FilteredStorageUsers = StorageUsers;

            //Push Storage Users into SQL User &  UserMap Tables
            foreach (var user in FilteredStorageUsers)
            {
                try
                {
                    _Repository.InsertUserToSQLUserTable(user).GetAwaiter().GetResult();

                }

                catch (Exception ex) { opsLogger.WriteLog("User Error Message: " + ex.Message + Environment.NewLine + "UserID: " + user.UserID + Environment.NewLine + "MobileNumber: " + user.MobileNumber + Environment.NewLine + "Email: " + user.Email); }
            }

            // manualReset.Set();
        }

        private void MigrateProfile(DateTime LastRunTime)
        {
            MigrateUser(LastRunTime);

            //Profile Migration

            MapUsers = _Repository.GetAllMapUsers().Result;

            MapProfiles = _Repository.GetAllMapProfiles().Result;

            List<OpstoolEntity.Profile> StorageProfiles = _MainStorageAccess.GetAllStorageProfiles(LastRunTime);

            List<OpstoolEntity.Profile> FilteredStorageProfiles = null;

            if (MapUsers.Count > 0 && MapProfiles.Count > 0)
            {
                var CommonStorageProfiles = from storageProfile in StorageProfiles
                                            join mapUser in MapUsers on storageProfile.UserID equals mapUser.Key.ToString()
                                            join mapProfile in MapProfiles on storageProfile.ProfileID equals mapProfile.Key.ToString()
                                            select storageProfile;

                FilteredStorageProfiles = StorageProfiles.Except(CommonStorageProfiles).ToList();

            }

            else
                FilteredStorageProfiles = StorageProfiles;


            //Push each Storage profile to sql profile & ProfileMap Tables
            foreach (var profile in FilteredStorageProfiles)
            {
                try
                {
                    _Repository.InsertProfileToSQLProfileTable(profile, MapUsers[Guid.Parse(profile.UserID)]).GetAwaiter().GetResult();
                }
                catch (Exception ex) { opsLogger.WriteLog("Profile Error Message: " + ex.Message + Environment.NewLine + "ProfileID: " + profile.ProfileID + Environment.NewLine + "UserID: " + profile.UserID + Environment.NewLine + "MobileNumber: " + profile.MobileNumber); }

            }



        }

        private void MigrateBuddy(DateTime LastRunTime)
        {

            MigrateProfile(LastRunTime);

            MapProfiles = _Repository.GetAllMapProfiles().GetAwaiter().GetResult();

            //Budddy Migration
            List<OpstoolEntity.Buddy> StorageBuddies = _MainStorageAccess.GetAllStorageBuddies(LastRunTime);

            //Buddy Exist or New buddy , its handled inside store procedure.
            foreach (var buddy in StorageBuddies)
            {
                try
                {
                    _Repository.InsertProfileToSQLBuddyTable(buddy, MapUsers[Guid.Parse(buddy.UserID)], MapProfiles[Guid.Parse(buddy.ProfileID)]).GetAwaiter().GetResult();
                }
                catch (Exception ex) { opsLogger.WriteLog("Buddy Error Message: " + ex.Message + Environment.NewLine + "BuddyID: " + buddy.BuddyID + Environment.NewLine + "BuddyName: " + buddy.BuddyName + Environment.NewLine + "UserID: " + buddy.UserID + "ProfileID: " + buddy.ProfileID); }
            }


        }

        private void MigrateGroupMembership(DateTime LastRunTime)
        {

            MigrateProfile(LastRunTime);
            //GroupMembership Migration
            List<OpstoolEntity.GroupMembership> StorageGroupMemberships = _MainStorageAccess.GetAllStorageGroupMemberships(LastRunTime);

            //GroupMembership Exist or New GroupMembership , its handled inside store procedure.
            foreach (var grp in StorageGroupMemberships)
            {
                try
                {
                    _Repository.InsertProfileToSQGroupMembershipTable(grp, MapProfiles[Guid.Parse(grp.ProfileID)]).GetAwaiter().GetResult();
                }
                catch (Exception ex) { opsLogger.WriteLog("GroupMembership Error Message: " + ex.Message + Environment.NewLine + "GroupID: " + grp.GroupID + Environment.NewLine + "ProfileID: " + grp.ProfileID); }
            }
        }

        private void MigeateGroupMarshal(DateTime LastRunTime)
        {

            MigrateProfile(LastRunTime);
            //GroupMarshal  Migration
            List<OpstoolEntity.GroupMarshalRelation> StorageGroupMarshals = _MainStorageAccess.GetAllStorageGroupMarshals(LastRunTime);

            //GroupMarshal Exist or New GroupMarshal , its handled inside store procedure.
            foreach (var grpMarshal in StorageGroupMarshals)
            {
                try
                {
                    _Repository.InsertProfileToSQGroupMarshalTable(grpMarshal, MapProfiles[Guid.Parse(grpMarshal.ProfileID)]).GetAwaiter().GetResult();
                }
                catch (Exception ex) { opsLogger.WriteLog("GroupMarshal Error Message: " + ex.Message + Environment.NewLine + "GroupID: " + grpMarshal.GroupID + Environment.NewLine + "ProfileID: " + grpMarshal.ProfileID); }
            }



        }


        private void btnMigrate_Click(object sender, EventArgs e)
        {
            
            DateTime LastRunTime = Convert.ToDateTime(OPsLogger.ReadLastRun("ToolLastRunTime.txt"));

            string NextRunTime = DateTime.Now.ToString();

            try
            {

                if (chkUser.Checked)
                {
                    MigrateUser(LastRunTime);
                }

                if (chkProfile.Checked)
                {

                    MigrateProfile(LastRunTime);
                }

                if (chkBuddy.Checked)
                {

                    MigrateBuddy(LastRunTime);
                }

                if (chkGroupMembership.Checked)
                {
                    MigrateGroupMembership(LastRunTime);
                }
                if (chkGroupMarshal.Checked)
                {
                    MigeateGroupMarshal(LastRunTime);
                }
                if (ALL.Checked)
                {
                    MigrateBuddy(LastRunTime);
                    MigrateGroupMembership(LastRunTime);
                    MigeateGroupMarshal(LastRunTime);
                }
                if (chkGroupMemberValidator.Checked)
                {
                    MigrateProfile(LastRunTime);
                    _MainStorageAccess.MigrateGroupMemberValidatortoDest(LastRunTime);
                }

                if (chkTease.Checked)
                {
                    MigrateProfile(LastRunTime);
                    _MainStorageAccess.MigrateIncidenttoDest(LastRunTime);
                }
                if (chkHistoryGeoLocation.Checked)
                {
                    MigrateProfile(LastRunTime);
                    _MainStorageAccess.MigrateHistoryGeoLocationtoDest(LastRunTime);
                }

                if (chkGroup.Checked)
                {
                    _MainStorageAccess.MigrateGrouptoDest(LastRunTime);
                }
                if (chkGroupAdmins.Checked)
                {
                    _MainStorageAccess.MigrateGroupAdminstoDest(LastRunTime);
                }

                if (chkPhoneValidation.Checked)
                {
                    _MainStorageAccess.MigratePhoneValidationtoDest(LastRunTime);
                }
                
                OPsLogger.UpdateLastRun("ToolLastRunTime.txt", NextRunTime);

                MessageBox.Show("Successfully Migrated");
            }
            catch (Exception ex)
            {
                //Log exception
            }
        }


    }
}


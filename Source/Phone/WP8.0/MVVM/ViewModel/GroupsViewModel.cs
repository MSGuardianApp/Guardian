using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace SOS.Phone
{
    public class GroupsViewModel : INotifyPropertyChanged
    {
        private SOSDataContext _dataContext;

        public GroupsViewModel()
        {
            _dataContext = new SOSDataContext(SOSDataContext.DBConnectionString);
        }

        private ObservableCollection<GroupTableEntity> _groups = null;
        public ObservableCollection<GroupTableEntity> Groups
        {
            get
            {
                return _groups;
            }
            private set
            {
                _groups = value;
                NotifyPropertyChanged("Groups");
            }
        }

        public List<GroupTableEntity> AllGroups
        {
            get
            {
                List<GroupTableEntity> allGroups = (from GroupTableEntity g in _dataContext.MyGroupsTable
                                                    where g.MyProfileId == Globals.User.CurrentProfileId
                                                    orderby g.Name ascending
                                                    select g).ToList();

                return allGroups;
            }
        }

        private MembershipServiceRef.HealthUpdate _pendingUpdates = null;
        public MembershipServiceRef.HealthUpdate PendingUpdates
        {
            get
            {
                return _pendingUpdates;
            }
            private set
            {
                _pendingUpdates = value;
            }
        }

        private DateTime _lastSyncDateTime = DateTime.Now;
        public DateTime LastSyncDateTime
        {
            get
            {
                return _lastSyncDateTime;
            }
            private set
            {
                _lastSyncDateTime = value;
            }
        }

        public GroupTableEntity ConvertGroup(MembershipServiceRef.Group serverGroup, string profileId = "")
        {
            var phoneGroup = new GroupTableEntity();

            phoneGroup.GroupId = serverGroup.GroupID;
            phoneGroup.MyProfileId = profileId == string.Empty ? Globals.User.CurrentProfileId : profileId;

            phoneGroup.Name = serverGroup.GroupName;
            phoneGroup.PhoneNumber = serverGroup.PhoneNumber;
            phoneGroup.Email = serverGroup.Email;
            phoneGroup.Type = serverGroup.Type;

            phoneGroup.EnrollmentType = serverGroup.EnrollmentType;
            phoneGroup.EnrollmentValue = serverGroup.EnrollmentValue;
            phoneGroup.EnrollmentKey = serverGroup.EnrollmentKey;

            phoneGroup.IsDeleted = serverGroup.ToRemove;
            phoneGroup.IsValidated = serverGroup.IsValidated;

            phoneGroup.BorderThickness = new Thickness(2);
            phoneGroup.BuddyStatusColor = Constants.WhiteColorCode;

            return phoneGroup;
        }

        public GroupTableEntity ConvertGroup(GroupServiceRef.Group serverGroup, string myProfileId = "")
        {
            var phoneGroup = new GroupTableEntity();

            phoneGroup.GroupId = serverGroup.GroupID;
            phoneGroup.MyProfileId = myProfileId;

            phoneGroup.Name = serverGroup.GroupName;
            phoneGroup.PhoneNumber = serverGroup.PhoneNumber;
            phoneGroup.Email = serverGroup.Email;
            phoneGroup.Type = (MembershipServiceRef.GroupType)serverGroup.Type;

            phoneGroup.EnrollmentType = (MembershipServiceRef.Enrollment)serverGroup.EnrollmentType;
            phoneGroup.EnrollmentValue = serverGroup.EnrollmentValue;
            phoneGroup.EnrollmentKey = serverGroup.EnrollmentKey;

            phoneGroup.IsDeleted = serverGroup.ToRemove;
            phoneGroup.IsValidated = serverGroup.IsValidated;

            phoneGroup.BorderThickness = new Thickness(2);
            phoneGroup.BuddyStatusColor = Constants.WhiteColorCode;

            return phoneGroup;
        }

        public MembershipServiceRef.Group ConvertGroup(GroupTableEntity phoneGroup)
        {
            var serverGroup = new MembershipServiceRef.Group();

            serverGroup.GroupID = phoneGroup.GroupId;
            serverGroup.GroupName = phoneGroup.Name;
            serverGroup.Email = phoneGroup.Email;
            serverGroup.PhoneNumber = phoneGroup.PhoneNumber;
            serverGroup.Type = phoneGroup.Type;

            serverGroup.EnrollmentType = phoneGroup.EnrollmentType;
            serverGroup.EnrollmentValue = phoneGroup.EnrollmentValue;
            serverGroup.EnrollmentKey = phoneGroup.EnrollmentKey;

            serverGroup.ToRemove = phoneGroup.IsDeleted;
            serverGroup.IsValidated = phoneGroup.IsValidated;

            return serverGroup;
        }

        public void LoadGroups(string profileId)
        {
            this.Message = string.Empty;
            this.IsInProgress = true;

            var groupsInLocalStore = from GroupTableEntity g in _dataContext.MyGroupsTable
                                     where g.MyProfileId == profileId && g.IsDeleted == false
                                     orderby g.Name ascending
                                     select g;

            this.Groups = new ObservableCollection<GroupTableEntity>(groupsInLocalStore);

            foreach (var group in this.Groups)
            {
                group.BorderThickness = new Thickness(2);
                group.BuddyStatusColor = Constants.WhiteColorCode;
            }

            this.IsInProgress = false;
            this.IsDataLoaded = true;
        }

        public void AddGroup(GroupTableEntity newGroup)
        {
            IsSuccess = true;
            Message = string.Empty;

            try
            {
                var groupInLocalStore = (from GroupTableEntity g in _dataContext.MyGroupsTable
                                         where g.GroupId == newGroup.GroupId
                                         select g).FirstOrDefault();

                if (groupInLocalStore != null)
                    groupInLocalStore.IsDeleted = false;
                else
                    _dataContext.MyGroupsTable.InsertOnSubmit(newGroup);

                _dataContext.SubmitChanges();
                this.Groups.Add(newGroup);
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = ex.Message;
            }

        }

        public void UpdateGroup(string profileId, GroupTableEntity latestGroup)
        {
            var localGroup = (from GroupTableEntity b in _dataContext.MyGroupsTable
                              where b.GroupId == latestGroup.GroupId && b.MyProfileId == profileId
                              select b).FirstOrDefault();

            if (localGroup != null)
            {
                localGroup.GroupId = latestGroup.GroupId;
                localGroup.MyProfileId = latestGroup.MyProfileId;

                localGroup.Name = latestGroup.Name;
                localGroup.PhoneNumber = latestGroup.PhoneNumber;
                localGroup.Email = latestGroup.Email;
                localGroup.Type = latestGroup.Type;

                localGroup.EnrollmentType = latestGroup.EnrollmentType;
                localGroup.EnrollmentValue = latestGroup.EnrollmentValue;
                localGroup.EnrollmentKey = latestGroup.EnrollmentKey;


                localGroup.BorderThickness = latestGroup.BorderThickness;
                localGroup.BuddyStatusColor = latestGroup.BuddyStatusColor;

                localGroup.IsDeleted = latestGroup.IsDeleted;
                localGroup.IsValidated = latestGroup.IsValidated;

                _dataContext.SubmitChanges();
            }
        }

        public void DeleteGroup(string groupId)
        {
            IsSuccess = true;
            Message = string.Empty;

            try
            {
                var group = (from GroupTableEntity b in this._dataContext.MyGroupsTable
                             where b.GroupId == groupId
                             select b).FirstOrDefault<GroupTableEntity>();
                group.IsDeleted = true;
                _dataContext.SubmitChanges();

                this.Groups.Remove(group);
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = ex.Message;
            }
        }

        public void CleanGroups()
        {
            if (Globals.CurrentProfile.IsDataSynced)
            {
                var groups = from GroupTableEntity g in _dataContext.MyGroupsTable
                             where g.IsDeleted == true
                             select g;
                _dataContext.MyGroupsTable.DeleteAllOnSubmit(groups);
                _dataContext.SubmitChanges();
            }
        }

        public void DeleteAllGroups()
        {
            var allGroups = from p in _dataContext.MyGroupsTable
                            select p;
            _dataContext.MyGroupsTable.DeleteAllOnSubmit(allGroups);
            _dataContext.SubmitChanges();

            this.Groups = new ObservableCollection<GroupTableEntity>();
        }

        /// <summary>
        /// Restore to be called with all Groups for all Profiles
        /// </summary>
        /// <param name="groupsFromServer"></param>
        public void RestoreProfileGroups(string profileId, ObservableCollection<MembershipServiceRef.Group> groupsFromServer)
        {
            this.Message = string.Empty;
            this.IsInProgress = true;
            IsSuccess = true;
            try
            {
                var groups = new ObservableCollection<GroupTableEntity>();

                foreach (var group in groupsFromServer)
                    groups.Add(ConvertGroup(group, profileId));

                _dataContext.MyGroupsTable.InsertAllOnSubmit(groups);
                _dataContext.SubmitChanges();

                this.IsInProgress = false;
                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                this.IsInProgress = false;
                IsSuccess = false;
                Message = ex.Message;
            }
        }

        /// <summary>
        /// Sync Groups between Guardian Server and Local Store
        /// </summary>
        public void SyncGroupsServer2Local(string profileId, ObservableCollection<MembershipServiceRef.Group> serverGroups)
        {
            //TODO - Remove groups, deactivated at server
            if (serverGroups != null)
            {
                foreach (var group in serverGroups)
                    UpdateGroup(profileId, ConvertGroup(group));

                LoadGroups(Globals.User.CurrentProfileId);
            }
        }

        public ObservableCollection<MembershipServiceRef.Group> GetAllCurrentProfileGroups()
        {
            ObservableCollection<MembershipServiceRef.Group> serverGroups = new ObservableCollection<MembershipServiceRef.Group>();
            foreach (var group in App.MyGroups.AllGroups)
            {
                serverGroups.Add(this.ConvertGroup(group));
            }
            return serverGroups;
        }

        #region Common Properties and Methods

        private bool _IsDataLoaded;
        public bool IsDataLoaded
        {
            get
            {
                return _IsDataLoaded;
            }
            set
            {
                if (value != _IsDataLoaded)
                {
                    _IsDataLoaded = value;
                    NotifyPropertyChanged("IsDataLoaded");
                }
            }
        }

        private bool _IsInProgress = false;
        public bool IsInProgress
        {
            get
            {
                return _IsInProgress;
            }
            set
            {
                if (value != _IsInProgress)
                {
                    _IsInProgress = value;
                    NotifyPropertyChanged("IsInProgress");
                }
            }
        }

        private bool _IsSuccess;
        public bool IsSuccess
        {
            get
            {
                return _IsSuccess;
            }
            set
            {
                if (value != _IsSuccess)
                {
                    _IsSuccess = value;
                    NotifyPropertyChanged("IsSuccess");
                }
            }
        }

        private string _message = string.Empty;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
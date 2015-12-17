using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace SOS.Phone
{
    public class BuddyViewModel : INotifyPropertyChanged
    {
        private SOSDataContext _dataContext;

        public BuddyViewModel()
        {
            _dataContext = new SOSDataContext(SOSDataContext.DBConnectionString);
        }

        private ObservableCollection<BuddyTableEntity> _buddies = null;
        public ObservableCollection<BuddyTableEntity> Buddies
        {
            get
            {
                return _buddies;
            }
            private set
            {
                _buddies = value;
                NotifyPropertyChanged("Buddies");
            }
        }

        public List<BuddyTableEntity> AllBuddies
        {
            get
            {
                List<BuddyTableEntity> allBuddies = (from BuddyTableEntity buddy in _dataContext.MyBuddiesTable
                                                     where buddy.MyProfileId == Globals.User.CurrentProfileId
                                                     //TODO: If buddy didnt sync with server and deleted, no need to send that info to Server
                                                     orderby buddy.Name ascending
                                                     select buddy).ToList();

                return allBuddies;
            }
        }

        private BuddyTableEntity ConvertBuddy(MembershipServiceRef.Buddy serverBuddy, string profileId = "")
        {
            var phoneBuddy = new BuddyTableEntity();

            phoneBuddy.BuddyRelationshipId = serverBuddy.BuddyID.ToString();
            phoneBuddy.MyProfileId = profileId == string.Empty ? Globals.User.CurrentProfileId : profileId;
            phoneBuddy.BuddyUserId = serverBuddy.UserID.ToString();
            phoneBuddy.Email = serverBuddy.Email;
            phoneBuddy.Name = serverBuddy.State == MembershipServiceRef.BuddyState.Suspended && !serverBuddy.Name.StartsWith("*") ? "*" + serverBuddy.Name : (serverBuddy.State == MembershipServiceRef.BuddyState.Marshal && !serverBuddy.Name.StartsWith("+") ? "+" + serverBuddy.Name : serverBuddy.Name);
            phoneBuddy.PhoneNumber = serverBuddy.MobileNumber;
            phoneBuddy.IsDeleted = serverBuddy.ToRemove;
            phoneBuddy.IsPrimeBuddy = serverBuddy.IsPrimeBuddy;

            phoneBuddy.OrderNumber = 0;
            phoneBuddy.BorderThickness = new Thickness(2);
            phoneBuddy.BuddyStatusColor = Constants.WhiteColorCode;
            phoneBuddy.State = serverBuddy.State;

            return phoneBuddy;
        }

        private MembershipServiceRef.Buddy ConvertBuddy(BuddyTableEntity phoneBuddy)
        {
            var serverBuddy = new MembershipServiceRef.Buddy();

            serverBuddy.BuddyID = Convert.ToInt64(phoneBuddy.BuddyRelationshipId);
            serverBuddy.UserID = Convert.ToInt64(phoneBuddy.BuddyUserId);
            serverBuddy.Email = phoneBuddy.Email;
            serverBuddy.Name = (phoneBuddy.Name.StartsWith("*") || phoneBuddy.Name.StartsWith("+")) ? phoneBuddy.Name.Substring(0, phoneBuddy.Name.Length - 1) : phoneBuddy.Name;
            serverBuddy.MobileNumber = phoneBuddy.PhoneNumber;
            serverBuddy.ToRemove = phoneBuddy.IsDeleted;
            serverBuddy.IsPrimeBuddy = phoneBuddy.IsPrimeBuddy;
            serverBuddy.State = phoneBuddy.State;
            return serverBuddy;
        }

        public void LoadBuddies(string profileId)
        {
            this.Message = string.Empty;
            this.IsInProgress = true;

            var buddiesInLocalStore = from BuddyTableEntity buddy in _dataContext.MyBuddiesTable
                                      where buddy.MyProfileId == profileId && buddy.IsDeleted == false
                                      orderby buddy.Name ascending
                                      select buddy;

            this.Buddies = new ObservableCollection<BuddyTableEntity>(buddiesInLocalStore);

            this.IsInProgress = false;
            this.IsDataLoaded = true;
        }

        public void AddBuddy(BuddyTableEntity newBuddy)
        {
            IsSuccess = true;
            Message = string.Empty;

            try
            {
                var buddyInLocalStore = (from BuddyTableEntity buddy in _dataContext.MyBuddiesTable
                                         where (buddy.PhoneNumber != "" && buddy.PhoneNumber == newBuddy.PhoneNumber)
                                         || (buddy.Email != "" && buddy.Email == newBuddy.Email)
                                         select buddy).FirstOrDefault();

                if (buddyInLocalStore != null)
                    buddyInLocalStore.IsDeleted = false;
                else
                    _dataContext.MyBuddiesTable.InsertOnSubmit(newBuddy);

                _dataContext.SubmitChanges();
                this.Buddies.Add(newBuddy);
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = ex.Message;
            }
        }

        public void UpdateBuddy(string profileId, BuddyTableEntity buddy)
        {
            var localBuddy = (from BuddyTableEntity b in _dataContext.MyBuddiesTable
                              where
                              (b.MyProfileId == profileId) && (
                              (b.BuddyRelationshipId != null && b.BuddyRelationshipId != string.Empty && buddy.BuddyRelationshipId == b.BuddyRelationshipId) ||
                              ((b.BuddyRelationshipId == null || b.BuddyRelationshipId == string.Empty) && b.PhoneNumber != null && b.PhoneNumber != string.Empty && b.PhoneNumber == buddy.PhoneNumber) ||
                              ((b.BuddyRelationshipId == null || b.BuddyRelationshipId == string.Empty) && (b.PhoneNumber == null || b.PhoneNumber == string.Empty) && b.Email != null && b.Email != string.Empty && b.Email == buddy.Email))
                              select b).FirstOrDefault();

            if (localBuddy != null)
            {
                localBuddy.BuddyRelationshipId = buddy.BuddyRelationshipId;
                localBuddy.MyProfileId = buddy.MyProfileId;
                localBuddy.BuddyUserId = buddy.BuddyUserId;
                localBuddy.Name = buddy.Name;
                localBuddy.PhoneNumber = buddy.PhoneNumber;
                localBuddy.Email = buddy.Email;
                localBuddy.IsDeleted = buddy.IsDeleted;
                localBuddy.IsPrimeBuddy = buddy.IsPrimeBuddy;

                localBuddy.BorderThickness = new Thickness(2);
                localBuddy.BuddyStatusColor = Constants.WhiteColorCode;
                localBuddy.OrderNumber = 0;
                localBuddy.State = buddy.State;
                _dataContext.SubmitChanges();
            }
            else if (!buddy.IsDeleted)
                AddBuddy(buddy);
        }

        public void DeleteBuddy(string buddyUserId)
        {
            var buddy = (from BuddyTableEntity b in this.Buddies//_dataContext.MyBuddiesTable
                         where b.BuddyUserId == buddyUserId
                         select b).FirstOrDefault<BuddyTableEntity>();

            DeleteBuddy(buddy);
        }

        public BuddyTableEntity GetPrimeBuddy(string profileId)
        {
            var buddy = (from BuddyTableEntity b in this.Buddies
                         where b.MyProfileId == profileId && b.IsPrimeBuddy
                         select b).FirstOrDefault<BuddyTableEntity>();
            return buddy;
        }

        public void DeleteBuddy(BuddyTableEntity buddy)
        {
            IsSuccess = true;
            Message = string.Empty;

            try
            {
                buddy.IsDeleted = true;
                _dataContext.SubmitChanges();

                this.Buddies.Remove(buddy);
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = ex.Message;
            }
        }

        public void CleanBuddies()
        {
            if (Globals.CurrentProfile.IsDataSynced)
            {
                var buddies = from BuddyTableEntity b in _dataContext.MyBuddiesTable
                              where b.IsDeleted == true
                              select b;
                _dataContext.MyBuddiesTable.DeleteAllOnSubmit(buddies);
                _dataContext.SubmitChanges();
            }
        }

        public void CleanBuddyIds(string newProfileId)
        {
            if (Globals.CurrentProfile != null && Globals.CurrentProfile.IsDataSynced)
            {
                var buddies = (from BuddyTableEntity b in _dataContext.MyBuddiesTable
                               select b).ToList();
                buddies.ForEach(delegate(BuddyTableEntity b)
                {
                    b.BuddyProfileId = string.Empty;
                    b.BuddyRelationshipId = string.Empty;
                    b.BuddyUserId = string.Empty;
                    b.MyProfileId = newProfileId;
                });

                _dataContext.SubmitChanges();

                this.Buddies = new ObservableCollection<BuddyTableEntity>(buddies);
            }
        }

        public void DeleteAllBuddies()
        {
            var allBuddies = from p in _dataContext.MyBuddiesTable
                             select p;
            _dataContext.MyBuddiesTable.DeleteAllOnSubmit(allBuddies);
            _dataContext.SubmitChanges();

            this.Buddies = new ObservableCollection<BuddyTableEntity>();
        }

        public void SetPrimeBuddy(string profileId, string phoneNumber)
        {
            IsSuccess = true;
            Message = string.Empty;

            try
            {
                var currentPrimeBuddy = (from p in _dataContext.MyBuddiesTable
                                         where p.MyProfileId == profileId && p.IsPrimeBuddy == true
                                         select p).FirstOrDefault();
                if (currentPrimeBuddy != null)
                    currentPrimeBuddy.IsPrimeBuddy = false;

                var buddy = (from BuddyTableEntity b in this.Buddies
                             where b.PhoneNumber == phoneNumber
                             && b.MyProfileId == profileId
                             select b).FirstOrDefault<BuddyTableEntity>();
                if (buddy != null)
                    buddy.IsPrimeBuddy = true;

                _dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = ex.Message;
            }
        }

        /// <summary>
        /// Restore to be called with all Buddies for all Profiles
        /// </summary>
        /// <param name="buddiesFromServer"></param>
        public void RestoreProfileBuddies(string profileId, ObservableCollection<SOS.Phone.MembershipServiceRef.Buddy> buddiesFromServer)
        {
            IsSuccess = true;
            Message = string.Empty;
            this.IsInProgress = true;

            try
            {
                var buddies = new ObservableCollection<BuddyTableEntity>();
                foreach (var buddy in buddiesFromServer)
                    buddies.Add(ConvertBuddy(buddy, profileId));

                _dataContext.MyBuddiesTable.InsertAllOnSubmit(buddies);
                _dataContext.SubmitChanges();

                this.IsInProgress = false;
                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = ex.Message;
            }
        }

        /// <summary>
        /// Sync Buddies between Guardian Server and Local Store
        /// </summary>
        public void SyncBuddiesServer2Local(string profileId, ObservableCollection<MembershipServiceRef.Buddy> serverBuddies)
        {
            if (serverBuddies != null)
            {
                foreach (var buddy in serverBuddies)
                    UpdateBuddy(profileId, ConvertBuddy(buddy));

                LoadBuddies(Globals.User.CurrentProfileId);
            }
        }

        public ObservableCollection<MembershipServiceRef.Buddy> GetAllCurrentProfileBuddies()
        {
            ObservableCollection<MembershipServiceRef.Buddy> serverBuddies = new ObservableCollection<MembershipServiceRef.Buddy>();
            foreach (var buddy in AllBuddies)
            {
                if (!(buddy.IsDeleted && buddy.BuddyUserId.GetValue() == string.Empty))
                    serverBuddies.Add(this.ConvertBuddy(buddy));
            }
            return serverBuddies;
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
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace SOS.Phone
{
    /* Use this formulae in excel to generate code for a Column
    ="private "&A1&" _"&LOWER(LEFT(B1,1))&MID(B1, 2,100)&";
        [Column]
        public "&A1&" "&B1&"
        {
            get
            {
                return _"&LOWER(LEFT(B1,1))&MID(B1, 2,100)&";
            }
            set
            {
                if (value != _"&LOWER(LEFT(B1,1))&MID(B1, 2,100)&")
                {
                    NotifyPropertyChanging('"&B1&"');
                    _"&LOWER(LEFT(B1,1))&MID(B1, 2,100)&" = value;
                    NotifyPropertyChanged('"&B1&"');
                }
            }
       }  
       "
     * After copy-paste, replace " with empty and ' with ".
     */
    [Table]
    public class UserTableEntity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _autoUserId;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL IDENTITY", AutoSync = AutoSync.OnInsert, CanBeNull = false)]
        public int AutoUserId
        {
            get { return _autoUserId; }
            set
            {
                if (_autoUserId != value)
                {
                    NotifyPropertyChanging("AutoUserId");
                    _autoUserId = value;
                    NotifyPropertyChanged("AutoUserId");
                }
            }
        }

        private string _userId;
        [Column]
        public string UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                if (value != _userId)
                {
                    NotifyPropertyChanging("UserId");
                    _userId = value;
                    NotifyPropertyChanged("UserId");
                }
            }
        }

        private string _name;
        [Column]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    NotifyPropertyChanging("Name");
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _liveEmail;
        [Column]
        public string LiveEmail
        {
            get
            {
                return _liveEmail;
            }
            set
            {
                if (value != _liveEmail)
                {
                    NotifyPropertyChanging("LiveEmail");
                    _liveEmail = value;
                    NotifyPropertyChanged("LiveEmail");
                }
            }
        }

        private string _liveAuthId;
        [Column]
        public string LiveAuthId
        {
            get
            {
                return _liveAuthId;
            }
            set
            {
                if (value != _liveAuthId)
                {
                    NotifyPropertyChanging("LiveAuthId");
                    _liveAuthId = value;
                    NotifyPropertyChanged("LiveAuthId");
                }
            }
        }

        private string _fbAuthId;
        [Column]
        public string FBAuthId
        {
            get
            {
                return _fbAuthId;
            }
            set
            {
                if (value != _fbAuthId)
                {
                    NotifyPropertyChanging("FBAuthId");
                    _fbAuthId = value;
                    NotifyPropertyChanged("FBAuthId");
                }
            }
        }

        

        //private bool _isProfileDataSynced;
        //[Column]
        //public bool IsProfileDataSynced
        //{
        //    get
        //    {
        //        return _isProfileDataSynced;
        //    }
        //    set
        //    {
        //        if (value != _isProfileDataSynced)
        //        {
        //            NotifyPropertyChanging("IsProfileDataSynced");
        //            _isProfileDataSynced = value;
        //            NotifyPropertyChanged("IsProfileDataSynced");
        //        }
        //    }
        //}

        //private bool _areBuddiesSynced;
        //[Column]
        //public bool AreBuddiesSynced
        //{
        //    get
        //    {
        //        return _areBuddiesSynced;
        //    }
        //    set
        //    {
        //        if (value != _areBuddiesSynced)
        //        {
        //            NotifyPropertyChanging("AreBuddiesSynced");
        //            _areBuddiesSynced = value;
        //            NotifyPropertyChanged("AreBuddiesSynced");
        //        }
        //    }
        //}

        //private bool _areGroupsSynced;
        //[Column]
        //public bool AreGroupsSynced
        //{
        //    get
        //    {
        //        return _areGroupsSynced;
        //    }
        //    set
        //    {
        //        if (value != _areGroupsSynced)
        //        {
        //            NotifyPropertyChanging("AreGroupsSynced");
        //            _areGroupsSynced = value;
        //            NotifyPropertyChanged("AreGroupsSynced");
        //        }
        //    }
        //}

        //// Version column aids update performance.
        //[Column(IsVersion = true)]
        //private Binary _version;

        //// Define the entity set for the collection side of the relationship.
        //private EntitySet<ProfileTableEntity> _profiles;
        //[Association(Storage = "_profiles", OtherKey = "_userId", ThisKey = "AutoUserId")]
        //public EntitySet<ProfileTableEntity> Profiles
        //{
        //    get { return this._profiles; }
        //    set { this._profiles.Assign(value); }
        //}

        //// Assign handlers for the add and remove operations, respectively.
        //public UserTableEntity()
        //{
        //    _profiles = new EntitySet<ProfileTableEntity>(
        //        new Action<ProfileTableEntity>(this.Attach_Profile),
        //        new Action<ProfileTableEntity>(this.Detach_Profile)
        //        );
        //}

        //// Called during an add operation
        //private void Attach_Profile(ProfileTableEntity profile)
        //{
        //    NotifyPropertyChanging("ProfileTableEntity");
        //    profile.User = this;
        //}

        //// Called during a remove operation
        //private void Detach_Profile(ProfileTableEntity profile)
        //{
        //    NotifyPropertyChanging("ProfileTableEntity");
        //    profile.User = null;
        //}

        //[Column]
        
        private string _currentProfileId;
        [Column]
        public string CurrentProfileId
        {
            get
            {
                return _currentProfileId;
            }
            set
            {
                if (value != _currentProfileId)
                {
                    NotifyPropertyChanging("CurrentProfileId");
                    _currentProfileId = value;
                    NotifyPropertyChanged("CurrentProfileId");
                }
            }
        }

        //private EntityRef<ProfileTableEntity> _currentProfile;

        //[Association(Storage = "_currentProfile", ThisKey = "_currentAutoProfileId", OtherKey = "AutoProfileId", IsForeignKey = true)]

        //private ProfileTableEntity _currentProfile;
        //public ProfileTableEntity CurrentProfile
        //{
        //    get { return _currentProfile; }
        //    set
        //    {
        //        if (value != _currentProfile)
        //        {
        //            NotifyPropertyChanging("CurrentProfile");
        //            _currentProfile = value;
        //            NotifyPropertyChanged("CurrentProfile");
        //        }
        //    }
        //}

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}

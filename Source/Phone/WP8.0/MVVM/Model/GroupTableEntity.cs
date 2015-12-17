using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Windows;

namespace SOS.Phone
{
    [Table]
    public class GroupTableEntity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _GID;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int GID
        {
            get
            {
                return _GID;
            }
            set
            {
                if (value != _GID)
                {
                    NotifyPropertyChanging("GID");
                    _GID = value;
                    NotifyPropertyChanged("GID");
                }
            }
        }

        private string _groupId;
        [Column]
        public string GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                if (value != _groupId)
                {
                    NotifyPropertyChanging("GroupId");
                    _groupId = value;
                    NotifyPropertyChanged("GroupId");
                }
            }
        }

        private string _myProfileId;
        [Column]
        public string MyProfileId
        {
            get
            {
                return _myProfileId;
            }
            set
            {
                if (value != _myProfileId)
                {
                    NotifyPropertyChanging("MyProfileId");
                    _myProfileId = value;
                    NotifyPropertyChanged("MyProfileId");
                }
            }
        }

        private string _name;
        [Column]
        public string Name
        {
            get
            {
                return (IsValidated) ? _name : (_name.StartsWith("*") ? _name : "*" + _name);
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

        private string _phoneNumber;
        [Column]
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                if (value != _phoneNumber)
                {
                    NotifyPropertyChanging("PhoneNumber");
                    _phoneNumber = value;
                    NotifyPropertyChanged("PhoneNumber");
                }
            }
        }

        private string _email;
        [Column]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    NotifyPropertyChanging("Email");
                    _email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        private MembershipServiceRef.GroupType _type;
        /// <summary>
        /// This field is used to determine next course of action, when user tries to add a group.
        /// </summary>
        /// <returns></returns>
        public MembershipServiceRef.GroupType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    NotifyPropertyChanging("Type");
                    _type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        private MembershipServiceRef.Enrollment _enrollmentType;
        /// <summary>
        /// This field is used to determine next course of action, when user tries to add a group.
        /// </summary>
        /// <returns></returns>
        [Column]
        public MembershipServiceRef.Enrollment EnrollmentType
        {
            get
            {
                return _enrollmentType;
            }
            set
            {
                if (value != _enrollmentType)
                {
                    NotifyPropertyChanging("EnrollmentType");
                    _enrollmentType = value;
                    NotifyPropertyChanged("EnrollmentType");
                }
            }
        }

        private string _enrollmentKey;
        /// <summary>
        /// This field is used to determine next course of action, when user tries to add a group.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string EnrollmentKey
        {
            get
            {
                return _enrollmentKey;
            }
            set
            {
                if (value != _enrollmentKey)
                {
                    NotifyPropertyChanging("EnrollmentKey");
                    _enrollmentKey = value;
                    NotifyPropertyChanged("EnrollmentKey");
                }
            }
        }

        private string _enrollmentValue;
        [Column]
        public string EnrollmentValue
        {
            get
            {
                return _enrollmentValue;
            }
            set
            {
                if (value != _enrollmentValue)
                {
                    NotifyPropertyChanging("EnrollmentValue");
                    _enrollmentValue = value;
                    NotifyPropertyChanged("EnrollmentValue");
                }
            }
        }

        private bool _isValidated;
        /// <summary>
        /// IsValidated flag to determine whether the Group is validated or not.
        /// </summary>
        /// <returns></returns>
        [Column]
        public bool IsValidated
        {
            get
            {
                return _isValidated;
            }
            set
            {
                if (value != _isValidated)
                {
                    NotifyPropertyChanging("IsValidated");
                    NotifyPropertyChanging("Name");
                    _isValidated = value;
                    NotifyPropertyChanged("IsValidated");
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private bool _isDeleted;
        /// <summary>
        /// IsDeleted flag to determine whether the Group is deleted or not.
        /// </summary>
        /// <returns></returns>
        [Column]
        public bool IsDeleted
        {
            get
            {
                return _isDeleted;
            }
            set
            {
                if (value != _isDeleted)
                {
                    NotifyPropertyChanging("IsDeleted");
                    _isDeleted = value;
                    NotifyPropertyChanged("IsDeleted");
                }
            }
        }

        //This property is only for registered buddies
        private string _buddyStatusColor;
        /// <summary>
        /// Buddy Status defines the buddy border color
        /// Safron- SOS; Green- Tracking
        /// </summary>
        /// <returns></returns>
        public string BuddyStatusColor
        {
            get
            {
                return _buddyStatusColor;
            }
            set
            {
                if (value != _buddyStatusColor)
                {
                    NotifyPropertyChanging("BuddyStatusColor");
                    _buddyStatusColor = value;
                    NotifyPropertyChanged("BuddyStatusColor");
                }
            }
        }

        private Thickness _borderThickness;
        /// <summary>
        /// Border Thickness of the buddy panel
        /// </summary>
        /// <returns></returns>
        public Thickness BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                if (value != _borderThickness)
                {
                    NotifyPropertyChanging("BorderThickness");
                    _borderThickness = value;
                    NotifyPropertyChanged("BorderThickness");
                }
            }
        }

        /// <summary>
        /// Dummy Property for binding
        /// </summary>
        public string LastLocation
        {
            get
            {
                return string.Empty;
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

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

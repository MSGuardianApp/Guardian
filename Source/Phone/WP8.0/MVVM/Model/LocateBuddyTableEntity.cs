using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Windows;

namespace SOS.Phone
{
    [Table]
    public class LocateBuddyTableEntity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _BID;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int BID
        {
            get
            {
                return _BID;
            }
            set
            {
                if (value != _BID)
                {
                    NotifyPropertyChanging("BID");
                    _BID = value;
                    NotifyPropertyChanged("BID");
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

        private string _buddyUserId;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string BuddyUserId
        {
            get
            {
                return _buddyUserId;
            }
            set
            {
                if (value != _buddyUserId)
                {
                    NotifyPropertyChanging("BuddyUserId");
                    _buddyUserId = value;
                    NotifyPropertyChanged("BuddyUserId");
                }
            }
        }

        private string _buddyProfileId;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string BuddyProfileId
        {
            get
            {
                return _buddyProfileId;
            }
            set
            {
                if (value != _buddyProfileId)
                {
                    NotifyPropertyChanging("BuddyProfileId");
                    _buddyProfileId = value;
                    NotifyPropertyChanged("BuddyProfileId");
                }
            }
        }

        private string _name;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
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

        //This property is only for registered buddies
        private string _shortTrackingURL;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string ShortTrackingURL
        {
            get
            {
                return _shortTrackingURL;
            }
            set
            {
                if (value != _shortTrackingURL)
                {
                    NotifyPropertyChanging("ShortTrackingURL");
                    _shortTrackingURL = value;
                    NotifyPropertyChanged("ShortTrackingURL");
                }
            }
        }

        private string _email;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
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

        //This property is only for registered buddies
        private string _lastLocation;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string LastLocation
        {
            get
            {
                return _lastLocation;
            }
            set
            {
                if (value != _lastLocation)
                {
                    NotifyPropertyChanging("LastLocation");
                    _lastLocation = value;
                    NotifyPropertyChanged("LastLocation");
                }
            }
        }

        private string _phoneNumber;
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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

        private string _trackingToken;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string TrackingToken
        {
            get
            {
                return _trackingToken;
            }
            set
            {
                if (value != _trackingToken)
                {
                    NotifyPropertyChanging("TrackingToken");
                    _trackingToken = value;
                    NotifyPropertyChanged("TrackingToken");
                }
            }
        }

        private bool _isDeleted;
        /// <summary>
        /// IsDeleted flag to determine whether the Buddy is deleted or not.
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
                return _buddyStatusColor == null ? Constants.WhiteColorCode : _buddyStatusColor;
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

        private int _orderNumber;
        /// <summary>
        /// Order by which Locate buddies have to be ordered.
        /// </summary>
        /// <returns></returns>
        public int OrderNumber
        {
            get
            {
                return _orderNumber == null ? 2 : _orderNumber;
            }
            set
            {
                if (value != _orderNumber)
                {
                    NotifyPropertyChanging("OrderNumber");
                    _orderNumber = value;
                    NotifyPropertyChanged("OrderNumber");
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
                return (_borderThickness == null || _borderThickness.Bottom == 0) ? new Thickness(2) : _borderThickness;
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

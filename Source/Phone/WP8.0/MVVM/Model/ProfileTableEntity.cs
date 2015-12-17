using Microsoft.Phone.Maps.Controls;
using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace SOS.Phone
{
    [Table]
    public class ProfileTableEntity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _autoProfileId;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL IDENTITY", AutoSync = AutoSync.OnInsert, CanBeNull = false)]
        public int AutoProfileId
        {
            get { return _autoProfileId; }
            set
            {
                NotifyPropertyChanging("AutoProfileId");
                _autoProfileId = value;
                NotifyPropertyChanged("AutoProfileId");
            }
        }

        private bool _postlocationConsent = true;
        [Column(CanBeNull = true)]
        public bool PostLocationConsent
        {
            get
            {
                return _postlocationConsent;
            }
            set
            {
                if (value != _postlocationConsent)
                {
                    NotifyPropertyChanging("PostLocationConsent");
                    _postlocationConsent = value;
                    NotifyPropertyChanged("PostLocationConsent");
                }
            }
        }

        private bool _isTrackingStatusSynced = true;
        [Column(CanBeNull = true)]
        public bool IsTrackingStatusSynced
        {
            get
            {
                return _isTrackingStatusSynced;
            }
            set
            {
                if (value != _isTrackingStatusSynced)
                {
                    NotifyPropertyChanging("IsTrackingStatusSynced");
                    _isTrackingStatusSynced = value;
                    NotifyPropertyChanged("IsTrackingStatusSynced");
                }
            }
        }

        private bool _isSOSStatusSynced = true;
        [Column(CanBeNull = true)]
        public bool IsSOSStatusSynced
        {
            get
            {
                return _isSOSStatusSynced;
            }
            set
            {
                if (value != _isSOSStatusSynced)
                {
                    NotifyPropertyChanging("IsSOSStatusSynced");
                    _isSOSStatusSynced = value;
                    NotifyPropertyChanged("IsSOSStatusSynced");
                }
            }
        }

        private DateTime? _lastSynced ;

        [Column(CanBeNull = true)]
        public DateTime? LastSynced
        {
            get { return _lastSynced; }
            set
            {
                if (_lastSynced != value)
                {
                    NotifyPropertyChanging("LastSynced");
                    _lastSynced = value;
                    NotifyPropertyChanged("LastSynced");
                }
            }
        }

        private MapCartographicMode? _mapView = MapCartographicMode.Road;

        [Column(CanBeNull = true)]
        public MapCartographicMode? MapView
        {
            get { return _mapView; }
            set
            {
                if (_mapView != value)
                {
                    NotifyPropertyChanging("MapView");
                    _mapView = value;
                    NotifyPropertyChanged("MapView");
                }
            }
        }

        private bool _isDataSynced;
        [Column]
        public bool IsDataSynced
        {
            get
            {
                return _isDataSynced;
            }
            set
            {
                if (value != _isDataSynced)
                {
                    NotifyPropertyChanging("IsDataSynced");
                    _isDataSynced = value;
                    NotifyPropertyChanged("IsDataSynced");
                }
            }
        }

        private string _profileId;
        [Column]
        public string ProfileId
        {
            get
            {
                return _profileId;
            }
            set
            {
                if (value != _profileId)
                {
                    NotifyPropertyChanging("ProfileId");
                    _profileId = value;
                    NotifyPropertyChanged("ProfileId");
                }
            }
        }

        private string _countryCode;
        [Column]
        public string CountryCode
        {
            get
            {
                return _countryCode;
            }
            set
            {
                if (value != _countryCode)
                {
                    NotifyPropertyChanging("CountryCode");
                    _countryCode = value;
                    NotifyPropertyChanged("CountryCode");
                }
            }
        }

        private string _mobileNumber;
        [Column]
        public string MobileNumber
        {
            get
            {
                return _mobileNumber;
            }
            set
            {
                if (value != _mobileNumber)
                {
                    NotifyPropertyChanging("MobileNumber");
                    _mobileNumber = value;
                    NotifyPropertyChanged("MobileNumber");
                }
            }
        }

        private string _sessionToken;
        [Column]
        public string SessionToken
        {
            get
            {
                return _sessionToken;
            }
            set
            {
                if (value != _sessionToken)
                {
                    NotifyPropertyChanging("SessionToken");
                    _sessionToken = value;
                    NotifyPropertyChanged("SessionToken");
                }
            }
        }

        private string _fBGroupId;
        [Column]
        public string FBGroupId
        {
            get
            {
                return _fBGroupId;
            }
            set
            {
                if (value != _fBGroupId)
                {
                    NotifyPropertyChanging("FBGroupId");
                    _fBGroupId = value;
                    NotifyPropertyChanged("FBGroupId");
                }
            }
        }

        private string _fBGroupName;
        [Column]
        public string FBGroupName
        {
            get
            {
                return _fBGroupName;
            }
            set
            {
                if (value != _fBGroupName)
                {
                    NotifyPropertyChanging("FBGroupName");
                    _fBGroupName = value;
                    NotifyPropertyChanged("FBGroupName");
                }
            }
        }

        private bool _locationConsent;
        [Column]
        public bool LocationConsent
        {
            get
            {
                return _locationConsent;
            }
            set
            {
                if (value != _locationConsent)
                {
                    NotifyPropertyChanging("LocationConsent");
                    _locationConsent = value;
                    NotifyPropertyChanged("LocationConsent");
                }
            }
        }

        private string _messageTemplate;
        [Column]
        public string MessageTemplate
        {
            get
            {
                return _messageTemplate;
            }
            set
            {
                if (value != _messageTemplate)
                {
                    NotifyPropertyChanging("MessageTemplate");
                    _messageTemplate = value;
                    NotifyPropertyChanged("MessageTemplate");
                }
            }
        }

        private bool _canSMS;
        [Column]
        public bool CanSMS
        {
            get
            {
                return _canSMS;
            }
            set
            {
                if (value != _canSMS)
                {
                    NotifyPropertyChanging("CanSendSMS");
                    _canSMS = value;
                    NotifyPropertyChanged("CanSendSMS");
                }
            }
        }

        private bool _canEmail;
        [Column]
        public bool CanEmail
        {
            get
            {
                return _canEmail;
            }
            set
            {
                if (value != _canEmail)
                {
                    NotifyPropertyChanging("CanSendEmail");
                    _canEmail = value;
                    NotifyPropertyChanged("CanSendEmail");
                }
            }
        }

        private bool _canFBPost;
        [Column]
        public bool CanFBPost
        {
            get
            {
                return _canFBPost;
            }
            set
            {
                if (value != _canFBPost)
                {
                    NotifyPropertyChanging("CanSendFBPost");
                    _canFBPost = value;
                    NotifyPropertyChanged("CanSendFBPost");
                }
            }
        }

        private string _canArchiveEvidence;
        [Column]
        public string CanArchiveEvidence
        {
            get
            {
                return _canArchiveEvidence;
            }
            set
            {
                if (value != _canArchiveEvidence)
                {
                    NotifyPropertyChanging("CanArchiveEvidence");
                    _canArchiveEvidence = value;
                    NotifyPropertyChanged("CanArchiveEvidence");
                }
            }
        }

        private string _archiveFolder;
        [Column]
        public string ArchiveFolder
        {
            get
            {
                return _archiveFolder;
            }
            set
            {
                if (value != _archiveFolder)
                {
                    NotifyPropertyChanging("ArchiveFolder");
                    _archiveFolder = value;
                    NotifyPropertyChanged("ArchiveFolder");
                }
            }
        }

        private string _tinyUri;
        [Column]
        public string TinyUri
        {
            get
            {
                return _tinyUri;
            }
            set
            {
                if (value != _tinyUri)
                {
                    NotifyPropertyChanging("TinyUri");
                    _tinyUri = value;
                    NotifyPropertyChanged("TinyUri");
                }
            }
        }

        private bool _isTrackingOn;
        [Column]
        public bool IsTrackingOn
        {
            get
            {
                return _isTrackingOn;
            }
            set
            {
                if (value != _isTrackingOn)
                {
                    NotifyPropertyChanging("IsTrackingOn");
                    _isTrackingOn = value;
                    NotifyPropertyChanged("IsTrackingOn");
                }
            }
        }

        private bool _isSOSOn;
        [Column]
        public bool IsSOSOn
        {
            get
            {
                return _isSOSOn;
            }
            set
            {
                if (value != _isSOSOn)
                {
                    NotifyPropertyChanging("IsSOSOn");
                    _isSOSOn = value;
                    NotifyPropertyChanged("IsSOSOn");
                }
            }
        }

        private bool _allowOthersToTrackYou;
        [Column]
        public bool AllowOthersToTrackYou
        {
            get
            {
                return _allowOthersToTrackYou;
            }
            set
            {
                if (value != _allowOthersToTrackYou)
                {
                    NotifyPropertyChanging("AllowOthersToTrackYou");
                    _allowOthersToTrackYou = value;
                    NotifyPropertyChanged("AllowOthersToTrackYou");
                }
            }
        }

        private string _deviceId;
        [Column]
        public string DeviceId
        {
            get
            {
                return _deviceId;
            }
            set
            {
                if (value != _deviceId)
                {
                    NotifyPropertyChanging("DeviceId");
                    _deviceId = value;
                    NotifyPropertyChanged("DeviceId");
                }
            }
        }

        private string _platform;
        [Column]
        public string Platform
        {
            get
            {
                return _platform;
            }
            set
            {
                if (value != _platform)
                {
                    NotifyPropertyChanging("Platform");
                    _platform = value;
                    NotifyPropertyChanged("Platform");
                }
            }
        }

        private string _policeContact;
        /// <summary>
        /// Country police emergency number.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string PoliceContact
        {
            get
            {
                return _policeContact;
            }
            set
            {
                if (value != _policeContact)
                {
                    NotifyPropertyChanging("PoliceContact");
                    _policeContact = value;
                    NotifyPropertyChanged("PoliceContact");
                }
            }
        }

        private string _ambulanceContact;
        /// <summary>
        /// Country police emergency number.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string AmbulanceContact
        {
            get
            {
                return _ambulanceContact;
            }
            set
            {
                if (value != _ambulanceContact)
                {
                    NotifyPropertyChanging("AmbulanceContact");
                    _ambulanceContact = value;
                    NotifyPropertyChanged("AmbulanceContact");
                }
            }
        }

        private string _fireContact;
        /// <summary>
        /// Country police emergency number.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string FireContact
        {
            get
            {
                return _fireContact;
            }
            set
            {
                if (value != _fireContact)
                {
                    NotifyPropertyChanging("FireContact");
                    _fireContact = value;
                    NotifyPropertyChanged("FireContact");
                }
            }
        }

        private string _countryName;
        /// <summary>
        /// Country name.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string CountryName
        {
            get
            {
                return _countryName;
            }
            set
            {
                if (value != _countryName)
                {
                    NotifyPropertyChanging("CountryName");
                    _countryName = value;
                    NotifyPropertyChanged("CountryName");
                }
            }
        }


        private string _maxPhonedigits;
        /// <summary>
        /// Country name.
        /// </summary>
        /// <returns></returns>
        [Column]
        public string MaxPhonedigits
        {
            get
            {
                return _maxPhonedigits;
            }
            set
            {
                if (value != _maxPhonedigits)
                {
                    NotifyPropertyChanging("MaxPhonedigits");
                    _maxPhonedigits = value;
                    NotifyPropertyChanged("MaxPhonedigits");
                }
            }
        }

        private string _notificationUri;
        [Column]
        public string NotificationUri
        {
            get
            {
                return _notificationUri;
            }
            set
            {
                if (value != _notificationUri)
                {
                    NotifyPropertyChanging("NotificationUri");
                    _notificationUri = value;
                    NotifyPropertyChanged("NotificationUri");
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

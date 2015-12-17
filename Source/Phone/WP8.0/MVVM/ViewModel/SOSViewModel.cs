using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SOS.Phone
{
    public class SOSViewModel : INotifyPropertyChanged
    {

        public SOSViewModel()
        {
        }

        public ObservableCollection<BuddyTableEntity> Buddies
        {
            get
            {
                return App.MyBuddies.Buddies.Count > 0 ? App.MyBuddies.Buddies : null;
            }

        }

        public ObservableCollection<GroupTableEntity> Groups
        {
            get
            {
                return App.MyGroups.Groups.Count > 0 ? App.MyGroups.Groups : null;
            }

        }

        private List<Helpline> helplines = null;
        public List<Helpline> Helplines
        {
            get
            {
                if (helplines == null)
                {
                    helplines = GetHelplines();
                }
                return helplines;
            }
            set { helplines = value; }

        }

        private List<Helpline> GetHelplines()
        {
            XElement root = XElement.Load("Content/Helplines.xml");
            XElement country = (from el in root.Elements("Country")
                                where (string)el.Attribute("IsdCode") == Globals.CurrentProfile.CountryCode
                                select el).FirstOrDefault();

            List<Helpline> helplines = null;
            if (country != null)
            {
                helplines = (from el in country.Elements("Helpline")
                             select new Helpline()
                             {
                                 Name = (string)el.Attribute("Name"),
                                 PhoneNumber = (string)el.Attribute("PhoneNumber")
                             }).ToList();

            }

            return helplines;
        }

        private Visibility _errorMessageVisibility = Visibility.Collapsed;
        public Visibility ErrorMessageVisibility
        {
            get
            {
                return _errorMessageVisibility;
            }
            set
            {
                _errorMessageVisibility = value;
                NotifyPropertyChanged("ErrorMessageVisibility");
            }
        }

        private Visibility _panicMessageVisibility = Visibility.Visible;
        public Visibility PanicMessageVisibility
        {
            get
            {
                return _panicMessageVisibility;
            }
            set
            {
                _panicMessageVisibility = value;
                NotifyPropertyChanged("PanicMessageVisibility");
            }
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

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (value != _errorMessage)
                {
                    _errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        public void UpdateErrorStatus(bool status)
        {
            if (status)
            {
                this.ErrorMessageVisibility = Visibility.Visible;
                this.PanicMessageVisibility = Visibility.Collapsed;

                if (!Globals.IsSyncedFirstTime) ErrorMessage = "*Unable to reach the server. While keep retrying, please look for help from others.";
                else ErrorMessage = "*Your buddies are informed but unable to send latest recorded locations.";
            }
            else
            {
                this.ErrorMessageVisibility = Visibility.Collapsed;
                this.PanicMessageVisibility = Visibility.Visible;
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SOS.Phone.MVVM.Model
{
    class ContactsModel : INotifyPropertyChanged, INotifyPropertyChanging
    {

        private List<string> _phoneNumbers;
        public List<string> PhoneNumbers
        {
            get
            {
                return _phoneNumbers;
            }
            set
            {
                if (value != _phoneNumbers)
                {
                    NotifyPropertyChanging("PhoneNumbers");
                    _phoneNumbers = value;
                    NotifyPropertyChanged("PhoneNumbers");
                }
            }
        }

        private string _phoneNumber;
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

        private string _fullName;
        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                if (value != _fullName)
                {
                    NotifyPropertyChanging("FullName");
                    _fullName = value;
                    NotifyPropertyChanged("FullName");
                }
            }
        }

        //private string _lastName;
        //public string LastName
        //{
        //    get
        //    {
        //        return _lastName;
        //    }
        //    set
        //    {
        //        if (value != _lastName)
        //        {
        //            NotifyPropertyChanging("LastName");
        //            _lastName = value;
        //            NotifyPropertyChanged("LastName");
        //        }
        //    }
        //}

        private List<string> _emailAddresses;
        public List<string> EmailAddresses
        {
            get
            {
                return _emailAddresses;
            }
            set
            {
                if (value != _emailAddresses)
                {
                    NotifyPropertyChanging("EmailAddresses");
                    _emailAddresses = value;
                    NotifyPropertyChanged("EmailAddresses");
                }
            }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                if (value != _emailAddress)
                {
                    NotifyPropertyChanging("EmailAddress");
                    _emailAddress = value;
                    NotifyPropertyChanged("EmailAddress");
                }
            }
        }

        private string _displayname;
        public string DisplayName
        {
            get
            {
                return _displayname;
            }
            set
            {
                if (value != _displayname)
                {
                    NotifyPropertyChanging("DisplayName");
                    _displayname = value;
                    NotifyPropertyChanged("DisplayName");
                }
            }
        }

        private Stream _imageStream;
        public Stream ImageStream
        {
            get
            {
                return _imageStream;
            }
            set
            {
                if (value != _imageStream)
                {
                    NotifyPropertyChanging("ImageStream");
                    _imageStream = value;
                    NotifyPropertyChanged("ImageStream");
                }
            }
        }
                
        private string _isdCode;
        public string IsdCode
        {

            get
            {
                return _isdCode;
            }
            set
            {
                if (value != _isdCode)
                {
                    NotifyPropertyChanging("IsdCode");
                    _isdCode = value;
                    NotifyPropertyChanged("IsdCode");
                }
            }
        }

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

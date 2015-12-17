using System;
using System.ComponentModel;

namespace SOS.Phone.MVVM.Model
{
    public class HealthViewModel : INotifyPropertyChanged
    {
        private HealthTableEntity _healthEntity = null;
        //public HealthTableEntity HealthEntityObject
        //{
        //    get
        //    {
        //        return _healthEntity;
        //    }
        //    private set
        //    {
        //        _healthEntity = value;
        //        NotifyPropertyChanged("HealthEntity");
        //    }
        //}

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

        private void CallErrorHandler()
        {
            IsInProgress = false;
            Message = "Unable to retrieve the latest information...";
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

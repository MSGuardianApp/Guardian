using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace SOS.Phone
{
    [Table]
    class HealthTableEntity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private Constants.HealthStatus _systemHealth;
        [Column]
        public Constants.HealthStatus SystemHealth
        {
            get
            {
                return _systemHealth;
            }
            set
            {
                if (value != _systemHealth)
                {
                    NotifyPropertyChanging("SystemHealth");
                    _systemHealth = value;
                    NotifyPropertyChanged("SystemHealth");
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

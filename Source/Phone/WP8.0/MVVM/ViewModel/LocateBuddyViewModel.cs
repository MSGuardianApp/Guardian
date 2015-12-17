using SOS.Phone.LocationServiceRef;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SOS.Phone.ServiceWrapper;

namespace SOS.Phone
{
    public class LocateBuddyViewModel : INotifyPropertyChanged
    {
        private SOSDataContext _dataContext;
        public LocateBuddyViewModel()
        {
            _dataContext = new SOSDataContext(SOSDataContext.DBConnectionString);

            //if (_dataContext.LocateBuddiesTable.Count() <= 0)
            //    this.LocateBuddies = new ObservableCollection<LocateBuddyTableEntity>();
        }

        private ObservableCollection<LocateBuddyTableEntity> _LocateBuddies = null;
        public ObservableCollection<LocateBuddyTableEntity> LocateBuddies
        {
            get
            {
                return _LocateBuddies;
            }
            private set
            {
                _LocateBuddies = value;
                NotifyPropertyChanged("LocateBuddies");
            }
        }

        /// <summary>
        /// Design: 
        /// 1. Always load from Isolated/ Local store.
        /// 2. Maintain a flag/timestamp(IsNewDataAvailable) in local store that data has been updated be it Locate buddies or phone number of buddy etc
        /// 3. If already loaded and IsNewDataAvailable flag is false, do not load again
        /// 4. In background for every 5 min or when app starts,asynchornously sync data with Azure via Service
        /// 5. If there are any changes, update the local store and load the data when user loads the page(steps 1,2 & 3)
        /// </summary>
        public void LoadLocateBuddies()
        {
            //this.LocateBuddies = LocalStorageWrapper.GetLocateBuddies();
            var locateBuddiesInLocalStore = from LocateBuddyTableEntity b in _dataContext.LocateBuddiesTable
                                            orderby b.Name ascending
                                            select b;

            this.LocateBuddies = new ObservableCollection<LocateBuddyTableEntity>(locateBuddiesInLocalStore);
        }

        public void ClearLocateBuddies()
        {
            var locateBuddies = from LocateBuddyTableEntity lb in _dataContext.LocateBuddiesTable
                                select lb;
            _dataContext.LocateBuddiesTable.DeleteAllOnSubmit(locateBuddies);
            _dataContext.SubmitChanges();
        }

        public async Task<bool> RefreshLocateBuddies()
        {
            try
            {
                this.Message = string.Empty;

                if (Globals.IsDataNetworkAvailable && Globals.IsRegisteredUser)
                {
                    IsInProgress = true;
                    await LocationServiceWrapper.GetLocateMembersAsync(this.GetLocateMembersList);

                    if (LocationServiceWrapper.IsErrorOccured == true)
                    {
                        Message = "Unable to retrieve latest information about your Locate Buddies...";
                    }
                }
            }
            catch (Exception)
            {
                this.Message = "Unable to retrieve latest information about your Locate Buddies...";
            }
            IsInProgress = false;
            return true;
        }

        private void GetLocateMembersList(ProfileLiteList oLocateMembersList)
        {
            try
            {
                if (oLocateMembersList == null && LocationServiceWrapper.IsErrorOccured == true)
                {
                    this.Message = "We are having issues in retrieving your data. Please try again later.";
                }
                else if ((oLocateMembersList == null || oLocateMembersList.List == null || oLocateMembersList.List.Count == 0) && (this.LocateBuddies.Count < 1))
                {
                    this.Message = "You have no buddies to track.";
                    ClearLocateBuddies();
                }
                else if (oLocateMembersList != null && oLocateMembersList.List != null)
                {
                    foreach (var item in oLocateMembersList.List)//.OrderByDescending(b => b.IsSOSOn).ThenByDescending(b => b.IsTrackingOn))
                    {
                        string backStatusColor = item.IsSOSOn ? Constants.SaffronColorCode : (item.IsTrackingOn ? Constants.GreenColorCode : Constants.WhiteColorCode);
                        int order = item.IsSOSOn ? 6 : (item.IsTrackingOn ? 4 : 2);//These numbers are used for Border Thickness as well.
                        var buddy = new LocateBuddyTableEntity() { BuddyProfileId = item.ProfileID.ToString(), Name = item.Name, Email = item.Email, PhoneNumber = item.MobileNumber, BuddyStatusColor = backStatusColor, TrackingToken = item.SessionID, OrderNumber = order, BorderThickness = new Thickness(order) };

                        UpdateLocateBuddy(buddy);

                        if (item.LastLocs != null && item.LastLocs.Count > 0)
                            GetLocationAddressAsync(item.LastLocs, item.ProfileID.ToString());//TODO: Optimize
                    }

                    if (this.LocateBuddies.Count > 0)
                    {
                        if (this.LocateBuddies.Count != oLocateMembersList.List.Count)
                            DeleteLocateBuddies(oLocateMembersList.List);

                        this.LocateBuddies.Sort(b => b.OrderNumber, ListSortDirection.Descending);
                    }
                }
            }
            catch (Exception)
            {
                CallErrorHandler();
            }
            IsInProgress = false;
        }

        private void UpdateLocateBuddy(LocateBuddyTableEntity buddy)
        {
            //BL: Delete buddies who are removed - Do this one time while loading the app asynchronously and update ISO
            //BL: Refresh status changes and add new Locate Buddies
            try
            {
                if (this.LocateBuddies.Any(b => b.BuddyProfileId == buddy.BuddyProfileId))
                {
                    var buddyItem = this.LocateBuddies.First(b => b.BuddyProfileId == buddy.BuddyProfileId);
                    buddyItem.Name = buddy.Name;
                    buddyItem.BorderThickness = buddy.BorderThickness;
                    buddyItem.OrderNumber = buddy.OrderNumber;
                    buddyItem.BuddyStatusColor = buddy.BuddyStatusColor;
                }
                else
                {
                    this.LocateBuddies.Add(buddy);
                    //Save to local storage
                    _dataContext.LocateBuddiesTable.InsertOnSubmit(buddy);
                    _dataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            { }
        }

        private void DeleteLocateBuddies(ObservableCollection<ProfileLite> serverLocateBuddies)
        {
            try
            {
                var locateBuddies = this.LocateBuddies;
                foreach (var buddy in locateBuddies)
                {
                    if (serverLocateBuddies.Any(b => b.ProfileID.ToString() == buddy.BuddyProfileId))
                    {
                        //BL: Do Nothing
                    }
                    else
                    {
                        this.LocateBuddies.Remove(buddy);
                        var dbBuddy = (from db in _dataContext.LocateBuddiesTable
                                       where db.BuddyProfileId == buddy.BuddyProfileId
                                       select db).FirstOrDefault();
                        if (dbBuddy != null)
                            _dataContext.LocateBuddiesTable.DeleteOnSubmit(dbBuddy);
                    }
                }
            }
            finally
            {
                _dataContext.SubmitChanges();
            }
        }

        private Task<bool> RefreshAllLocateBuddies()
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                if (_dataContext.LocateBuddiesTable.Count() > 0)
                {
                    var dbLocateBuddies = (from b in _dataContext.LocateBuddiesTable
                                           select b).ToList();
                    _dataContext.LocateBuddiesTable.DeleteAllOnSubmit(dbLocateBuddies);
                    _dataContext.SubmitChanges();
                }

                var locateBuddies = (from item in this.LocateBuddies
                                     select new LocateBuddyTableEntity
                                                {
                                                    BuddyProfileId = item.BuddyProfileId,
                                                    BuddyUserId = item.BuddyUserId,
                                                    Email = item.Email,
                                                    PhoneNumber = item.PhoneNumber,
                                                    LastLocation = item.LastLocation,
                                                    Name = item.Name,
                                                    ShortTrackingURL = item.ShortTrackingURL,
                                                    TrackingToken = item.TrackingToken,
                                                    //BuddyStatusColor = Constants.WhiteColorCode,
                                                    //OrderNumber = item.OrderNumber
                                                }).ToList();

                _dataContext.LocateBuddiesTable.InsertAllOnSubmit(locateBuddies);
                _dataContext.SubmitChanges();

                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                tcs.SetResult(false);
            }
            return tcs.Task;
        }

        private async void GetLocationAddressAsync(ObservableCollection<GeoTag> locationTag, string profileId)
        {
            try
            {
                string address = await Utility.GetCombOfBingAndGMapsAddress(locationTag[0].Lat, locationTag[0].Long);

                if (this.LocateBuddies.Any(b => b.BuddyProfileId == profileId))
                {
                    this.LocateBuddies.First(b => b.BuddyProfileId == profileId).LastLocation = "@ " + address + " - " + new DateTime(locationTag[0].TimeStamp).ToString("dd/MM/yyyy HH:mm:ss");

                    using (var dataContext = new SOSDataContext(SOSDataContext.DBConnectionString))
                    {
                        if (dataContext.LocateBuddiesTable.Count() > 0)
                        {
                            //TODO: Bug - if uses Single, then it is throwing error. This means there are multiple entries in LocateBuddies with Same buddyId
                            dataContext.LocateBuddiesTable.First(b => b.BuddyProfileId == profileId).LastLocation = "( was at " + address + " - " + new DateTime(locationTag[0].TimeStamp).ToString("dd/MM/yyyy HH:mm:ss") + ")";
                            dataContext.SubmitChanges();
                        }
                    }
                }
            }
            catch
            {
                //absorb exception occurred while getting address
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using Guardian.Common;
using SOS.EventHubReceiver;
using SOS.Mappers;
using SOS.Model;
using SOS.Service.Interfaces;
using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using SOS.Service.Utility;
using LiveUserStatus = SOS.AzureSQLAccessLayer.LiveUserStatus;

namespace SOS.Service.Implementation
{
    /// <summary>
    ///     Provides Location query service of various entities, especially members for and buddies.
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class LocationService : ILocationService
    {
        private readonly bool IsEventHubEnabled = Config.UseEventHubs;
        private readonly MemberService _MS = new MemberService();

        private readonly Authorization _authService;
        private readonly LocationRepository _locRepository;
        private LocationHistoryStorageAccess _GPSA;

        public LocationService()
        {
            _locRepository = new LocationRepository();
            _authService = new Authorization();
            _GPSA = new LocationHistoryStorageAccess();
        }

        /// <summary>
        /// </summary>
        /// <param name="ProfileID"></param>
        /// <param name="LastUpdateTime"></param>
        /// <returns>Returns null if no location detials</returns>
        public async Task<ProfileLite> GetUserLocation(string profileID, string lastUpdateTime)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            long ProfileID = Convert.ToInt64(profileID);
            long LastUpdateTime = Convert.ToInt64(lastUpdateTime);
            ProfileLite rMemLite = new ProfileLite();

            bool isAuthorizedForSelf = await _authService.SelfAccess(liveUserID, ProfileID);
            bool isAuthorizedForLocateBuddy = await _authService.LocateBuddyAccess(liveUserID, ProfileID);
            if (isAuthorizedForSelf || isAuthorizedForLocateBuddy)
            {
                var geoLiveLocation = await _locRepository.GetLocationData(ProfileID, LastUpdateTime);
                    if (geoLiveLocation == null && geoLiveLocation.ToList().Count == 0)
                    {
                        ResultsManager.AddResultInfo(rMemLite, ResultTypeEnum.Information, "NOLOCDTL:" + profileID);
                        return rMemLite;
                    }


                    if (geoLiveLocation != null && geoLiveLocation.ToList().Count > 0)
                    {
                        List<GeoTag> sGeo = geoLiveLocation.ToList().ConvertToGeoTagList();
                        rMemLite.LastLocs = sGeo;
                    }
                }
                else
                {
                Utility.ResultsManager.AddResultInfo(rMemLite, ResultTypeEnum.AuthError, "You are not authorized to view locations for this profile");
            }

            return rMemLite;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileID"></param>
        /// <param name="Token"></param>
        /// <param name="lastUpdateTime"></param>
        /// <returns></returns>
        public async Task<BasicProfile> GetUserLocationsByToken(string profileID, string sessionID, string lastUpdateTime)
        {
            long ProfileID = Convert.ToInt64(profileID);
            long LastUpdateTime = Convert.ToInt64(lastUpdateTime);

            var retprofile = new BasicProfile();

            List<LiveLocation> tokenizedLocationList =
                await _locRepository.GetLocationDataByToken(ProfileID, sessionID, LastUpdateTime);

            if (tokenizedLocationList == null || tokenizedLocationList.Count == 0)
            {
                ResultsManager.AddResultInfo(retprofile, ResultTypeEnum.Error,
                    "No Loation details for Profile:" + profileID);
                return retprofile;
            }

            List<BasicGeoTag> sGeo = tokenizedLocationList.ConvertToBasicGeoTagList();
            retprofile.LastLocs = sGeo;

            BasicGeoTag lastGeoLoc = sGeo[sGeo.Count - 1];
            retprofile.IsSOSOn = lastGeoLoc.IsSOS.HasValue ? lastGeoLoc.IsSOS.Value : false; //1
            retprofile.IsTrackingOn = !(lastGeoLoc.IsSOS.HasValue ? lastGeoLoc.IsSOS.Value : false) ||
                                      !string.IsNullOrEmpty(lastGeoLoc.Lat); //0,1

            return retprofile;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileID"></param>
        /// <param name="lastUpdateTimeTicks"></param>
        /// <returns></returns>
        public async Task<GeoTagList> GetUserLocations(string profileID, string lastUpdateTimeTicks,
            string GroupID = "0")
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            long ProfileID = Convert.ToInt64(profileID);
            long LastUpdateTimeTicks = Convert.ToInt64(lastUpdateTimeTicks);

            bool isAuthorized = false;
            if (UType != null && Convert.ToChar(UType) == 'a')
                isAuthorized = await _authService.OwnGroupMembersAccess(liveUserID, Convert.ToInt32(GroupID), ProfileID);
            else
            {
                bool isAuthorizedForSelf = await _authService.SelfAccess(liveUserID, ProfileID);
                bool isAuthorizedForLocateBuddy = await _authService.LocateBuddyAccess(liveUserID, ProfileID);
                if (isAuthorizedForSelf || isAuthorizedForLocateBuddy)
                    isAuthorized = true;
            }
            var geoTagList = new GeoTagList();
            if (isAuthorized)
            {
                List<LiveLocation> geoLiveLocation =
                    (await _locRepository.GetLocationData(ProfileID, LastUpdateTimeTicks)).ToList();

                List<GeoTag> LiveLocationData = geoLiveLocation.ConvertToGeoTagList();
                geoTagList.List = LiveLocationData;
            }
            else
            {
                ResultsManager.AddResultInfo(geoTagList, ResultTypeEnum.AuthError,
                    "You are not authorized to view locations for this profile");
            }

            return geoTagList;
        }

        /// <summary>
        /// </summary>
        /// <param name="profileID"></param>
        /// <param name="lastUpdateTime"></param>
        /// <returns></returns>
        public async Task<GeoTags> GetUserLocationArray(string profileID, string lastUpdateTime)
        {
            long ProfileID = Convert.ToInt64(profileID);
            long LastUpdateTime = Convert.ToInt64(lastUpdateTime);

            GeoTags result = null;
            ProfileLite returnList = await GetUserLocation(profileID, lastUpdateTime); // LiveLocationData
            if (returnList != null && returnList.LastLocs != null && returnList.LastLocs.Count > 0)
            {
                int cnt = returnList.LastLocs.Count;

                var isSOSArr = new bool[cnt];
                var latArr = new string[cnt];
                var longArr = new string[cnt];
                var timeArr = new long[cnt];
                var speedArr = new int[cnt];
                var accuracyArr = new double[cnt];
                var altArr = new string[cnt];

                int i = 0;

                foreach (GeoTag gt in returnList.LastLocs)
                {
                    isSOSArr[i] = gt.IsSOS == null ? false : gt.IsSOS.Value;

                    latArr[i] = gt.Lat;

                    longArr[i] = gt.Long;

                    timeArr[i] = gt.TimeStamp;

                    speedArr[i] = gt.Speed;

                    accuracyArr[i] = gt.Accuracy;

                    altArr[i] = gt.Alt;

                    i++;
                }

                result = new GeoTags
                {
                    PID = ProfileID,
                    IsSOS = isSOSArr,
                    Lat = latArr,
                    Long = longArr,
                    TS = timeArr,
                    LocCnt = cnt,
                    Spd = speedArr,
                    Accuracy = accuracyArr,
                    Alt = altArr
                };
            }

            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ProfileLiteList> GetBuddiesToLocate(string userID)
        {
            long UserID = Convert.ToInt64(userID);
            var oReturn = new ProfileLiteList();

            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            //TODO: remove userID as input and continue with liveuserid
            bool isAuthorized = await _authService.ValidUserAccess(liveUserID, UserID);

            if (isAuthorized)
            {
                List<ProfileLite> budList = await GetLocateBuddies(UserID);

                if (budList != null && budList.Count > 0)
                    oReturn.List = budList;
                else
                    ResultsManager.AddResultInfo(oReturn, ResultTypeEnum.Error,
                        "Dependent List not loaded for UserID:" + userID);
            }
            else
            {
                ResultsManager.AddResultInfo(oReturn, ResultTypeEnum.AuthError,
                    "You are not authorized to access this method");
            }

            return oReturn;
        }

        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="dummyTicks"></param>
        /// <returns></returns>
        public async Task<ProfileLiteList> GetBuddiesToLocateLastLocation(string userID, string dummyTicks)
        {
            long UserID = Convert.ToInt64(userID);

            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            bool isAuthorized = await _authService.ValidUserAccess(liveUserID, UserID);

            var profileLiteList = new ProfileLiteList();
            if (isAuthorized)
            {
                profileLiteList.List = await GetLocateBuddies(UserID);
            }
            else
            {
                ResultsManager.AddResultInfo(profileLiteList, ResultTypeEnum.AuthError,
                    "You are not authorized to access this method");
            }

            return profileLiteList;
        }

        /// <summary>
        ///     Find IsSOSON and IsTrackingON for the buddies associated with USERID
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="DummyTicks"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetSOSTrackCount(string userID, string dummyTicks)
            //Change based on the query
        {
            long UserID = Convert.ToInt64(userID);
            Dictionary<string, string> result = await _locRepository.GetSOSTrackCountAsync(UserID);

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public async Task<SOSTrackCounts> GetSOSTrackCountByGroupId(string groupID)
        {
            int GroupID = Convert.ToInt32(groupID);
            SOSTrackInfo _sosTrackInfo = await _locRepository.GetSOSTrackCountByGroupId(GroupID);

            return _sosTrackInfo.ConvertToSOSTrackCounts();
        }

        public async Task<ProfileLite> GetLocationDetails(string ProfileID, string GroupID, string LastUpdateDateTicks)
        {
            //note: here only location details are returned, no profile and user details
            var resProfileLite = new ProfileLite();

            GeoTagList listLocationData = await GetUserLocations(ProfileID, LastUpdateDateTicks, GroupID);

            if (listLocationData == null || listLocationData.List == null || listLocationData.List.Count < 1)
            {
                resProfileLite.IsSOSOn = false;
                resProfileLite.IsTrackingOn = false;
            }
            else
            {
                resProfileLite.ProfileID = Convert.ToInt64(ProfileID);
                //just passing avaialable info, imp is location details alone
                GeoTag latest = listLocationData.List[listLocationData.List.Count - 1];
                resProfileLite.IsSOSOn = latest.IsSOS.Value;
                resProfileLite.IsTrackingOn = !latest.IsSOS.Value || !string.IsNullOrEmpty(latest.Lat);

                resProfileLite.LastLocs = listLocationData.List;
                ResultsManager.AddResultInfo(resProfileLite, ResultTypeEnum.Success, "Location details filled.");
            }

            if (!resProfileLite.IsSOSOn && !resProfileLite.IsTrackingOn)
                ResultsManager.AddResultInfo(resProfileLite, ResultTypeEnum.Success, "No Tracking or SOS Details found");

            return resProfileLite;
        }

        public async Task<LiveUserStatusList> GetLiveMembersByGroupID(string groupID)
        {
            bool includeActiveUsers = Config.IncludeActiveMembers;
            int GroupID = Convert.ToInt32(groupID);
            List<LiveUserStatus> liveGroupMembers =
                await _locRepository.GetLiveMembersByGroupID(GroupID, includeActiveUsers);

            List<Interfaces.DataContracts.OutBound.LiveUserStatus> liveGroupMembersLst =
                liveGroupMembers.ConvertToLiveMemberStatusList();

            var groupMembers = new LiveUserStatusList {List = liveGroupMembersLst};

            return groupMembers;
        }

        public async Task<LiveUserStatusList> GetLiveLocateBuddiesByUserId(string UserID)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            long userID = Convert.ToInt64(UserID);
            bool isAuthorized = await _authService.ValidUserAccess(liveUserID, userID);

            LiveUserStatusList liveBuddyMembers = null;
            if (isAuthorized)
            {
                List<LiveUserStatus> liveBuddies = await _locRepository.GetLiveLocateBuddiesByUserId(userID);

                List<Interfaces.DataContracts.OutBound.LiveUserStatus> liveBuddyMembersLst =
                    liveBuddies.ConvertToLiveMemberStatusList();

                liveBuddyMembers = new LiveUserStatusList {List = liveBuddyMembersLst};
            }
            else
            {
                liveBuddyMembers = new LiveUserStatusList();
                ResultsManager.AddResultInfo(liveBuddyMembers, ResultTypeEnum.AuthError,
                    "You are not authorized to view these buddies");
            }

            return liveBuddyMembers;
        }

        internal async Task<List<ProfileLite>> GetLocateBuddies(long UserID)
        {
            List<UserLocation> usersLocations = (await _locRepository.GetBuddiesToLocateLastLocation(UserID)).ToList();

            usersLocations.ForEach(userLoc => userLoc.MobileNumber = Utility.Security.Decrypt(userLoc.MobileNumber));

            return usersLocations.ConvertToProfileLiteListContract();
        }

        public async Task<ProfileLiteList> GetBuddiesToLocateByProfileID(string userProfileID)
        {
            long UserProfileID = Convert.ToInt64(userProfileID);

            List<UserLocation> usersLocations =
                (await _locRepository.GetBuddiesToLocateByProfileID(UserProfileID)).ToList();

            return new ProfileLiteList {List = usersLocations.ConvertToProfileLiteListContract()};
        }

        public async Task PostMyLocation(LiveLocation liveLocation)
        {
            if (IsEventHubEnabled)
                await EventsSender.SendLocationEvent(liveLocation);
            else
                LocationProcessor.ProcessLocation(liveLocation);
        }

        public async Task PostMyLocation(LiveLocation[] liveLocations)
        {
            if (IsEventHubEnabled)
                await EventsSender.SendLocationEvents(liveLocations);
            else
            {
                Parallel.ForEach(liveLocations, loc => { LocationProcessor.ProcessLocation(loc); });
            }
        }

        public async Task<int> GetUserStatus()
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            return await _locRepository.GetUserLiveStatus(liveUserID);            


        }
    }
}
using SOS.Service.Implementation;
using SOS.Service.Interfaces.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using SOS.Service.Interfaces.DataContracts.OutBound;
using System.Text.RegularExpressions;
using SOS.Web.EncryptionAlgorithm;
using SOS.Service.Interfaces.DataContracts.InBound;

namespace SOS.Web
{
    public sealed class ServiceProxy : ProxyBase, IServiceProxy
    {
        public string Ping()
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {
                return "200";
            }
            else
            {
                return "401";
            }

        }

        #region History Service


        public HistoryList GetHistorySessions(string profileID, string startDate, string endDate, string Ticks)
        {

            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {	//If Authorized, that is, if user.SOSUserID <> ''	

                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new HistoryService().GetHistorySessions(profileID, startDate, endDate);
            }
            else
            {
                return null;
            }
        }

        public GeoTagList GetHistoryLocationsBySessionID(string profileID, string sessionID, string Ticks)
        {

            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {	
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new HistoryService().GetHistoryLocationsBySessionID(profileID, sessionID);
            }
            else
            {
                return null;
            }
        }

        public bool DeleteHistoryDetails(string ProfileID, string SessionID, string GroupID, string Ticks)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new HistoryService().DeleteHistoryDetails(ProfileID, SessionID, GroupID).Result;
            }
            else
            {
                return false;
            }
        }



        #endregion

        #region Location Service

        public BasicProfile GetUserLocationsByToken(string ProfileID, string Token, string LastUpdateTime, string Ticks)
        {
            // No authentication is required, if user comes with token. If token is valid display the data else not.
            return new LocationService().GetUserLocationsByToken(ProfileID, Token, LastUpdateTime).Result;
        }


        #endregion

        #region GroupService

        public Admin GetAdminProfile(string LiveMail)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {
                return new GroupService().GetAdminProfile(user.LiveUserID);
            }
            else
            {
                return null;
            }

        }

        public ResultInfo SaveMarshalInfo(MarshallToAdd oMarshallToAdd)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), oMarshallToAdd.GroupID);

            if (user.IsValid)
            {
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new GroupService().SaveMarshalInfo(oMarshallToAdd).Result;
            }
            else
            {
                return null;
            }
        }

        public ResultInfo DeleteMarshal(string AdminID, string GroupID, string MarshalProfileID, string Token)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new GroupService().DeleteMarshal(AdminID, GroupID, MarshalProfileID).Result;
            }
            else
            {
                return null;
            }
        }

        public ResultInfo AssignBuddyToMarshal(string AdminID, string GroupID, string MarshalProfileID, string MarshalUserID, string TargetUserProfileID, string Token)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new GroupService().AssignBuddyToMarshal(AdminID, GroupID, MarshalProfileID, MarshalUserID, TargetUserProfileID).Result;
            }
            else
            {
                return null;
            }

        }

        public ResultInfo RemoveBuddyFromMarshal(string AdminID, string GroupID, string MarshalProfileID, string MarshalUserID, string TargetUserProfileID, string Token)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new GroupService().RemoveBuddyFromMarshal(AdminID, GroupID, MarshalProfileID, MarshalUserID, TargetUserProfileID).Result;
            }
            else
            {
                return null;
            }
        }

        public MarshalList GetMarshalList(string GroupID, string Token)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new GroupService().GetMarshalList(GroupID).Result;
            }
            else
            {
                return null;
            }
        }

        public ResultInfo ValidateGroupMember(string ValidationID, string ProfileID)
        {
            return new GroupService().ValidateGroupMember(ValidationID, ProfileID).Result;
        }

        public ResultInfo ValidateGroupMarshal(string ValidationID, string ProfileID)
        {
            return new GroupService().ValidateGroupMarshal(ValidationID, ProfileID).Result;
        }

        public List<Member> GetAllGroupMembers(string AuthID, string UType, string GroupID, string searchKey, string IdentificationToken, string StartDate, string EndDate)
        {
            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new MemberService().GetMembersByGroupID(GroupID, searchKey,StartDate,EndDate).Result;
            }
            else
            {
                return null;
            }
        }

        public LiveUserStatusList GetLiveGroupMembers(string GroupID, string Ticks)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new LocationService().GetLiveMembersByGroupID(GroupID).Result;
            }
            else
            {
                return null;
            }
        }

        public LiveUserStatusList GetLiveLocateBuddies(string UserID, string Ticks)
        {

            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new LocationService().GetLiveLocateBuddiesByUserId(UserID).Result;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region MemberService

        public ProfileLite GetProfileLiteByProfileID(string ProfileID, string GroupID)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new MemberService().GetProfileLiteByProfileID(ProfileID, GroupID).Result;
            }
            else
            {
                return null;
            }

        }

        public BasicProfile GetBasicProfile(string ProfileID, string SessionID, string Ticks)
        {
            return new MemberService().GetBasicProfile(ProfileID, SessionID).Result;
        }

        public string GetMiniProfileForLiveID()
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {
                return new MemberService().GetMiniProfileForLiveID(user.LiveUserID).Result;
            }
            else
            {
                return null;
            }
        }

        public User SubscribeBuddyAction(string ProfileID, string UserID, string ActionType, string SubscribtionID)
        {
            return new MemberService().SubscribeBuddyAction(ProfileID, UserID, ActionType, SubscribtionID).Result;
        }
        #endregion

        #region GeoUpdate

        public string SwitchOnSOSviaSMS(string encryptedParms, string utcTicks, string ticks, string lat, string longi)
        {
            try
            {
                if (!string.IsNullOrEmpty(encryptedParms))
                {
                    string decodeEncryptedString = EncryptAndDecrypt.DecodeString(encryptedParms);

                    if (!string.IsNullOrEmpty(decodeEncryptedString))
                    {
                        string decryptedParameters = EncryptAndDecrypt.Decrypt(decodeEncryptedString);

                        if (!string.IsNullOrEmpty(decryptedParameters))
                        {
                            string[] parameters = Regex.Split(decryptedParameters, "&");

                            string ProfileID = Regex.Split(parameters[0], "=")[1];
                            string Token = Regex.Split(parameters[1], "=")[1];
                            string SOS = Regex.Split(parameters[2], "=")[1];

                            new GeoUpdate().SwitchOnSOSviaSMS(ProfileID, Token, SOS, ticks, lat, longi, utcTicks).GetAwaiter().GetResult();
                            return ProfileID + "," + Token;
                        }

                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IncidentList GetIncidents(string IdentificationToken)
        {

            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid && UType.Equals("a"))
            {
                return new GeoUpdate().GetIncidents(IdentificationToken);
            }
            else
            {
                return null;
            }
        }

        public IncidentList GetIncidentsbyDates(string IdentificationToken, string startDate, string endDate)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid && UType.Equals("a"))
            {
                return new GeoUpdate().GetIncidentsbyDates(IdentificationToken, startDate, endDate);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ReportService

        public int UserCount(string AuthToken, string UserType)
        {
            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().UserCount().Result;
            else
                return 0;
        }

        public object GroupUsers(string AuthToken, string UserType)
        {
            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().GroupUsers().Result;
            else
                return null;
        }

        public int MissedActivationCount(string AuthToken, string UserType)
        {

            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().MissedActivationCount();
            else
                return 0;
        }

        public object HistorySOSData(string AuthToken, string UserType, string startTicks, string endTicks)
        {

            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().SOSStats(startTicks, endTicks);
            else
                return null;
        }

        public List<ActiveSOSReports> ActiveSOSData(string AuthToken, string UserType)
        {

            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().ActiveModeStats().Result;
            else
                return null;
        }

        public object HistorySOSAndTrackData(string AuthToken, string UserType, string startTicks, string endTicks)
        {

            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().SOSAndTrackStats(startTicks, endTicks);
            else
                return null;
        }

        public List<DemographyReport> UserReport(string AuthToken, string UserType)
        {

            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().UserReport().Result;
            else
                return null;
        }

        public bool StopAllPostingsRpt(string AuthToken, string UserType, string ProfileID)
        {
            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
        {
               return new ReportService().StopAllPostingsRpt(ProfileID, user.SOSUserID).GetAwaiter().GetResult();
                
            }
            else
                return false;
        }

        public List<Incident> IncidentsDataByFilterRpt(string IdentificationToken, string UserType, string startDate, string endDate)
        {

            var user = AuthorizeUser(IdentificationToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().IncidentsDataByFilterRpt(IdentificationToken, startDate, endDate);
            else
                return null;
        }


        public List<Incident> IncidentsDataByIDRpt(string IdentificationToken, string UserType, string incidentID)
        {

            var user = AuthorizeUser(IdentificationToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                return new ReportService().IncidentsDataByFilterRpt(IdentificationToken, incidentID);
            else
                return null;
        }
        #endregion


        public List<BuddyAndMarshalRelation> GetMarshalsListToUnAssign(string GroupID, string Token)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new ReportService().UnMarshalsReport(GroupID).Result;
            }
            else
            {
                return null;
            }

        }


        public ResultInfo UnAssignMarshalFromList(string TargetUserProfileID, string BuddyUserID, string Token)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {
                return new MemberService().UnAssignMarshalFromList(TargetUserProfileID, BuddyUserID).Result;
            }
            else
            {
                return null;
            }

        }

        public ProfileLite GetLocationDetails(string ProfileID, string GroupID, string LastUpdateDateTicks, string Ticks)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new LocationService().GetLocationDetails(ProfileID, GroupID, LastUpdateDateTicks).Result;
            }
            else
            {
                return null;
            }
        }

        public string GetGroupMembersCount(string GroupID, string LastUpdateDateTicks)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);

            if (user.IsValid)
            {
                return new MemberService().GetMembersCountForGroup(GroupID).Result;
            }
            else
            {
                return null;
            }
        }

        public string GetUserBuddiesCount(string UserID, string LastUpdateDateTicks)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));

            if (user.IsValid)
            {
                return new MemberService().GetLocateBuddiesCountForUser(UserID).Result;
            }
            else
            {
                return null;
            }
        }

        public LiveUserStatusList GetFilteredGroupMembers(string GroupID, string SearchKey, string LastUpdateDateTicks)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), GroupID);
            if (SearchKey == "Empty")
                SearchKey = "";

            if (user.IsValid)
            {
                return new MemberService().GetFilteredGroupMembers(GroupID, SearchKey).Result;
            }
            else
            {
                return null;
            }
        }

        public LiveUserStatusList GetFilteredLocateBuddies(string UserID, string SearchKey, string LastUpdateDateTicks)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType));
            if (SearchKey == "Empty")
                SearchKey = "";

            if (user.IsValid)
            {
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new MemberService().GetFilteredLocateBuddies(UserID, SearchKey).Result;
            }
            else
            {
                return null;
            }
        }

        public void EditGroup(string AuthToken, string UserType, SOS.Service.Interfaces.DataContracts.Group grp, string partitionKey = null, string rowkey = null, bool isCreate = false)
        {
            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a"))
                new GroupService().EditGroup(grp, partitionKey, rowkey, isCreate);
        }

        public List<GroupDTO> GetAllGroupWithAdmins(string AuthToken, string UserType)
        {
            var user = AuthorizeUser(AuthToken, Convert.ToChar(UserType));

            if (user.IsValid && UserType.Equals("a") && CheckGroupManagementAccess(user.LiveUserID))
                return new GroupService().GetAllGroupWithAdmins();
            else
                return null;
        }

        public ResultInfo SaveDispatchInfo(DispatchInfo dispatchInfo)
        {
            string AuthID = WebOperationContext.Current.IncomingRequest.Headers["AuthID"];
            string UType = WebOperationContext.Current.IncomingRequest.Headers["UserType"];

            var user = AuthorizeUser(AuthID, Convert.ToChar(UType), dispatchInfo.GroupID);

            if (user.IsValid)
            {
                WebOperationContext.Current.IncomingRequest.Headers.Add("LiveUserID", user.LiveUserID);
                return new MemberService().SaveDispatchInfo(dispatchInfo).Result;
            }
            else
            {
                return null;
            }
        }
    }
}
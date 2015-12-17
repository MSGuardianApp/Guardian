using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using AutoMapper;
using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using SOS.Service.Interfaces;
using SOS.Service.Interfaces.DataContracts;
using model = SOS.Model;
using utility = SOS.Service.Utility;

namespace SOS.Service.Implementation
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ReportService : IReportService
    {
        private readonly GeoUpdate _GeoService;
        private readonly GroupRepository _GroupRepository;
        private readonly GroupStorageAccess _GroupStorageAccess;
        private readonly LiveSessionRepository _LiveSessionRepository;
        private readonly LocationRepository _LocationRepository;
        private readonly MemberRepository _MemberRepository;
        private readonly MemberService _MemberService;
        private readonly MemberStorageAccess _MemberStorageAccess;
        private readonly ReportRepository _ReportRepository;

        //Storage Classes
        private readonly SessionHistoryStorageAccess _SessionHistoryStorageAccess;
        private GroupService _GroupService;

        public ReportService()
        {
            _LocationRepository = new LocationRepository();
            _GroupRepository = new GroupRepository();
            _MemberRepository = new MemberRepository();
            _MemberService = new MemberService();
            _GroupService = new GroupService();
            _GeoService = new GeoUpdate();
            _LiveSessionRepository = new LiveSessionRepository();
            //Storage Classes
            _GroupStorageAccess = new GroupStorageAccess();
            _MemberStorageAccess = new MemberStorageAccess();
            _ReportRepository = new ReportRepository();
            _SessionHistoryStorageAccess = new SessionHistoryStorageAccess();
        }


        public async Task<int> UserCount()
        {
            return await _MemberRepository.GetAllUserPorfilesCount(); //.ConfigureAwait(false);
        }

        public async Task<object> GroupUsers()
        {
            Dictionary<int, int> groupListUserCount = await _GroupRepository.GetAllGroupsWithUserCount();

            Dictionary<int, string> groupListwithGroupNames = _GroupStorageAccess.GetAllGroupsWithGroupNames();

            var groupUserCountwithGroupNames = from usr in groupListUserCount
                join grp in groupListwithGroupNames on usr.Key equals grp.Key
                select new
                {
                    GroupID = usr.Key,
                    GroupName = grp.Value,
                    UserCount = usr.Value
                };


            return groupUserCountwithGroupNames;
        }


        public int MissedActivationCount()
        {
            return _MemberStorageAccess.PhoneValidationCount();
        }

        public async Task<List<DemographyReport>> UserReport()
        {
            int rowCount = 1;

            Dictionary<long, int> ProfilesWithCount = await _ReportRepository.GetAllProfileIDWithCount();
            Dictionary<long, int> ProfilesWithCountFromGrpMemShip =
                await _ReportRepository.GetAllProfileIDWithCountFromGrpMemShip();
            Dictionary<long, long> ProfileIDsWithUserIDs = await _ReportRepository.GetAllProfileIDWithUserID();
            Dictionary<long, string> UserIDsWithAttributes = await _ReportRepository.GetAllUserIDWithAttributes();

            IEnumerable<DemographyReport> demographyReports = from p in UserIDsWithAttributes
                join q in ProfileIDsWithUserIDs on p.Key equals q.Value
                join r in ProfilesWithCount on q.Key equals r.Key
                join s in ProfilesWithCountFromGrpMemShip on q.Key equals s.Key
                select new DemographyReport
                {
                    SNo = rowCount++,
                    UserName = p.Value.Split(',')[0],
                    BuddiesCount = r.Value,
                    GroupCount = s.Value,
                    FacebookLinked = p.Value.Split(',')[3] != null ? "Yes" : "No"
                };


            return demographyReports.ToList();
        }

        public List<SOSTrackingReport> SOSStats(string startTicks, string endTicks)
        {
            var SOSTrackingReports = new List<SOSTrackingReport>();

            List<object> SessionHistoryInfo =
                _SessionHistoryStorageAccess.GetAllSessionHistory(Convert.ToInt64(startTicks), Convert.ToInt64(endTicks),
                    true);

            foreach (object refobj in SessionHistoryInfo)
            {
                var sosTrackingReport = new SOSTrackingReport();
                Type type = refobj.GetType();
                PropertyInfo[] fields = type.GetProperties();
                foreach (PropertyInfo field in fields)
                {
                    if (field.Name.Equals("SNo"))
                        sosTrackingReport.SNo = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("MobileNumber"))
                        sosTrackingReport.MobileNumber = field.GetValue(refobj, null) != null
                            ? utility.Security.Decrypt(field.GetValue(refobj, null).ToString())
                            : null;
                    if (field.Name.Equals("UserName"))
                        sosTrackingReport.UserName = field.GetValue(refobj, null) != null
                            ? field.GetValue(refobj, null).ToString()
                            : null;
                    if (field.Name.Equals("EmailAlerts"))
                        sosTrackingReport.EmailAlerts = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("SOSAlerts"))
                        sosTrackingReport.SOSAlerts = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("TotalTimeinSOS"))
                        sosTrackingReport.TotalTimeinSOS = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("EmailBuddies"))
                        sosTrackingReport.EmailBuddies = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("SOSBuddies"))
                        sosTrackingReport.SOSBuddies = field.GetValue(refobj, null).ToString();
                }
                SOSTrackingReports.Add(sosTrackingReport);
            }

            return SOSTrackingReports;
        }

        //private static T Cast<T>(object obj, T type)
        //{
        //    return (T)obj;
        //}

        public List<SOSAndTrackonReport> SOSAndTrackStats(string startTicks, string endTicks)
        {
            var SOSAndTrackonReports = new List<SOSAndTrackonReport>();

            List<object> SessionHistoryInfo =
                _SessionHistoryStorageAccess.GetAllSessionSOSAndTrackHistory(Convert.ToInt64(startTicks),
                    Convert.ToInt64(endTicks), false);

            foreach (object refobj in SessionHistoryInfo)
            {
                var SOSAndTrackonReport = new SOSAndTrackonReport();
                Type type = refobj.GetType();
                PropertyInfo[] fields = type.GetProperties();
                foreach (PropertyInfo field in fields)
                {
                    if (field.Name.Equals("SNo"))
                        SOSAndTrackonReport.SNo = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("MobileNumber"))
                        SOSAndTrackonReport.MobileNumber = field.GetValue(refobj, null) != null
                            ? utility.Security.Decrypt(field.GetValue(refobj, null).ToString())
                            : null;
                    if (field.Name.Equals("UserName"))
                        SOSAndTrackonReport.UserName = field.GetValue(refobj, null) != null
                            ? field.GetValue(refobj, null).ToString()
                            : null;
                    if (field.Name.Equals("TotalTracks"))
                        SOSAndTrackonReport.TotalTracks = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("TotalSOSs"))
                        SOSAndTrackonReport.TotalSOSs = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("TotalSMSSent"))
                        SOSAndTrackonReport.TotalSMSSent = field.GetValue(refobj, null).ToString();
                    if (field.Name.Equals("TotalEmailSent"))
                        SOSAndTrackonReport.TotalEmailSent = field.GetValue(refobj, null).ToString();
                }
                SOSAndTrackonReports.Add(SOSAndTrackonReport);
            }

            return SOSAndTrackonReports;
        }

        public async Task<List<ActiveSOSReports>> ActiveModeStats()
        {
            int rowCount = 1;

            //1: User Calls           
            Dictionary<long, Tuple<short, DateTime>> liveSessionData =
                await _LiveSessionRepository.GetSOSLiveSessionData();
            Dictionary<long, Tuple<long, string>> UserAttributewithProfile =
                await _ReportRepository.GetAllUserwithProfileData(string.Join(",", liveSessionData.Keys.ToList()));


            //2: Group Calls
            // var groupListwithProfileIDs1 = await  _GroupRepository.GetAllGroupsWithProfileID1();            
            Dictionary<string, long> groupListwithProfileIDs = await _GroupRepository.GetAllGroupsWithProfileID();
            Dictionary<int, string> groupListwithGroupNames = _GroupStorageAccess.GetAllGroupsWithGroupNames();


            //Method1
            IEnumerable<ActiveSOSReports> ActiveSOSReports = from p in liveSessionData
                join q in UserAttributewithProfile on p.Key equals q.Value.Item1
                join s in groupListwithProfileIDs on q.Value.Item1 equals s.Value
                join t in groupListwithGroupNames on s.Key.Split(':')[0] equals t.Key.ToString()
                orderby t.Value
                select new ActiveSOSReports
                {
                    SNo = rowCount++,
                    UserName = q.Value.Item2.Split(',')[0],
                    MobileNumber = utility.Security.Decrypt(q.Value.Item2.Split(',')[2]),
                    SOSAlertCount = p.Value.Item1.ToString(),
                    StartTime = p.Value.Item2.ToString(),
                    GroupName = t.Value,
                    ProfileId = p.Key.ToString()
                };


            //Method2

            return ActiveSOSReports.ToList();
        }

        public async Task<List<BuddyAndMarshalRelation>> UnMarshalsReport(string groupID)
        {
            int GroupID = Convert.ToInt32(groupID);
            var _ListOfRemoveBuddyAndMarshalRelation = new List<BuddyAndMarshalRelation>();

            List<model.Buddy> bdyList = await _MemberRepository.GetMarshalBuddiesByGroupID(GroupID);
            if (bdyList != null && bdyList.Count > 0)
            {
                bdyList.ForEach(buddy =>
                {
                    var _RemoveBuddyAndMarshalRelation = new BuddyAndMarshalRelation();
                    _RemoveBuddyAndMarshalRelation.GroupID = GroupID.ToString();
                    _RemoveBuddyAndMarshalRelation.MarshalUserID = buddy.UserID.ToString();
                    _RemoveBuddyAndMarshalRelation.TargetUserProfileID = buddy.ProfileID.ToString();
                    _RemoveBuddyAndMarshalRelation.MarshalName = buddy.BuddyName;
                    _RemoveBuddyAndMarshalRelation.UserName = buddy.User.Name;

                    _ListOfRemoveBuddyAndMarshalRelation.Add(_RemoveBuddyAndMarshalRelation);
                });
            }

            return _ListOfRemoveBuddyAndMarshalRelation;
        }


        public List<Incident> IncidentsDataByFilterRpt(string IdentificationToken, string startDate, string endDate)
        {
            return _GeoService.GetIncidentsbyDates(IdentificationToken, startDate, endDate).List;
        }

        public List<Incident> IncidentsDataByFilterRpt(string IdentificationToken, string incidentID)
        {
            return _GeoService.GetIncidentsbyID(IdentificationToken, incidentID).List;
        }

        public async Task<string> GetUserName(string profileId)
        {
            return await _ReportRepository.GetUserNameForProfileID(profileId);
        }

        public async Task<User> GetUserByProfileID(string profileId)
        {
            Mapper.CreateMap<model.User, User>();
            return Mapper.Map<User>(await _MemberRepository.GetUserByProfileID(profileId));
        }

        public async Task<bool> StopAllPostingsRpt(string ProfileID, string AdminGroupID)
        {
            GroupMembers AllProfilesOfGroup = await _MemberService.GetAllProfilesAssociatedToGroup(AdminGroupID);

            if (AllProfilesOfGroup.Profiles.Contains(ProfileID))
            {
                await _LocationRepository.StopSOSOnly(Convert.ToInt64(ProfileID), "0");
                return true;
            }

            return false;
        }

        public async Task<SOSTrackCounts> SOSTrackCount(int GroupID)
        {
            return Mappers.Mapper.ConvertToSOSTrackCounts((await _LocationRepository.GetSOSTrackCountByGroupId(GroupID)));
        }


        //public void UpdateNameInGroupMembership()
        //{
        //    try
        //    {
        //        _GroupStorageAccess.GetAllRowsFromGroupMembership().ForEach((grpMembership) =>
        //        {
        //            var name = getUserName(grpMembership.ProfileID);
        //            if (name == "N/A")
        //                _GroupStorageAccess.CUDGroupMembership(grpMembership, IsRemove: true);
        //            else
        //            {
        //                grpMembership.UserName = name;
        //                _GroupStorageAccess.CUDGroupMembership(grpMembership, IsRemove: false);
        //            }

        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //As discussed, verifying the usage of this method
        public void encryptPhoneNumber()
        {
            try
            {
                _MemberStorageAccess.GetAllPhoneValidationData().ForEach(phValid =>
                {
                    phValid.IsValiated = true;
                    _MemberStorageAccess.DeletePhoneValidationEntry(phValid);

                    phValid.PhoneNumber = utility.Security.Encrypt(phValid.PhoneNumber);
                    phValid.RowKey = phValid.SecurityToken.ToString() + "_" +
                                     DateTime.Parse(phValid.Timestamp.ToString()).Ticks;
                    phValid.IsValiated = false;
                    _MemberStorageAccess.SavePhoneValidationRecord(phValid);
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
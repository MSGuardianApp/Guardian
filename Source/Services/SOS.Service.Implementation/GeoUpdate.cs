using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using SOS.AzureSQLAccessLayer;
using SOS.AzureStorageAccessLayer;
using SOS.Mappers;
using SOS.Model;
using SOS.Service.Interfaces;
using SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using SOS.Service.Utility;
using entity = SOS.AzureStorageAccessLayer.Entities;

namespace SOS.Service.Implementation
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GeoUpdate : IGeoUpdates
    {
        private readonly LocationHistoryStorageAccess GpsaAccess = new LocationHistoryStorageAccess();
        private readonly Authorization _authService;
        private readonly IncidentStorageAccess _incidentAccess = new IncidentStorageAccess();
        private readonly LocationRepository _locRepository = new LocationRepository();
        private readonly MemberRepository _memberRepository;
        private MemberStorageAccess _memberStorage;

        public GeoUpdate()
        {
            Mapper.InitializeMappers();
            _memberRepository = new MemberRepository();
            _memberStorage = new MemberStorageAccess();
            _authService = new Authorization();
        }


        public async Task SwitchOnSOSviaSMS(string ProfileID, string sessionID, string SOS, string ticks, string lat,
            string lng, string utcTicks)
        {
            long profileID = Convert.ToInt64(ProfileID);
            lat = lat == "-1" ? string.Empty : lat;
            lng = lng == "-1" ? string.Empty : lng;
            long outticks;
            long outUtcTicks;
            bool isSOS = (SOS == "1") ? true : false;

            try
            {
                if (long.TryParse(ticks, out outticks) && long.TryParse(utcTicks, out outUtcTicks))
                {
                    //to check whether current time outUtcTicks is lessthan 4 hours to current utc time
                    var localUtcTime = new DateTime(outUtcTicks);
                    DateTime serverUtcTime = DateTime.UtcNow;
                    TimeSpan ticksDifference = serverUtcTime - localUtcTime;
                    int differenceHours = Convert.ToInt32(ticksDifference.TotalHours);

                    if (!(await _locRepository.IsSosAlreadyOn(profileID, sessionID, outticks)) &&
                        (await _memberRepository.IsProfileValidAsync(profileID)) &&
                        !(GpsaAccess.IsEntryThereInHistoryTable(profileID.ToString(), sessionID, outticks)) &&
                        (outUtcTicks == 0 || differenceHours < 4))
                    {
                        var geotag = new GeoTags
                        {
                            PID = profileID,
                            Id = sessionID,
                            Alt = new string[] {null},
                            Lat = new[] {lat},
                            Long = new[] {lng},
                            TS = new[] {outticks},
                            IsSOS = new[] {isSOS},
                            Accuracy = new double[] { 0 }
                            //Spd = new int[]{0}
                        };
                        await PostTheLocation(geotag);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error in {0} : {1}", "SwitchOnSOSviaSMS", ex.Message));
                throw ex;
            }
        }

        public async Task<bool> PostMyLocation(GeoTags vGeoTags)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            bool isAuthorizedForSelf = await _authService.SelfAccess(liveUserID, vGeoTags.PID);

            if (isAuthorizedForSelf)
            {
                await PostTheLocation(vGeoTags);
                return true;
            }
            return false;
        }

        public async Task<bool> PostLocationWithMedia(GeoTag vGeoTag)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            bool isAuthorizedForSelf = await _authService.SelfAccess(liveUserID, vGeoTag.ProfileID);

            if (isAuthorizedForSelf)
            {
                LiveLocation LiveLocation = vGeoTag.ConvertToLiveLocation();

                try
                {
                    if (vGeoTag.MediaContent != null && vGeoTag.MediaContent.Length > 0)
                    {
                        byte[] MediaContents = vGeoTag.MediaContent;
                        LiveLocation.MediaUri = new BlobAccess().UploadImage(MediaContents, Guid.NewGuid().ToString());
                    }
                    else
                    {
                        LiveLocation.MediaUri = null;
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(
                        string.Format("Error while saving the media contents for profile id: {0}; Error details: {1}",
                            vGeoTag.ProfileID, ex.Message));
                }

                //new line added here
                await new LocationService().PostMyLocation(LiveLocation);

                return true;
            }
            return false;
        }

        public async Task StopPostings(string profileID, string sessionID, string ticks)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            long ProfileID = Convert.ToInt64(profileID);
            long clientTicks = Convert.ToInt64(ticks);

            bool isAuthorizedForSelf = await _authService.SelfAccess(liveUserID, ProfileID);
            if (isAuthorizedForSelf)
            {
                if (sessionID != "0")
                    await _locRepository.RemoveLiveLocationData(ProfileID, clientTicks, sessionID);
                else
                    await _locRepository.RemoveLiveLocationData(ProfileID, clientTicks);
            }
        }

        public async Task StopSOSOnly(string profileID, string sessionID, string ticks)
        {
            long ProfileID = Convert.ToInt64(profileID);
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            bool isAuthorizedForSelf = await _authService.SelfAccess(liveUserID, ProfileID);
            if (isAuthorizedForSelf)
            {
                var tasks = new List<Task>();

                Task updateDatabaseTask = _locRepository.StopSOSOnly(ProfileID, sessionID);
                tasks.Add(updateDatabaseTask);

                LiveSession profileNotifyDetails =
                    await new LiveSessionRepository().GetNotificationDetails(ProfileID, sessionID);

                if (profileNotifyDetails != null)
                {
                    Task sendNotificationsTask = ServiceUtility.SendSafeNotificationAsync(profileNotifyDetails);
                    tasks.Add(sendNotificationsTask);
                }
                Task.WhenAll(tasks.ToArray()).Wait();
            }
        }

        public async Task UpdateLastSMSPostedTime(string ProfileID, string SessionID, string SMSPostedTime)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];
            long profileID = Convert.ToInt64(ProfileID);
            DateTime smsPostTime = new DateTime(Convert.ToInt64(SMSPostedTime));
            bool isAuthorizedForSelf = await _authService.SelfAccess(liveUserID, profileID);
            if (isAuthorizedForSelf)
                await new LiveSessionRepository().UpdateLastSMSPostedTime(profileID, SessionID, smsPostTime);
        }

        #region Incident

        public string ReportIncident(IncidentTag incidentTag)
        {
            string liveUserID = WebOperationContext.Current.IncomingRequest.Headers["LiveUserID"];

            bool isAuthorizedForSelf = _authService.SelfAccess(liveUserID, incidentTag.ProfileID).Result;
            if (isAuthorizedForSelf)
            {
                entity.Incident teaser = Caster.MakeEntityTeaser(incidentTag);
                try
                {
                    if (incidentTag.MediaContent != null && incidentTag.MediaContent.Length > 0)
                    {
                        byte[] MediaContents = incidentTag.MediaContent;
                        teaser.MediaUri = new BlobAccess().UploadImage(MediaContents, Guid.NewGuid().ToString());
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(
                        string.Format("Error while saving the media contents for profile id: {0}; Error details: {1}",
                            incidentTag.ProfileID, ex.Message));
                }

                _incidentAccess.RecordIncident(teaser);

                return teaser.IncidentID;
            }
            return null;
        }

        public IncidentList GetIncidents(string IdentificationToken)
        {
            // Authenticate the Request from Token

            var incidents = new IncidentList {List = new List<Incident>()};

            var Incidents = new List<entity.Incident>();
            try
            {
                Incidents = _incidentAccess.GetAllIncidents();

                Incidents.ForEach(tease => { incidents.List.Add(Caster.MakeContractIncident(tease)); });

                ResultsManager.AddResultInfo(incidents, ResultTypeEnum.Success, "Successed");
            }
            catch
            {
                ResultsManager.AddResultInfo(incidents, ResultTypeEnum.Exception, "Failed");
            }
            return incidents;
        }

        public IncidentList GetIncidentsbyDates(string IdentificationToken, string startDate, string endDate)
        {
            // Authenticate the Request from Token

            var incidents = new IncidentList {List = new List<Incident>()};

            DateTime vStartTime;
            DateTime vEndTime;
            string mobileNumber = string.Empty;

            vStartTime = Converter.ToDateTime(startDate).Date;
            vEndTime = Converter.ToMaxDateTime(endDate).Date.AddMinutes(1440).AddSeconds(-1);

            var Incidents = new List<entity.Incident>();
            try
            {
                Incidents = _incidentAccess.GetAllIncidentsDataByFilter(vStartTime.Ticks.ToString(),
                    vEndTime.Ticks.ToString());
                if (Incidents != null)
                {
                    Incidents.ForEach(tease => { incidents.List.Add(Caster.MakeContractIncident(tease)); });

                    ResultsManager.AddResultInfo(incidents, ResultTypeEnum.Success, "Successed");
                }
            }
            catch
            {
                ResultsManager.AddResultInfo(incidents, ResultTypeEnum.Exception, "Failed");
            }
            return incidents;
        }

        public IncidentList GetIncidentsbyID(string IdentificationToken, string incidentID)
        {
            // Authenticate the Request from Token

            var incidents = new IncidentList {List = new List<Incident>()};

            string mobileNumber = string.Empty;

            var Incidents = new List<entity.Incident>();
            try
            {
                Incidents = _incidentAccess.GetAllIncidentsDataByID(incidentID);
                if (Incidents != null)
                {
                    Incidents.ForEach(tease => { incidents.List.Add(Caster.MakeContractIncident(tease)); });

                    ResultsManager.AddResultInfo(incidents, ResultTypeEnum.Success, "Successed");
                }
            }
            catch
            {
                ResultsManager.AddResultInfo(incidents, ResultTypeEnum.Exception, "Failed");
            }
            return incidents;
        }

        #endregion Incident

        private async Task PostTheLocation(GeoTags vGeoTags)
        {
            GeoTagList GTL = Caster.ConvertToGeoTagList(vGeoTags);
            List<LiveLocation> locations = GTL.List.ConvertToLiveLocationList();
            await new LocationService().PostMyLocation(locations.ToArray());
        }

        ///// <summary>
        ///// Unused
        ///// </summary>
        ///// <param name="profileID"></param>
        ///// <param name="clientTicks"></param>
        ///// <returns></returns>
        //public async Task StopAllPostings(string profileID, string clientTicks)
        //{
        //    long ProfileID = Convert.ToInt64(profileID);
        //    long ClientTicks = Convert.ToInt64(clientTicks);

        //    await _locRepository.RemoveLiveLocationData(ProfileID, ClientTicks);
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using entity = SOS.AzureStorageAccessLayer.Entities;
using contract = SOS.Service.Interfaces.DataContracts;
using inbound = SOS.Service.Interfaces.DataContracts.InBound;
using utility = SOS.Service.Utility;
using model = SOS.Model;

namespace SOS.Service.Implementation
{
    internal class Caster
    {
        internal static void FillEntityUser(model.User ReUser, contract.User userInst)
        {
            ReUser.Name = userInst.Name;
            ReUser.Email = userInst.Email;
            ReUser.MobileNumber = utility.Security.Encrypt(userInst.MobileNumber);

            ReUser.FBAuthID = userInst.FBAuthID;
            ReUser.FBID = userInst.FBID;

            if (userInst.LiveDetails != null)
            {
                ReUser.LiveID = userInst.LiveDetails.LiveID;
            }
        }

        internal static void FillProfileEntity(model.Profile pe, contract.Profile profile, bool RestrictPhone)
        {
            if (profile == null || pe == null)
                return;
            pe.UserID = profile.UserID;
            pe.ProfileID = profile.ProfileID;

            if (profile.PhoneSetting == null)
                profile.PhoneSetting = new contract.DeviceSetting();


            if (!RestrictPhone)
            {
                pe.MobileNumber = utility.Security.Encrypt(profile.MobileNumber);
                pe.SecurityToken = profile.SecurityToken;
            }

            pe.CanEmail = profile.CanMail;
            pe.CanPost = profile.CanPost;

            pe.IsValid = profile.IsValid;

            pe.RegionCode = profile.RegionCode;

            pe.CanSMS = profile.CanSMS;

            pe.LocationConsent = profile.LocationConsent;

            if (profile.PhoneSetting != null)
            {
                pe.DeviceID = profile.PhoneSetting.DeviceID;
                pe.DeviceType = GetDeviceType(profile.PhoneSetting.PlatForm);
            }
            //Filled in Fill FBDetails
            pe.FBGroup = profile.FBGroupName;
            pe.FBGroupID = profile.FBGroupID;

            pe.EnterpriseSecurityToken = profile.EnterpriseSecurityToken;
            pe.EnterpriseEmailID = profile.EnterpriseEmailID;
            pe.NotificationUri = profile.NotificationUri;
        }

        private static model.DeviceType GetDeviceType(string platform)
        {
            if (platform == "2") return model.DeviceType.Andoid;
            if (platform == "3") return model.DeviceType.iOS;

            return model.DeviceType.WindowsPhone;
        }

        internal static model.Buddy MakeEntityBuddy(contract.Buddy buddy, long ProfileID, bool updateState = true)
        {
            if (buddy == null)
                return null;

            var buddyEntity = new model.Buddy
            {
                BuddyID = buddy.BuddyID,
                ProfileID = ProfileID,
                UserID = buddy.UserID,
                BuddyName = buddy.Name,
                MobileNumber = utility.Security.Encrypt(buddy.MobileNumber),
                Email = buddy.Email,
                IsPrimeBuddy = buddy.IsPrimeBuddy,
                SubscribtionId = buddy.SubscribtionID
            };

            if (updateState)
            {
                buddyEntity.State = (model.BuddyState)buddy.State;
            }
            return buddyEntity;
        }


        internal static List<contract.Buddy> MakeContractBuddyFromEntity(List<model.Buddy> buddyusers)
        {
            if (buddyusers == null)
                return null;

            var mybuddiesreturn = new List<contract.Buddy>();

            foreach (model.Buddy bud in buddyusers)
            {
                if (bud != null)
                {
                    mybuddiesreturn.Add(new contract.Buddy
                    {
                        BuddyID = bud.BuddyID,
                        UserID = bud.UserID,
                        Name = bud.BuddyName,
                        Email = bud.Email,
                        MobileNumber = utility.Security.Decrypt(bud.MobileNumber),
                        IsPrimeBuddy = bud.IsPrimeBuddy,
                        State = ((bud.State == null) ? contract.BuddyState.Suspended : (contract.BuddyState)bud.State),
                        SubscribtionID = bud.SubscribtionId
                    });
                }
            }

            return mybuddiesreturn;
        }

        internal static contract.Profile MakeContractProfile(model.Profile profileEntity, model.User userEntity,
            List<contract.Group> groupsAsociated, List<model.Buddy> buddies, List<contract.ProfileLite> LocateBuds)
        {
            if (profileEntity == null)
                return null;

            var retProfile = new contract.Profile();

            retProfile.ProfileID = profileEntity.ProfileID;
            retProfile.UserID = profileEntity.UserID;

            retProfile.Name = userEntity.Name;
            retProfile.Email = userEntity.Email;
            retProfile.MobileNumber = (string.IsNullOrEmpty(utility.Security.Decrypt(profileEntity.MobileNumber)))
                ? utility.Security.Decrypt(userEntity.MobileNumber)
                : utility.Security.Decrypt(profileEntity.MobileNumber);
            retProfile.CanPost = profileEntity.CanPost;
            retProfile.CanMail = profileEntity.CanEmail;
            retProfile.CanSMS = profileEntity.CanSMS;
            retProfile.IsValid = profileEntity.IsValid;
            retProfile.LocationConsent = profileEntity.LocationConsent;
            retProfile.RegionCode = profileEntity.RegionCode;
            retProfile.LiveDetails = new contract.LiveCred
            {
                LiveID = userEntity.Email
            };

            retProfile.FBAuthID = userEntity.FBAuthID;
            retProfile.FBID = userEntity.FBID;
            retProfile.FBGroupName = profileEntity.FBGroup;
            retProfile.FBGroupID = profileEntity.FBGroupID;

            List<contract.Buddy> buddys = null;
            if (buddies != null && buddies.Count > 0)
            {
                buddys = new List<contract.Buddy>();
                foreach (model.Buddy bud in buddies)
                {
                    buddys.Add(new contract.Buddy
                    {
                        BuddyID = bud.BuddyID,
                        //RelProfileID = bud.ProfileID,
                        Name = bud.BuddyName,
                        UserID = bud.UserID,
                        Email = bud.Email,
                        MobileNumber = utility.Security.Decrypt(bud.MobileNumber),
                        IsPrimeBuddy = bud.IsPrimeBuddy,
                        State = (contract.BuddyState)(bud.State),
                        SubscribtionID = bud.SubscribtionId
                    });
                }
            }
            else
            {
                buddys = null;
            }
            retProfile.AscGroups = groupsAsociated;
            retProfile.MyBuddies = buddys;
            retProfile.LocateBuddies = LocateBuds;

            return retProfile;
        }


        internal static contract.Group MakeContractGroupLite(entity.Group groupEntity)
        {
            if (groupEntity == null)
                return null;

            var grp = new contract.Group();
            grp.GroupID = groupEntity.GroupID.ToString();
            grp.GroupName = groupEntity.GroupName;
            grp.GroupLocation = groupEntity.Location;
            grp.EnrollmentType = (contract.Enrollment)groupEntity.EnrollmentType;
            grp.EnrollmentKey = groupEntity.EnrollmentKey;
            grp.Type = (contract.GroupType)groupEntity.GroupType;
            grp.IsActive = groupEntity.IsActive;
            grp.Email = groupEntity.Email;
            grp.PhoneNumber = groupEntity.PhoneNumber; //TODO:Load Contact details from GroupContacts.
            grp.GeoLocation = groupEntity.GeoLocation;
            grp.ShowIncidents = groupEntity.ShowIncidents;
            return grp;
        }

        internal static contract.ProfileLite MakeProfileLiteOnCombination(model.Profile profileEntity,
            model.User userEntity)
        {
            if (profileEntity == null || userEntity == null)
                return null;

            var retProfile = new contract.ProfileLite
            {
                ProfileID = profileEntity.ProfileID,
                Email = userEntity.Email,
                MobileNumber = utility.Security.Decrypt(profileEntity.MobileNumber ?? profileEntity.User.MobileNumber),
                Name = userEntity.Name,
                UserID = profileEntity.UserID,
            };

            return retProfile;
        }

        internal static entity.Incident MakeEntityTeaser(contract.IncidentTag geoTag)
        {
            if (geoTag.ProfileID == 0 || string.IsNullOrEmpty(geoTag.Lat) || string.IsNullOrEmpty(geoTag.Long))
                return null;
            double dOut = 0;
            string incidentGuid = utility.TokenManager.GenerateNewTeaserID();
            return new entity.Incident
            {
                ProfileID = geoTag.ProfileID.ToString(),
                ID = incidentGuid.Substring(incidentGuid.LastIndexOf("-") + 1),
                IncidentID = incidentGuid,
                Latitude = (double.TryParse(geoTag.Lat, out dOut) ? dOut : 0),
                Longitude = (double.TryParse(geoTag.Long, out dOut) ? dOut : 0),
                Altitude = (double.TryParse(geoTag.Alt, out dOut) ? dOut : 0),
                DateTime = geoTag.TimeStamp,
                Type = geoTag.Command,
                Name = geoTag.Name,
                MobileNumber = utility.Security.Encrypt(geoTag.MobileNumber),
                AdditionalInfo =
                    geoTag.AdditionalInfo != null
                        ? !string.IsNullOrEmpty(geoTag.AdditionalInfo) ? geoTag.AdditionalInfo : ""
                        : ""
            };
        }

        internal static contract.Admin MakeContractAdmin(entity.AdminUser admin)
        {
            if (admin == null) return null;

            var oAdmin = new contract.Admin
            {
                AdminID = admin.AdminID,
                Name = admin.Name,
                MobileNumber = admin.MobileNumber,
                Email = admin.Email,
                GroupIDCSV = admin.GroupIDCSV,
                AllowGroupManagement = admin.AllowGroupManagement
            };

            return oAdmin;
        }

        internal static contract.HistoryList MakeContractHistoryList(List<entity.LocationHistory> HistoryEntityData,
            string MobileNumber)
        {
            if (HistoryEntityData == null || HistoryEntityData.Count < 0)
                return null;

            var HistoryLocationData = new contract.HistoryList();

            List<IGrouping<string, entity.LocationHistory>> filteredList =
                HistoryEntityData.GroupBy(x => x.SessionID).ToList();

            int cnt = 1;

            foreach (var item in filteredList)
            {
                var history = new contract.History();
                history.PeriodStartDate = item.Min(s => s.ClientTimeStamp).ToString() == "0"
                    ? utility.Converter.GetSOSMinDateTime().Ticks.ToString()
                    : item.Min(s => s.ClientTimeStamp).ToString();
                history.PeriodEndDate = item.Max(s => s.ClientTimeStamp).ToString() == "0"
                    ? utility.Converter.GetSOSMaxDateTime().Ticks.ToString()
                    : item.Max(s => s.ClientTimeStamp).ToString();
                history.PhoneNumber = utility.Security.Decrypt(MobileNumber);
                history.GeoInstances =
                    item.Select(
                        s =>
                            new contract.GeoTag
                            {
                                SessionID = s.SessionID,
                                IsSOS = s.IsSOS,
                                Lat = s.Lat,
                                Long = s.Long,
                                Alt = s.Alt,
                                GeoDirection = 0,
                                Speed = s.Speed,
                                TimeStamp = s.Timestamp.DateTime.Ticks,
                                ProfileID = Convert.ToInt64(s.ProfileID),
                                MediaUri = s.MediaUri,
                                Accuracy = s.Accuracy
                            }).ToList();

                HistoryLocationData.List.Add(history);
            }

            return HistoryLocationData;
        }

        internal static contract.HistoryList MakeContractSessionHistoryList(
            List<entity.SessionHistory> SessionHistoryEntityData)
        {
            if (SessionHistoryEntityData == null || SessionHistoryEntityData.Count < 0)
                return null;

            var SessionHistoryData = new contract.HistoryList();

            foreach (entity.SessionHistory item in SessionHistoryEntityData)
            {
                var history = new contract.History();
                history.PeriodStartDate = item.SessionStartTime.Ticks.ToString();
                history.PeriodEndDate = (item.SessionEndTime != null)
                    ? item.SessionEndTime.Value.Ticks.ToString()
                    : "Unknown";

                history.SessionID = item.SessionID;
                history.IsEvidenceAvailable = item.IsEvidenceAvailable;

                SessionHistoryData.List.Add(history);
            }

            return SessionHistoryData;
        }

        internal static contract.GeoTagList MakeGeoTagList(List<entity.LocationHistory> historyEntityData)
        {
            if (historyEntityData == null || historyEntityData.Count < 0)
                return null;

            var history = new List<contract.GeoTag>();
            foreach (entity.LocationHistory s in historyEntityData)
            {
                history.Add(new contract.GeoTag
                {
                    SessionID = s.SessionID,
                    IsSOS = s.IsSOS,
                    Lat = s.Lat,
                    Long = s.Long,
                    Alt = s.Alt,
                    GeoDirection = 0,
                    Speed = s.Speed,
                    TimeStamp = s.Timestamp.DateTime.Ticks,
                    ProfileID = Convert.ToInt64(s.ProfileID),
                    MediaUri = s.MediaUri,
                    Accuracy = s.Accuracy
                });
            }
            return new contract.GeoTagList { List = history };
        }

        internal static List<contract.GeoTag> MakeContractGeoTagList(List<entity.GeoTag> geoLocation)
        {
            if (geoLocation == null || geoLocation.Count < 0)
                return null;

            List<contract.GeoTag> geoTagList =
                geoLocation.Select(
                    s =>
                        new contract.GeoTag
                        {
                            ProfileID = s.ProfileID,
                            SessionID = s.SessionID,
                            IsSOS = s.IsSOS,
                            Lat = s.Lat,
                            Long = s.Long,
                            Alt = s.Alt,
                            Speed = s.Speed,
                            TimeStamp = s.ClientTimeStamp,
                            MediaUri = s.MediaUri,
                            Accuracy = s.Accuracy
                        }).ToList();

            return geoTagList;
        }

        internal static List<contract.GeoTag> MakeContractGeoTagList(List<entity.LocationHistory> geoLocation)
        {
            if (geoLocation == null || geoLocation.Count <= 0)
                return null;

            List<contract.GeoTag> geoTagList =
                geoLocation.Select(
                    s =>
                        new contract.GeoTag
                        {
                            ProfileID = Convert.ToInt64(s.ProfileID),
                            SessionID = s.SessionID,
                            IsSOS = s.IsSOS,
                            Lat = s.Lat,
                            Long = s.Long,
                            Alt = s.Alt,
                            Speed = s.Speed,
                            TimeStamp = s.ClientTimeStamp,
                            MediaUri = s.MediaUri,
                            Accuracy = s.Accuracy
                        }).ToList();

            return geoTagList;
        }

        internal static contract.Marshal MakeContractMarshal(contract.ProfileLite profile,
            List<contract.ProfileLite> locateBuddies, bool isValidated)
        {
            return new contract.Marshal
            {
                ProfileID = profile.ProfileID,
                Email = profile.Email,
                MobileNumber = profile.MobileNumber,
                Name = profile.Name,
                SessionID = profile.SessionID,
                TinyURI = profile.TinyURI,
                UserID = profile.UserID,
                IsSOSOn = profile.IsSOSOn,
                IsTrackingOn = profile.IsTrackingOn,
                LastLocs = profile.LastLocs,
                IsValidated = isValidated,
                LocateBuddies = locateBuddies,
            };
        }

        internal static model.GroupMembership MakeGroupMemberEntity(long ProfileID, string UserName, contract.Group Grp, bool IsValidated)
        {
            int GroupID = 0;
            if (int.TryParse(Grp.GroupID, out GroupID))
                return new model.GroupMembership
                {
                    GroupID = GroupID,
                    ProfileID = ProfileID,
                    UserName = UserName,
                    EnrollmentKeyValue = Grp.EnrollmentValue,
                    IsValidated = IsValidated
                };
            return null;
        }

        internal static entity.GroupMemberValidator MakeGroupMemberValidator(long ProfileID, contract.Group Grp, string ValidationGUID)
        {
            int GroupID = 0;
            if (int.TryParse(Grp.GroupID, out GroupID))
                return new entity.GroupMemberValidator
                {
                    GroupID = GroupID,
                    ProfileID = ProfileID.ToString(),
                    PartitionKey = GroupID.ToString(),
                    RowKey = ValidationGUID,
                    ValidationID = ValidationGUID,
                    IsValidated = false,
                    NotificationSent = false,
                    NotificationIdentity = Grp.EnrollmentValue,
                };
            return null;
        }

        internal static entity.GroupMarshalValidator MakeGroupMarshalValidator(long ProfileID, int GroupID,
            string GroupIdentity, string ValidationID)
        {
            return new entity.GroupMarshalValidator
            {
                GroupID = GroupID,
                ProfileID = ProfileID.ToString(),
                PartitionKey = GroupID.ToString(),
                RowKey = ValidationID,
                ValidationID = ValidationID,
                IsValidated = false,
                NotificationSent = false,
                NotificationIdentity = GroupIdentity
            };
        }

        internal static void DiscardAuthDetails(model.User userEntity)
        {
            //throw new NotImplementedException();
            userEntity.FBAuthID = string.Empty;
            userEntity.FBID = string.Empty;
            userEntity.LiveID = string.Empty;
        }

        internal static contract.GeoTagList ConvertToGeoTagList(contract.GeoTags GT)
        {
            if (GT == null) return null;
            long profileID = GT.PID;
            string ID = GT.Id;
            var geoTagList = new contract.GeoTagList();
            int _counter;

            for (_counter = 0; _counter < GT.IsSOS.Count(); _counter++)
            {
                geoTagList.List.Add(new contract.GeoTag
                {
                    Alt = GT.Alt[_counter],
                    SessionID = GT.Id,
                    IsSOS = GT.IsSOS[_counter],
                    Lat = GT.Lat[_counter],
                    Long = GT.Long[_counter],
                    ProfileID = profileID,
                    Speed = GT.Spd != null ? GT.Spd[_counter] != null ? GT.Spd[_counter] : 0 : 0,
                    TimeStamp = GT.TS[_counter],
                    Accuracy = GT.Accuracy[_counter]
                });
            }
            return geoTagList;
        }

        internal static contract.Incident MakeContractIncident(entity.Incident incident)
        {
            return new contract.Incident
            {
                IncidentID = incident.IncidentID.Substring(incident.IncidentID.LastIndexOf("-") + 1),
                ProfileID = incident.ProfileID,
                Lat = incident.Latitude,
                Long = incident.Longitude,
                Alt = incident.Altitude,
                MediaUri = incident.MediaUri,
                DateTime = incident.DateTime,
                Type = incident.Type,
                AdditionalInfo = incident.AdditionalInfo,
                Name = incident.Name,
                MobileNumber =
                    (!String.IsNullOrEmpty(incident.MobileNumber))
                        ? utility.Security.Decrypt(incident.MobileNumber)
                        : null
            };
        }

        internal static void MakeBasicContractUserForCache(contract.User user, model.User eUser)
        {
            user.Email = eUser.Email;
            user.Name = eUser.Name;
            user.UserID = eUser.UserID;
            user.MobileNumber = utility.Security.Decrypt(eUser.MobileNumber);
        }
    }
}
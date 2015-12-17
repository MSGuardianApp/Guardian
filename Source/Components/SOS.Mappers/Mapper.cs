using System;
using System.Collections.Generic;
using System.Globalization;
using StorageEntity = SOS.AzureStorageAccessLayer.Entities;
using SQLModel = SOS.Model;
using DTOModel = SOS.AzureSQLAccessLayer;
using ServiceModel = SOS.Service.Interfaces.DataContracts;
using SOS.Service.Interfaces.DataContracts.OutBound;
using SOS.Service.Utility;

namespace SOS.Mappers
{
    public static class Mapper
    {
        public static void InitializeMappers()
        {
            AutoMapper.Mapper.CreateMap<SQLModel.LiveLocation, StorageEntity.LocationHistory>()
                .ForMember(d => d.ProfileID, opt => opt.MapFrom(s => s.ProfileID.ToString()))
                .ForMember(d => d.Accuracy, opt => opt.MapFrom(s => string.IsNullOrWhiteSpace(s.Accuracy) ? 0 : Convert.ToDouble(s.Accuracy)));

            AutoMapper.Mapper.CreateMap<DTOModel.LiveUserStatus, LiveUserStatus>()
                .ForMember(dest => dest.PID, opt => opt.MapFrom(src => src.ProfileID))
                .ForMember(dest => dest.N, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SID, opt => opt.MapFrom(src => src.SessionID))
                .ForMember(dest => dest.S, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.AT, opt => opt.MapFrom(src => src.DispatchInfo))
                .ForMember(dest => dest.M,
                   opt => opt.MapFrom(src => (src.Status == 1 || string.IsNullOrEmpty(src.MobileNumber)) ? string.Empty : Security.Decrypt(src.MobileNumber)));

            //Mapping from GeoTagLocs to GeoTag
            AutoMapper.Mapper.CreateMap<DTOModel.GeoTagLocs, ServiceModel.GeoTag>();

            AutoMapper.Mapper.CreateMap<DTOModel.UserLocation, ServiceModel.ProfileLite>();


            AutoMapper.Mapper.CreateMap<DTOModel.SOSTrackInfo, ServiceModel.SOSTrackCounts>();

            AutoMapper.Mapper.CreateMap<SQLModel.LiveSession, StorageEntity.SessionHistory>()
                 .ForMember(d => d.PartitionKey, opt => opt.MapFrom(s => s.ProfileID.ToString(CultureInfo.InvariantCulture)))
                 .ForMember(d => d.RowKey, opt => opt.MapFrom(s => s.ClientTimeStamp.Value.ToString(CultureInfo.InvariantCulture)))
                 .ForMember(d => d.NoOfSMSRecipients, opt => opt.MapFrom(s => s.NoOfSMSRecipients.HasValue ? s.NoOfSMSRecipients.Value : 0))
                 .ForMember(d => d.NoOfEmailRecipients, opt => opt.MapFrom(s => s.NoOfEmailRecipients.HasValue ? s.NoOfEmailRecipients.Value : 0))
                 .ForMember(d => d.NoOfSMSSent, opt => opt.MapFrom(s => s.NoOfSMSSent.HasValue ? s.NoOfSMSSent.Value : 0))
                 .ForMember(d => d.NoOfEmailsSent, opt => opt.MapFrom(s => s.NoOfEmailsSent.HasValue ? s.NoOfEmailsSent.Value : 0));

            AutoMapper.Mapper.CreateMap<SQLModel.LiveSession, SOS.Model.DTO.LiveSessionLite>();

            AutoMapper.Mapper.CreateMap<DTOModel.Member, Member>()
                .ForMember(dest => dest.P, opt => opt.MapFrom(src => src.ProfileID))
                .ForMember(dest => dest.N, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.E, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EE, opt => opt.MapFrom(src => src.EnterpriseEmail))
                .ForMember(dest => dest.J, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.M,
                   opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.MobileNumber)) ? string.Empty : Security.Decrypt(src.MobileNumber)));

            AutoMapper.Mapper.CreateMap<SOS.Service.Interfaces.DataContracts.GeoTag, SOS.Model.LiveLocation>()
                .ForMember(d => d.ClientTimeStamp, opt => opt.MapFrom(s => s.TimeStamp))
                .ForMember(d => d.ClientDateTime, opt => opt.MapFrom(s => new DateTime(s.TimeStamp)))
                .ForMember(d => d.CreatedDate, opt => opt.UseValue(DateTime.UtcNow))
                .ForMember(d => d.Accuracy, opt => opt.MapFrom(s => Convert.ToString(s.Accuracy)));

            AutoMapper.Mapper.CreateMap<SOS.AzureSQLAccessLayer.GeoTagLocs, SOS.Service.Interfaces.DataContracts.GeoTag>()
                .ForMember(d => d.Accuracy, opt => opt.MapFrom(s => (string.IsNullOrEmpty(s.Accuracy)) ? Convert.ToDouble("0") : Convert.ToDouble(s.Accuracy)));

            AutoMapper.Mapper.CreateMap<SOS.Service.Interfaces.DataContracts.Buddy, SOS.Model.Buddy>()
                .ForMember(d => d.BuddyName, opt => opt.MapFrom(s => s.Name))
                .ForMember(dest => dest.MobileNumber,
                   opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.MobileNumber)) ? string.Empty : Security.Encrypt(src.MobileNumber)));

            AutoMapper.Mapper.CreateMap<SOS.Model.LiveLocation, SOS.Service.Interfaces.DataContracts.GeoTag>()
                .ForMember(d => d.TimeStamp, opt => opt.MapFrom(src => src.ClientTimeStamp))
                .ForMember(d => d.Accuracy, opt => opt.MapFrom(s => (string.IsNullOrEmpty(s.Accuracy)) ? Convert.ToDouble("0") : Convert.ToDouble(s.Accuracy)));

            AutoMapper.Mapper.CreateMap<SOS.Model.LiveLocation, BasicGeoTag>()
                .ForMember(d => d.TimeStamp, opt => opt.MapFrom(src => src.ClientTimeStamp))
                .ForMember(d => d.Accuracy, opt => opt.MapFrom(s => (string.IsNullOrEmpty(s.Accuracy)) ? Convert.ToDouble("0") : Convert.ToDouble(s.Accuracy)));
        }

        public static StorageEntity.LocationHistory ConvertToHistory(this SQLModel.LiveLocation loc)
        {
            return AutoMapper.Mapper.Map<StorageEntity.LocationHistory>(loc);
        }

        public static List<LiveUserStatus> ConvertToLiveMemberStatusList(this List<DTOModel.LiveUserStatus> liveMemberStatusList)
        {
            return AutoMapper.Mapper.Map<List<LiveUserStatus>>(liveMemberStatusList);
        }

        public static List<SOS.Service.Interfaces.DataContracts.ProfileLite> ConvertToProfileLiteListContract(this List<DTOModel.UserLocation> userLocationList)
        {
            return AutoMapper.Mapper.Map<List<SOS.Service.Interfaces.DataContracts.ProfileLite>>(userLocationList);
        }

        public static ServiceModel.SOSTrackCounts ConvertToSOSTrackCounts(this DTOModel.SOSTrackInfo sosTrackInfo)
        {
            return AutoMapper.Mapper.Map<ServiceModel.SOSTrackCounts>(sosTrackInfo);
        }

        public static StorageEntity.SessionHistory ConvertToHistory(this SQLModel.LiveSession loc)
        {
            return AutoMapper.Mapper.Map<StorageEntity.SessionHistory>(loc);
        }

        public static List<StorageEntity.SessionHistory> ConvertToHistory(this List<SQLModel.LiveSession> locs)
        {
            return AutoMapper.Mapper.Map<List<StorageEntity.SessionHistory>>(locs);
        }

        public static List<SQLModel.DTO.LiveSessionLite> ConvertToLiveSessionLite(this List<SQLModel.LiveSession> locs)
        {
            return AutoMapper.Mapper.Map<List<SQLModel.DTO.LiveSessionLite>>(locs);
        }

        public static List<Member> ConvertToMembersList(this List<DTOModel.Member> memberStatusList)
        {
            return AutoMapper.Mapper.Map<List<Member>>(memberStatusList);
        }

        public static List<SOS.Model.LiveLocation> ConvertToLiveLocationList(this List<SOS.Service.Interfaces.DataContracts.GeoTag> geoTagList)
        {
            return AutoMapper.Mapper.Map<List<SOS.Model.LiveLocation>>(geoTagList);
        }

        public static SOS.Model.LiveLocation ConvertToLiveLocation(this SOS.Service.Interfaces.DataContracts.GeoTag geoTag)
        {
            return AutoMapper.Mapper.Map<SOS.Model.LiveLocation>(geoTag);
        }

        public static SOS.Model.Buddy ConvertToModelBuddy(this SOS.Service.Interfaces.DataContracts.Buddy buddy)
        {
            return AutoMapper.Mapper.Map<SOS.Model.Buddy>(buddy);
        }

        public static List<SOS.Service.Interfaces.DataContracts.GeoTag> ConvertToGeoTagList(this  List<SOS.Model.LiveLocation> liveLocationList)
        {
            return AutoMapper.Mapper.Map<List<SOS.Service.Interfaces.DataContracts.GeoTag>>(liveLocationList);
        }

        public static List<BasicGeoTag> ConvertToBasicGeoTagList(this  List<SOS.Model.LiveLocation> liveLocationList)
        {
            return AutoMapper.Mapper.Map<List<BasicGeoTag>>(liveLocationList);
        }
    }
}

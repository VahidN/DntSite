using AutoMapper;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.ModelsMappings;

public class LoggerMappingsProfiles : Profile
{
    public LoggerMappingsProfiles()
        => CreateMap<AppLogItem, AppLogItemModel>()
            .ForMember(appLogItemModel => appLogItemModel.UserFriendlyName, opt => opt.MapFrom(appLogItem
                => appLogItem.User != null ? appLogItem.User.FriendlyName :
                appLogItem.GuestUser != null ? appLogItem.GuestUser.UserName : ""))
            .ForMember(appLogItemModel => appLogItemModel.CreatedAt,
                opt => opt.MapFrom(appLogItem => appLogItem.Audit.CreatedAt))
            .ForMember(appLogItemModel => appLogItemModel.UserIp,
                opt => opt.MapFrom(appLogItem => appLogItem.Audit.CreatedByUserIp))
            .ForMember(appLogItemModel => appLogItemModel.UserAgent,
                opt => opt.MapFrom(appLogItem => appLogItem.Audit.CreatedByUserAgent));
}

using AutoMapper;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.ModelsMappings;

public class AppSettingsProfiles : Profile
{
    public AppSettingsProfiles()
    {
        MapAppSettingToModel();
        MapModelToAppSetting();
    }

    private void MapModelToAppSetting()
        => CreateMap<AppSettingModel, AppSetting>(MemberList.None)
            .ForMember(setting => setting.SiteRootUri,
                opt => opt.MapFrom(model => $"{model.SiteRootUri.TrimEnd('/')}/"))
            .ForMember(setting => setting.BannedUrls,
                opt => opt.MapFrom(model => model.BannedUrls.ConvertMultiLineTextToList()))
            .ForMember(setting => setting.BannedSites,
                opt => opt.MapFrom(model => model.BannedSites.ConvertMultiLineTextToList()))
            .ForMember(setting => setting.BannedReferrers,
                opt => opt.MapFrom(model => model.BannedReferrers.ConvertMultiLineTextToList()))
            .ForMember(setting => setting.BannedEmails,
                opt => opt.MapFrom(model => model.BannedEmails.ConvertMultiLineTextToList()))
            .ForMember(setting => setting.BannedPasswords,
                opt => opt.MapFrom(model => model.BannedPasswords.ConvertMultiLineTextToList()))
            .ForPath(setting => setting.GeminiNewsFeeds.ApiKey,
                opt => opt.MapFrom(model => model.GeminiNewsFeedsInfo.ApiKey))
            .ForPath(setting => setting.GeminiNewsFeeds.IsActive,
                opt => opt.MapFrom(model => model.GeminiNewsFeedsInfo.IsActive))
            .ForPath(setting => setting.GeminiNewsFeeds.NewsFeeds,
                opt => opt.MapFrom(model => model.GeminiNewsFeedsInfo.NewsFeeds.ConvertMultiLineTextToList()));

    private void MapAppSettingToModel()
        => CreateMap<AppSetting, AppSettingModel>(MemberList.None)
            .ForMember(model => model.SiteRootUri,
                opt => opt.MapFrom(setting => $"{setting.SiteRootUri.TrimEnd('/')}/"))
            .ForMember(model => model.BannedUrls,
                opt => opt.MapFrom(setting => setting.BannedUrls.ConvertListToMultiLineText()))
            .ForMember(model => model.BannedSites,
                opt => opt.MapFrom(setting => setting.BannedSites.ConvertListToMultiLineText()))
            .ForMember(model => model.BannedReferrers,
                opt => opt.MapFrom(setting => setting.BannedReferrers.ConvertListToMultiLineText()))
            .ForMember(model => model.BannedEmails,
                opt => opt.MapFrom(setting => setting.BannedEmails.ConvertListToMultiLineText()))
            .ForMember(model => model.BannedPasswords,
                opt => opt.MapFrom(setting => setting.BannedPasswords.ConvertListToMultiLineText()))
            .ForPath(model => model.GeminiNewsFeedsInfo.ApiKey,
                opt => opt.MapFrom(setting => setting.GeminiNewsFeeds.ApiKey))
            .ForPath(model => model.GeminiNewsFeedsInfo.IsActive,
                opt => opt.MapFrom(setting => setting.GeminiNewsFeeds.IsActive))
            .ForPath(model => model.GeminiNewsFeedsInfo.NewsFeeds,
                opt => opt.MapFrom(setting => setting.GeminiNewsFeeds.NewsFeeds.ConvertListToMultiLineText()));
}

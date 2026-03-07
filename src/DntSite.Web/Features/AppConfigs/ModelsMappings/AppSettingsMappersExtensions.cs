using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.ModelsMappings;

public static class AppSettingsMappersExtensions
{
    public static AppSetting MapAppSettingModelToAppSetting(this AppSettingModel source, AppSetting? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        var appSetting = new AppSetting
        {
            SiteRootUri = $"{source.SiteRootUri.TrimEnd(trimChar: '/')}/",
            BannedUrls = source.BannedUrls.ConvertMultiLineTextToList(),
            BannedSites = source.BannedSites.ConvertMultiLineTextToList(),
            BannedReferrers = source.BannedReferrers.ConvertMultiLineTextToList(),
            BannedEmails = source.BannedEmails.ConvertMultiLineTextToList(),
            BannedPasswords = source.BannedPasswords.ConvertMultiLineTextToList(),
            GeminiNewsFeeds = new GeminiNewsFeeds
            {
                ApiKey = source.GeminiNewsFeedsInfo.ApiKey,
                IsActive = source.GeminiNewsFeedsInfo.IsActive,
                NewsFeeds = source.GeminiNewsFeedsInfo.NewsFeeds.ConvertMultiLineTextToList()
            },
            BlogName = source.BlogName
        };

        if (destination is not null)
        {
            destination.SiteRootUri = appSetting.SiteRootUri;
            destination.BannedUrls = appSetting.BannedUrls;
            destination.BannedSites = appSetting.BannedSites;
            destination.BannedReferrers = appSetting.BannedReferrers;
            destination.BannedEmails = appSetting.BannedEmails;
            destination.BannedPasswords = appSetting.BannedPasswords;
            destination.GeminiNewsFeeds = appSetting.GeminiNewsFeeds;
        }

        return destination ?? appSetting;
    }

    public static AppSettingModel MapAppSettingToAppSettingModel(this AppSetting source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new AppSettingModel
        {
            SiteRootUri = $"{source.SiteRootUri.TrimEnd(trimChar: '/')}/",
            BannedUrls = source.BannedUrls.ConvertListToMultiLineText(),
            BannedSites = source.BannedSites.ConvertListToMultiLineText(),
            BannedReferrers = source.BannedReferrers.ConvertListToMultiLineText(),
            BannedEmails = source.BannedEmails.ConvertListToMultiLineText(),
            BannedPasswords = source.BannedPasswords.ConvertListToMultiLineText(),
            GeminiNewsFeedsInfo = new GeminiNewsFeedsModel
            {
                ApiKey = source.GeminiNewsFeeds.ApiKey,
                IsActive = source.GeminiNewsFeeds.IsActive,
                NewsFeeds = source.GeminiNewsFeeds.NewsFeeds.ConvertListToMultiLineText()
            }
        };
    }

    public static AppLogItemModel MapAppLogItemToAppLogItemModel(this AppLogItem source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var userFriendlyName = source.User?.FriendlyName ?? source.GuestUser?.UserName ?? "";

        return new AppLogItemModel
        {
            UserFriendlyName = userFriendlyName,
            CreatedAt = source.Audit.CreatedAt,
            UserIp = source.Audit.CreatedByUserIp,
            UserAgent = source.Audit.CreatedByUserAgent
        };
    }
}

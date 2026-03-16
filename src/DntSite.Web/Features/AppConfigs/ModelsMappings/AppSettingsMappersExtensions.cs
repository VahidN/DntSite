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
            MinimumRequiredPosts = source.MinimumRequiredPosts,
            BannedUrls = source.BannedUrls.ConvertMultiLineTextToList(),
            BannedSites = source.BannedSites.ConvertMultiLineTextToList(),
            BannedReferrers = source.BannedReferrers.ConvertMultiLineTextToList(),
            BannedEmails = source.BannedEmails.ConvertMultiLineTextToList(),
            BannedPasswords = source.BannedPasswords.ConvertMultiLineTextToList(),
            UsedPasswords = source.UsedPasswords,
            GeminiNewsFeeds = new GeminiNewsFeeds
            {
                ApiKey = source.GeminiNewsFeedsInfo.ApiKey,
                IsActive = source.GeminiNewsFeedsInfo.IsActive,
                NewsFeeds = source.GeminiNewsFeedsInfo.NewsFeeds.ConvertMultiLineTextToList()
            },
            ShowRssBriefDescription = source.ShowRssBriefDescription,
            ShouldCreateNewsScreenshots = source.ShouldCreateNewsScreenshots,
            YouTubeDataApikey = source.YouTubeDataApikey,
            BlogName = source.BlogName,
            SiteIsActive = source.SiteIsActive,
            DeactivateSiteAfterDaysOfInactivity = source.DeactivateSiteAfterDaysOfInactivity,
            SiteEmailsSig = source.SiteEmailsSig,
            SmtpServerSetting = source.SmtpServerSetting,
            SiteFromEmail = source.SiteFromEmail,
            CanUsersRegister = source.CanUsersRegister
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
            BlogName = source.BlogName,
            SiteIsActive = source.SiteIsActive,
            DeactivateSiteAfterDaysOfInactivity = source.DeactivateSiteAfterDaysOfInactivity,
            SiteEmailsSig = source.SiteEmailsSig,
            SmtpServerSetting = source.SmtpServerSetting,
            SiteFromEmail = source.SiteFromEmail,
            CanUsersRegister = source.CanUsersRegister,
            SiteRootUri = $"{source.SiteRootUri.TrimEnd(trimChar: '/')}/",
            MinimumRequiredPosts = source.MinimumRequiredPosts,
            BannedUrls = source.BannedUrls.ConvertListToMultiLineText(),
            BannedSites = source.BannedSites.ConvertListToMultiLineText(),
            BannedReferrers = source.BannedReferrers.ConvertListToMultiLineText(),
            BannedEmails = source.BannedEmails.ConvertListToMultiLineText(),
            BannedPasswords = source.BannedPasswords.ConvertListToMultiLineText(),
            UsedPasswords = source.UsedPasswords,
            GeminiNewsFeedsInfo = new GeminiNewsFeedsModel
            {
                ApiKey = source.GeminiNewsFeeds.ApiKey,
                IsActive = source.GeminiNewsFeeds.IsActive,
                NewsFeeds = source.GeminiNewsFeeds.NewsFeeds.ConvertListToMultiLineText()
            },
            ShowRssBriefDescription = source.ShowRssBriefDescription,
            ShouldCreateNewsScreenshots = source.ShouldCreateNewsScreenshots,
            YouTubeDataApikey = source.YouTubeDataApikey
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
            EventId = source.EventId,
            Url = source.Url,
            LogLevel = source.LogLevel,
            Logger = source.Logger,
            Message = source.Message,
            UserIp = source.Audit.CreatedByUserIp,
            UserAgent = source.Audit.CreatedByUserAgent
        };
    }
}

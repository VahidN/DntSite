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
            TelegramBackupGroup = source.TelegramBackupGroupInfo.MapToTelegramBackupGroup(),
            TelegramEPubGroup = source.TelegramEPubGroupInfo.MapToTelegramBackupGroup(),
            BaleBackupGroup = source.BaleBackupGroupInfo.MapToTelegramBackupGroup(),
            BaleEPubGroup = source.BaleEPubGroupInfo.MapToTelegramBackupGroup(),
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
            destination.SiteRootUri = $"{appSetting.SiteRootUri.TrimEnd(trimChar: '/')}/";
            destination.BlogName = appSetting.BlogName;
            destination.MinimumRequiredPosts = appSetting.MinimumRequiredPosts;
            destination.BannedUrls = appSetting.BannedUrls;
            destination.BannedSites = appSetting.BannedSites;
            destination.BannedReferrers = appSetting.BannedReferrers;
            destination.BannedEmails = appSetting.BannedEmails;
            destination.BannedPasswords = appSetting.BannedPasswords;
            destination.UsedPasswords = appSetting.UsedPasswords;

            destination.GeminiNewsFeeds = new GeminiNewsFeeds
            {
                ApiKey = appSetting.GeminiNewsFeeds.ApiKey,
                IsActive = appSetting.GeminiNewsFeeds.IsActive,
                NewsFeeds = appSetting.GeminiNewsFeeds.NewsFeeds
            };

            destination.TelegramBackupGroup = appSetting.TelegramBackupGroup.MapToTelegramBackupGroup();
            destination.TelegramEPubGroup = appSetting.TelegramEPubGroup.MapToTelegramBackupGroup();
            destination.BaleBackupGroup = appSetting.BaleBackupGroup.MapToTelegramBackupGroup();
            destination.BaleEPubGroup = appSetting.BaleEPubGroup.MapToTelegramBackupGroup();

            destination.ShowRssBriefDescription = appSetting.ShowRssBriefDescription;
            destination.ShouldCreateNewsScreenshots = appSetting.ShouldCreateNewsScreenshots;
            destination.YouTubeDataApikey = appSetting.YouTubeDataApikey;
            destination.SiteIsActive = appSetting.SiteIsActive;
            destination.DeactivateSiteAfterDaysOfInactivity = appSetting.DeactivateSiteAfterDaysOfInactivity;
            destination.SiteEmailsSig = appSetting.SiteEmailsSig;
            destination.SmtpServerSetting = appSetting.SmtpServerSetting;
            destination.SiteFromEmail = appSetting.SiteFromEmail;
            destination.CanUsersRegister = appSetting.CanUsersRegister;
        }

        return destination ?? appSetting;
    }

    public static TelegramBackupGroup MapToTelegramBackupGroup(this TelegramBackupGroup source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new TelegramBackupGroup
        {
            IsActive = source.IsActive,
            AccessToken = source.AccessToken,
            ChatId = source.ChatId,
            ZipPassword = source.ZipPassword,
            MaxZipPartSize = source.MaxZipPartSize
        };
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
            TelegramBackupGroupInfo = source.TelegramBackupGroup.MapToTelegramBackupGroup(),
            TelegramEPubGroupInfo = source.TelegramEPubGroup.MapToTelegramBackupGroup(),
            BaleBackupGroupInfo = source.BaleBackupGroup.MapToTelegramBackupGroup(),
            BaleEPubGroupInfo = source.BaleEPubGroup.MapToTelegramBackupGroup(),
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

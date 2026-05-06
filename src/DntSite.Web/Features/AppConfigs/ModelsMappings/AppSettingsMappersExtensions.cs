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
            TelegramBackupGroup = new TelegramBackupGroup
            {
                IsActive = source.TelegramBackupGroupInfo.IsActive,
                AccessToken = source.TelegramBackupGroupInfo.AccessToken,
                ChatId = source.TelegramBackupGroupInfo.ChatId,
                ZipPassword = source.TelegramBackupGroupInfo.ZipPassword
            },
            TelegramEPubGroup = new TelegramBackupGroup
            {
                IsActive = source.TelegramEPubGroupInfo.IsActive,
                AccessToken = source.TelegramEPubGroupInfo.AccessToken,
                ChatId = source.TelegramEPubGroupInfo.ChatId,
                ZipPassword = source.TelegramEPubGroupInfo.ZipPassword
            },
            BaleBackupGroup = new TelegramBackupGroup
            {
                IsActive = source.BaleBackupGroupInfo.IsActive,
                AccessToken = source.BaleBackupGroupInfo.AccessToken,
                ChatId = source.BaleBackupGroupInfo.ChatId,
                ZipPassword = source.BaleBackupGroupInfo.ZipPassword
            },
            BaleEPubGroup = new TelegramBackupGroup
            {
                IsActive = source.BaleEPubGroupInfo.IsActive,
                AccessToken = source.BaleEPubGroupInfo.AccessToken,
                ChatId = source.BaleEPubGroupInfo.ChatId,
                ZipPassword = source.BaleEPubGroupInfo.ZipPassword
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

            destination.TelegramBackupGroup = new TelegramBackupGroup
            {
                IsActive = appSetting.TelegramBackupGroup.IsActive,
                AccessToken = appSetting.TelegramBackupGroup.AccessToken,
                ChatId = appSetting.TelegramBackupGroup.ChatId,
                ZipPassword = appSetting.TelegramBackupGroup.ZipPassword
            };

            destination.TelegramEPubGroup = new TelegramBackupGroup
            {
                IsActive = appSetting.TelegramEPubGroup.IsActive,
                AccessToken = appSetting.TelegramEPubGroup.AccessToken,
                ChatId = appSetting.TelegramEPubGroup.ChatId,
                ZipPassword = appSetting.TelegramEPubGroup.ZipPassword
            };

            destination.BaleBackupGroup = new TelegramBackupGroup
            {
                IsActive = appSetting.BaleBackupGroup.IsActive,
                AccessToken = appSetting.BaleBackupGroup.AccessToken,
                ChatId = appSetting.BaleBackupGroup.ChatId,
                ZipPassword = appSetting.BaleBackupGroup.ZipPassword
            };

            destination.BaleEPubGroup = new TelegramBackupGroup
            {
                IsActive = appSetting.BaleEPubGroup.IsActive,
                AccessToken = appSetting.BaleEPubGroup.AccessToken,
                ChatId = appSetting.BaleEPubGroup.ChatId,
                ZipPassword = appSetting.BaleEPubGroup.ZipPassword
            };

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
            TelegramBackupGroupInfo = new TelegramBackupGroup
            {
                IsActive = source.TelegramBackupGroup.IsActive,
                AccessToken = source.TelegramBackupGroup.AccessToken,
                ChatId = source.TelegramBackupGroup.ChatId,
                ZipPassword = source.TelegramBackupGroup.ZipPassword
            },
            TelegramEPubGroupInfo = new TelegramBackupGroup
            {
                IsActive = source.TelegramEPubGroup.IsActive,
                AccessToken = source.TelegramEPubGroup.AccessToken,
                ChatId = source.TelegramEPubGroup.ChatId,
                ZipPassword = source.TelegramEPubGroup.ZipPassword
            },
            BaleBackupGroupInfo = new TelegramBackupGroup
            {
                IsActive = source.BaleBackupGroup.IsActive,
                AccessToken = source.BaleBackupGroup.AccessToken,
                ChatId = source.BaleBackupGroup.ChatId,
                ZipPassword = source.BaleBackupGroup.ZipPassword
            },
            BaleEPubGroupInfo = new TelegramBackupGroup
            {
                IsActive = source.BaleEPubGroup.IsActive,
                AccessToken = source.BaleEPubGroup.AccessToken,
                ChatId = source.BaleEPubGroup.ChatId,
                ZipPassword = source.BaleEPubGroup.ZipPassword
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

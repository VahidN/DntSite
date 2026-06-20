using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Models;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace DntSite.Web.Features.SiteBackup.Services;

public class TelegramUploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    IHttpClientFactory httpClientFactory,
    IEmailsFactoryService emailsFactoryService,
    ILogger<TelegramUploadBackupService> logger) : ITelegramUploadBackupService
{
    private const string InReplyTo = "TelegramUploadBackup";
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 20);

    public async Task<PartsInfo?> UploadSiteBackupFileToTelegramAsync(bool isFolder,
        string path,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var telegramBackupGroup = await GetTelegramBackupGroupAsync();

            if (telegramBackupGroup is null)
            {
                return null;
            }

            var tempDirectory = appFoldersService.GetTempDirectory();

            var zipPassword = telegramBackupGroup.ZipPassword;

            var maxPartSizeMB = telegramBackupGroup.MaxZipPartSize <= 0
                ? UploadBackupsExtensions.MaxPartSizeMB
                : telegramBackupGroup.MaxZipPartSize;

            var useProvidedParts = parts.UseProvidedParts(zipPassword);

            if (!useProvidedParts)
            {
                parts?.Parts.DeleteParts(logger);
            }

            var partPaths = useProvidedParts ? parts?.Parts :
                isFolder ? await path.ZipAndSplitFolderToMultiplePartsAsync(tempDirectory, maxPartSizeMB,
                    outputFileName, password: zipPassword, logger: logger, cancellationToken: cancellationToken) :
                await path.ZipAndSplitFileToMultiplePartsAsync(tempDirectory, maxPartSizeMB, outputFileName,
                    password: zipPassword, logger: logger, cancellationToken: cancellationToken);

            if (partPaths?.Count == 0)
            {
                return null;
            }

            using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);

            var telegramBotClient =
                new TelegramBotClient(telegramBackupGroup.AccessToken!, httpClient, cancellationToken);

            await UploadPartsAsync(telegramBotClient, telegramBackupGroup.ChatId!, partPaths,
                $"{(isFolder ? "پوشه " : "فایل ")}`{outputFileName}`", cancellationToken);

            await SendMessageAsync(telegramBotClient, telegramBackupGroup.ChatId!, partPaths, cancellationToken);

            return new PartsInfo(partPaths, zipPassword);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to UploadSiteBackupFileToTelegramAsync.");

            return null;
        }
    }

    public async Task<PartsInfo?> UploadSiteEPubFileToTelegramAsync(string filePath,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var telegramEPubGroup = await GetTelegramEPubGroupAsync();

            if (telegramEPubGroup is null)
            {
                return null;
            }

            var tempDirectory = appFoldersService.GetTempDirectory();

            var zipPassword = telegramEPubGroup.ZipPassword;

            var maxPartSizeMB = telegramEPubGroup.MaxZipPartSize <= 0
                ? UploadBackupsExtensions.MaxPartSizeMB
                : telegramEPubGroup.MaxZipPartSize;

            var useProvidedParts = parts.UseProvidedParts(zipPassword);

            if (!useProvidedParts)
            {
                parts?.Parts.DeleteParts(logger);
            }

            var partPaths = useProvidedParts
                ? parts?.Parts
                : await filePath.ZipAndSplitFileToMultiplePartsAsync(tempDirectory, maxPartSizeMB, outputFileName,
                    password: zipPassword, logger: logger, cancellationToken: cancellationToken);

            if (partPaths?.Count == 0)
            {
                return null;
            }

            using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);

            var telegramBotClient =
                new TelegramBotClient(telegramEPubGroup.AccessToken!, httpClient, cancellationToken);

            await UploadPartsAsync(telegramBotClient, telegramEPubGroup.ChatId!, partPaths,
                $"فایل `{filePath.GetFileName()}`", cancellationToken);

            await SendMessageAsync(telegramBotClient, telegramEPubGroup.ChatId!, partPaths, cancellationToken);

            return new PartsInfo(partPaths, zipPassword);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to UploadSiteEPubFileToTelegramAsync.");

            return null;
        }
    }

    private async Task<TelegramBackupGroup?> GetTelegramBackupGroupAsync()
    {
        var telegramBackupGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).TelegramBackupGroup;

        if (telegramBackupGroup.IsActive && !telegramBackupGroup.AccessToken.IsEmpty() &&
            !telegramBackupGroup.ChatId.IsEmpty())
        {
            return telegramBackupGroup;
        }

        if (logger.IsEnabled(LogLevel.Critical))
        {
            logger.LogCritical(message: "`TelegramBackupGroup` is not active or set.");
        }

        return null;
    }

    private async Task<TelegramBackupGroup?> GetTelegramEPubGroupAsync()
    {
        var telegramEPubGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).TelegramEPubGroup;

        if (telegramEPubGroup.IsActive && !telegramEPubGroup.AccessToken.IsEmpty() &&
            !telegramEPubGroup.ChatId.IsEmpty())
        {
            return telegramEPubGroup;
        }

        if (logger.IsEnabled(LogLevel.Critical))
        {
            logger.LogCritical(message: "`TelegramEPubGroup` is not active or set.");
        }

        return null;
    }

    private async Task UploadPartsAsync(TelegramBotClient telegramBotClient,
        string chatId,
        IList<string>? partPaths,
        string description,
        CancellationToken cancellationToken)
    {
        var totalParts = partPaths?.Count ?? 0;

        if (partPaths is null || totalParts == 0)
        {
            return;
        }

        var partNumber = 1;

        foreach (var partPath in partPaths)
        {
            await using (var content = File.OpenRead(partPath))
            {
                await telegramBotClient.SendDocument(chatId, new InputFileStream(content, partPath.GetFileName()), $"""
                     🔹 بخش {partNumber.ToPersianNumbers()} از {totalParts.ToPersianNumbers()} {description}
                     """, ParseMode.MarkdownV2, cancellationToken: cancellationToken);
            }

            partNumber++;

            await Task.Delay(_delay, cancellationToken);
        }

        await Task.Delay(_delay, cancellationToken);
    }

    private async Task SendMessageAsync(TelegramBotClient telegramBotClient,
        string chatId,
        IList<string>? partPaths,
        CancellationToken cancellationToken)
    {
        var text = partPaths.GetUploadMessage();

        if (text.IsEmpty())
        {
            return;
        }

        await telegramBotClient.SendMessage(chatId, text, ParseMode.Html, cancellationToken: cancellationToken);

        await emailsFactoryService.SendEmailToAllAdminsNormalAsync(InReplyTo, InReplyTo, InReplyTo,
            text.Replace(oldValue: "\n", newValue: "<br>", StringComparison.Ordinal)
                .WrapInDirectionalDiv(fontFamily: "inherit", fontSize: "inherit"),
            emailSubject: "ارسال فایل بک‌آپ به تلگرام");
    }
}

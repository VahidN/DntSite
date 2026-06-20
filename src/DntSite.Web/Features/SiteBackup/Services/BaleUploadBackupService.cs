using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Models;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Utils;

namespace DntSite.Web.Features.SiteBackup.Services;

public class BaleUploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    IHttpClientFactory httpClientFactory,
    IEmailsFactoryService emailsFactoryService,
    ILogger<BaleUploadBackupService> logger) : IBaleUploadBackupService
{
    private const string InReplyTo = "BaleUploadBackup";
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 20);

    public async Task<PartsInfo?> UploadSiteBackupFileToBaleAsync(bool isFolder,
        string path,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var baleBackupGroup = await GetBaleBackupGroupAsync();

            if (baleBackupGroup is null)
            {
                return null;
            }

            var tempDirectory = appFoldersService.GetTempDirectory();

            var zipPassword = baleBackupGroup.ZipPassword;

            var maxPartSizeMB = baleBackupGroup.MaxZipPartSize <= 0
                ? UploadBackupsExtensions.MaxPartSizeMB
                : baleBackupGroup.MaxZipPartSize;

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

            await UploadPartsAsync(httpClient, baleBackupGroup.AccessToken!, baleBackupGroup.ChatId!, partPaths,
                $"{(isFolder ? "پوشه " : "فایل ")}`{outputFileName}`", cancellationToken);

            await SendMessageAsync(httpClient, baleBackupGroup.AccessToken!, baleBackupGroup.ChatId!, partPaths,
                cancellationToken);

            return new PartsInfo(partPaths, zipPassword);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to UploadSiteBackupFileToBaleAsync.");

            return null;
        }
    }

    public async Task<PartsInfo?> UploadSiteEPubFileToBaleAsync(string filePath,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var baleEPubGroup = await GetBaleEPubGroupAsync();

            if (baleEPubGroup is null)
            {
                return null;
            }

            var tempDirectory = appFoldersService.GetTempDirectory();

            var zipPassword = baleEPubGroup.ZipPassword;

            var maxPartSizeMB = baleEPubGroup.MaxZipPartSize <= 0
                ? UploadBackupsExtensions.MaxPartSizeMB
                : baleEPubGroup.MaxZipPartSize;

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

            await UploadPartsAsync(httpClient, baleEPubGroup.AccessToken!, baleEPubGroup.ChatId!, partPaths,
                $"فایل `{filePath.GetFileName()}`", cancellationToken);

            await SendMessageAsync(httpClient, baleEPubGroup.AccessToken!, baleEPubGroup.ChatId!, partPaths,
                cancellationToken);

            return new PartsInfo(partPaths, zipPassword);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to UploadSiteEPubFileToBaleAsync.");

            return null;
        }
    }

    private async Task<TelegramBackupGroup?> GetBaleBackupGroupAsync()
    {
        var baleBackupGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).BaleBackupGroup;

        if (baleBackupGroup.IsActive && !baleBackupGroup.AccessToken.IsEmpty() && !baleBackupGroup.ChatId.IsEmpty())
        {
            return baleBackupGroup;
        }

        if (logger.IsEnabled(LogLevel.Critical))
        {
            logger.LogCritical(message: "`BaleBackupGroup` is not active or set.");
        }

        return null;
    }

    private async Task<TelegramBackupGroup?> GetBaleEPubGroupAsync()
    {
        var baleEPubGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).BaleEPubGroup;

        if (baleEPubGroup.IsActive && !baleEPubGroup.AccessToken.IsEmpty() && !baleEPubGroup.ChatId.IsEmpty())
        {
            return baleEPubGroup;
        }

        if (logger.IsEnabled(LogLevel.Critical))
        {
            logger.LogCritical(message: "`BaleEPubGroup` is not active or set.");
        }

        return null;
    }

    private async Task UploadPartsAsync(HttpClient httpClient,
        string baleToken,
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
            var uploadedSize = new FileInfo(partPath).Length;

            var status = await httpClient.SendFileToBaleChannelAsync(baleToken, chatId, BaleFileType.Document, partPath,
                $"""
                 🔹 بخش {partNumber.ToPersianNumbers()} از {totalParts.ToPersianNumbers()} {description}
                 """, BaleParseMode.MarkdownV2, cancellationToken);

            LogBaleErrors(status);

            if (IsFailed(status))
            {
                return;
            }

            partNumber++;

            await Task.Delay(_delay, cancellationToken);
        }

        await Task.Delay(_delay, cancellationToken);
    }

    private static bool IsFailed(BaleApiResponseStatus? status) => status is not null and not { Success: true };

    private async Task SendMessageAsync(HttpClient httpClient,
        string baleToken,
        string chatId,
        IList<string>? partPaths,
        CancellationToken cancellationToken)
    {
        var text = partPaths.GetUploadMessage();

        if (text.IsEmpty())
        {
            return;
        }

        var status =
            await httpClient.SendTextMessageToBaleChannelAsync(baleToken, chatId, text, BaleParseMode.Html,
                cancellationToken);

        await emailsFactoryService.SendEmailToAllAdminsNormalAsync(InReplyTo, InReplyTo, InReplyTo,
            text.Replace(oldValue: "\n", newValue: "<br>", StringComparison.Ordinal)
                .WrapInDirectionalDiv(fontFamily: "inherit", fontSize: "inherit"),
            emailSubject: "ارسال فایل بک‌آپ به بله");

        LogBaleErrors(status);
    }

    private void LogBaleErrors(BaleApiResponseStatus? status)
    {
        if (IsFailed(status) && logger.IsEnabled(LogLevel.Error))
        {
            logger.LogError(
                message: "Send message to Bale failed. StatusCode:`{StatusCode}`, ResponseContent:`{Response}`.",
                status?.StatusCode, status?.ResponseContent);
        }
    }
}

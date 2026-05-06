using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Models;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Utils;

namespace DntSite.Web.Features.SiteBackup.Services;

public class BaleUploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    IHttpClientFactory httpClientFactory,
    ILogger<BaleUploadBackupService> logger) : IBaleUploadBackupService
{
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 20);

    public async Task<PartsInfo?> UploadSiteBackupFileToBaleAsync(bool isFolder,
        string path,
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

            var partPaths = parts.UseProvidedParts(zipPassword) ? parts?.Parts :
                isFolder ? await path.ZipAndSplitFolderToMultiplePartsAsync(tempDirectory,
                    UploadBackupsExtensions.MaxPartSizeMB, password: zipPassword, logger: logger,
                    cancellationToken: cancellationToken) :
                await path.ZipAndSplitFileToMultiplePartsAsync(tempDirectory, UploadBackupsExtensions.MaxPartSizeMB,
                    password: zipPassword, logger: logger, cancellationToken: cancellationToken);

            if (partPaths?.Count == 0)
            {
                return null;
            }

            using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);

            await UploadPartsAsync(httpClient, baleBackupGroup.AccessToken!, baleBackupGroup.ChatId!, partPaths,
                cancellationToken);

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

            var partPaths = parts.UseProvidedParts(zipPassword)
                ? parts?.Parts
                : await filePath.ZipAndSplitFileToMultiplePartsAsync(tempDirectory,
                    UploadBackupsExtensions.MaxPartSizeMB, password: zipPassword, logger: logger,
                    cancellationToken: cancellationToken);

            if (partPaths?.Count == 0)
            {
                return null;
            }

            using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);

            await UploadPartsAsync(httpClient, baleEPubGroup.AccessToken!, baleEPubGroup.ChatId!, partPaths,
                cancellationToken);

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

            await httpClient.SendFileToBaleChannelAsync(baleToken, chatId, BaleFileType.Document, partPath, $"""
                 🔹 بخش {partNumber.ToPersianNumbers()} از {totalParts.ToPersianNumbers()}
                 📏 حجم بخش: {uploadedSize.ToFormattedFileSize()}
                 """, cancellationToken);

            partNumber++;

            await Task.Delay(_delay, cancellationToken);
        }

        await Task.Delay(_delay, cancellationToken);
    }

    private static async Task SendMessageAsync(HttpClient httpClient,
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

        await httpClient.SendTextMessageToBaleChannelAsync(baleToken, chatId, text, cancellationToken);
    }
}

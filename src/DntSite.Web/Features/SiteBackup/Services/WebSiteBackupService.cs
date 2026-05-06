using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Utils;

namespace DntSite.Web.Features.SiteBackup.Services;

public class WebSiteBackupService(
    IOnlineSqliteBackupService onlineSqliteBackupService,
    ITelegramUploadBackupService telegramUploadBackupService,
    IBaleUploadBackupService baleUploadBackupService,
    IAppFoldersService appFoldersService,
    ILogger<WebSiteBackupService> logger) : IWebSiteBackupService
{
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 7);

    public async Task CreateSiteBackupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsConnectedToInternet())
            {
                return;
            }

            DeleteOldZipFiles();

            var dbBackupFilePath = GetDbBackupFilePath();

            if (await onlineSqliteBackupService.CreateOnlineSqliteBackupAsync(dbBackupFilePath, cancellationToken) &&
                await onlineSqliteBackupService.ValidateSqliteBackupAsync(dbBackupFilePath, cancellationToken))
            {
                await CompressAndUploadDatabaseBackupFileAsync(dbBackupFilePath, cancellationToken);
            }

            await CompressAndUploadDataFolderBackupFileAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to upload site's backup file.");
        }
    }

    public async Task UploadSiteEPubFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsConnectedToInternet())
            {
                return;
            }

            var telegramPartsInfo = await telegramUploadBackupService.UploadSiteEPubFileToTelegramAsync(filePath,
                parts: null, cancellationToken);

            var balePartsInfo =
                await baleUploadBackupService.UploadSiteEPubFileToBaleAsync(filePath, telegramPartsInfo,
                    cancellationToken);

            filePath.TryDeleteFile(logger);
            telegramPartsInfo?.Parts.DeleteParts(logger);
            balePartsInfo?.Parts.DeleteParts(logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to upload site's epub-backup file.");
        }
    }

    private bool IsConnectedToInternet()
    {
        if (NetworkExtensions.IsConnectedToInternet(_delay))
        {
            return true;
        }

        if (logger.IsEnabled(LogLevel.Critical))
        {
            logger.LogCritical(message: "There is no internet connection to run UploadBackupService.");
        }

        return false;
    }

    private void DeleteOldZipFiles()
        => appFoldersService.BackupFolderPath.DeleteFiles(SearchOption.AllDirectories, "*.*");

    private string GetDbBackupFilePath()
    {
        var dbBackupFileName = string.Create(CultureInfo.InvariantCulture,
            $"db.backup.{DateTime.UtcNow:yyyyMMdd_HHmmss}.{Guid.CryptographicallySecureGuid:N}.sqlite");

        return appFoldersService.BackupFolderPath.SafePathCombine(dbBackupFileName)!.Replace(oldValue: "'",
            newValue: "''", StringComparison.Ordinal);
    }

    private async Task CompressAndUploadDataFolderBackupFileAsync(CancellationToken cancellationToken)
    {
        try
        {
            var telegramPartsInfo =
                await telegramUploadBackupService.UploadSiteBackupFileToTelegramAsync(isFolder: true,
                    appFoldersService.UploadsFolderPath, parts: null, cancellationToken);

            var balePartsInfo = await baleUploadBackupService.UploadSiteBackupFileToBaleAsync(isFolder: true,
                appFoldersService.UploadsFolderPath, telegramPartsInfo, cancellationToken);

            telegramPartsInfo?.Parts.DeleteParts(logger);
            balePartsInfo?.Parts.DeleteParts(logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to compress and upload data backup file.");
        }
    }

    private async Task CompressAndUploadDatabaseBackupFileAsync(string dbBackupFilePath,
        CancellationToken cancellationToken)
    {
        try
        {
            var telegramPartsInfo =
                await telegramUploadBackupService.UploadSiteBackupFileToTelegramAsync(isFolder: false, dbBackupFilePath,
                    parts: null, cancellationToken);

            var balePartsInfo = await baleUploadBackupService.UploadSiteBackupFileToBaleAsync(isFolder: false,
                dbBackupFilePath, telegramPartsInfo, cancellationToken);

            await Task.Delay(_delay, cancellationToken);

            dbBackupFilePath.TryDeleteFile(logger);
            telegramPartsInfo?.Parts.DeleteParts(logger);
            balePartsInfo?.Parts.DeleteParts(logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to compress and upload DB backup file.");
        }
    }
}

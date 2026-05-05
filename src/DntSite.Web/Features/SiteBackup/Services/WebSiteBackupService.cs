using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using Microsoft.Data.Sqlite;

namespace DntSite.Web.Features.SiteBackup.Services;

public class WebSiteBackupService(
    ITelegramUploadBackupService telegramUploadBackupService,
    IAppFoldersService appFoldersService,
    ILogger<WebSiteBackupService> logger) : IWebSiteBackupService
{
    private const int MaxPartSizeInBytes = 45 * 1024 * 1024; // 45 مگابایت
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 7);

    public async Task CreateBackupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!NetworkExtensions.IsConnectedToInternet(_delay))
            {
                if (logger.IsEnabled(LogLevel.Critical))
                {
                    logger.LogCritical(message: "There is no internet connection to run UploadBackupService.");
                }

                return;
            }

            DeleteOldZipFiles();

            var dbBackupFilePath = GetDbBackupFilePath();

            if (await CreateOnlineSqliteBackupAsync(dbBackupFilePath, cancellationToken) &&
                await ValidateSqliteBackupAsync(dbBackupFilePath, cancellationToken))
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
            if (!NetworkExtensions.IsConnectedToInternet(_delay))
            {
                if (logger.IsEnabled(LogLevel.Critical))
                {
                    logger.LogCritical(message: "There is no internet connection to run UploadBackupService.");
                }

                return;
            }

            var tempDirectory = appFoldersService.GetTempDirectory();

            var partPaths = await filePath.ZipAndSplitFileToMultiplePartsAsync(tempDirectory,
                MaxPartSizeInBytes.ToMegabytes(FileSizeUnit.Byte), logger: logger,
                cancellationToken: cancellationToken);

            await telegramUploadBackupService.UploadSiteEPubFileToTelegramAsync(partPaths, cancellationToken);
            DeleteParts(partPaths);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to upload site's epub-backup file.");
        }
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
            var tempDirectory = appFoldersService.GetTempDirectory();

            var partPaths = await appFoldersService.UploadsFolderPath.ZipAndSplitFolderToMultiplePartsAsync(
                tempDirectory, MaxPartSizeInBytes.ToMegabytes(FileSizeUnit.Byte), logger: logger,
                cancellationToken: cancellationToken);

            await telegramUploadBackupService.UploadSiteBackupFileToTelegramAsync(partPaths, cancellationToken);
            DeleteParts(partPaths);
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
            var tempDirectory = appFoldersService.GetTempDirectory();

            var partPaths = await dbBackupFilePath.ZipAndSplitFileToMultiplePartsAsync(tempDirectory,
                MaxPartSizeInBytes.ToMegabytes(FileSizeUnit.Byte), logger: logger,
                cancellationToken: cancellationToken);

            await telegramUploadBackupService.UploadSiteBackupFileToTelegramAsync(partPaths, cancellationToken);

            await Task.Delay(_delay, cancellationToken);

            dbBackupFilePath.TryDeleteFile(logger);
            DeleteParts(partPaths);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to compress and upload DB backup file.");
        }
    }

    private void DeleteParts(IList<string>? partPaths)
    {
        if (partPaths is not null)
        {
            foreach (var partPath in partPaths)
            {
                partPath.TryDeleteFile(logger);
            }
        }
    }

    private async Task<bool> CreateOnlineSqliteBackupAsync(string dbBackupFilePath, CancellationToken cancellationToken)
    {
        try
        {
            var sourceConnectionString = appFoldersService.DefaultConnectionString;

            var connectionString =
                sourceConnectionString.Contains(value: "Pooling=", StringComparison.OrdinalIgnoreCase)
                    ? sourceConnectionString
                    : $"{sourceConnectionString};Pooling=False";

            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandTimeout = 0;

            command.CommandText = $"""
                                       PRAGMA journal_mode = WAL;
                                       PRAGMA busy_timeout = 10000;
                                       VACUUM INTO '{dbBackupFilePath}';
                                   """;

            await command.ExecuteNonQueryAsync(cancellationToken);

            await Task.Delay(_delay, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to validate DB backup file.");

            return false;
        }
    }

    private async Task<bool> ValidateSqliteBackupAsync(string dbBackupFilePath, CancellationToken cancellationToken)
    {
        try
        {
            if (!dbBackupFilePath.FileExists())
            {
                return false;
            }

            var backupConnectionString = $"Data Source={dbBackupFilePath};Mode=ReadOnly;Pooling=False";

            await using var backupConnection = new SqliteConnection(backupConnectionString);
            await backupConnection.OpenAsync(cancellationToken);

            await using var command = backupConnection.CreateCommand();
            command.CommandText = "PRAGMA quick_check;";
            command.CommandTimeout = 0;

            var result = await command.ExecuteScalarAsync(cancellationToken);

            var success = result is not null && string.Equals(string.Create(CultureInfo.InvariantCulture, $"{result}"),
                b: "ok", StringComparison.OrdinalIgnoreCase);

            if (!success && logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "SQLite backup integrity check failed: {Result}", result);
            }

            await Task.Delay(_delay, cancellationToken);

            return success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to validate DB backup file.");

            return false;
        }
    }
}

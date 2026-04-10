using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using Microsoft.Data.Sqlite;

namespace DntSite.Web.Features.SiteBackup.Services;

public class WebSiteBackupService(
    ITelegramUploadBackupService uploadBackupService,
    IAppFoldersService appFoldersService,
    ILogger<WebSiteBackupService> logger) : IWebSiteBackupService
{
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 7);

    public async Task CreateBackupAsync(CancellationToken cancellationToken = default)
    {
        DeleteOldZipFiles();

        var dbBackupFilePath = GetDbBackupFilePath();

        if (await CreateOnlineSqliteBackupAsync(dbBackupFilePath, cancellationToken) &&
            await ValidateSqliteBackupAsync(dbBackupFilePath, cancellationToken))
        {
            await CompressAndUploadDbBackupFileAsync(dbBackupFilePath, cancellationToken);
        }

        await CompressAndUploadDataBackupFileAsync(cancellationToken);
    }

    private void DeleteOldZipFiles()
        => appFoldersService.BackupFolderPath.DeleteFiles(SearchOption.AllDirectories, "*.zip");

    private string GetDbBackupFilePath()
    {
        var dbBackupFileName = string.Create(CultureInfo.InvariantCulture,
            $"db.backup.{DateTime.UtcNow:yyyyMMdd_HHmmss}.{Guid.CryptographicallySecureGuid:N}.sqlite");

        return appFoldersService.BackupFolderPath.SafePathCombine(dbBackupFileName)!.Replace(oldValue: "'",
            newValue: "''", StringComparison.Ordinal);
    }

    private async Task CompressAndUploadDataBackupFileAsync(CancellationToken cancellationToken)
    {
        try
        {
            var dataBackupFileName = string.Create(CultureInfo.InvariantCulture,
                $"uploads.{DateTime.UtcNow:yyyyMMdd_HHmmss}.{Guid.CryptographicallySecureGuid:N}.zip");

            var dataBackupFilePath = appFoldersService.BackupFolderPath.SafePathCombine(dataBackupFileName)!;

            appFoldersService.UploadsFolderPath.CompressFolderToZipFile(dataBackupFilePath);
            await Task.Delay(_delay, cancellationToken);

            await uploadBackupService.UploadSiteBackupFileToTelegramAsync(dataBackupFilePath, cancellationToken);
            await Task.Delay(_delay, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to compress and upload data backup file.");
        }
    }

    private async Task CompressAndUploadDbBackupFileAsync(string dbBackupFilePath, CancellationToken cancellationToken)
    {
        try
        {
            var backupZipFilePath = $"{dbBackupFilePath}.zip";
            backupZipFilePath.CompressFilesToZipFile(dbBackupFilePath);
            await Task.Delay(_delay, cancellationToken);
            dbBackupFilePath.TryDeleteFile(logger);

            await uploadBackupService.UploadSiteBackupFileToTelegramAsync(backupZipFilePath, cancellationToken);
            await Task.Delay(_delay, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to compress and upload DB backup file.");
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

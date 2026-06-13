namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IOnlineSqliteBackupService : IScopedService
{
    Task<bool> CreateOnlineSqliteBackupAsync(string dbBackupFilePath, CancellationToken cancellationToken = default);

    Task<bool> ValidateSqliteBackupAsync(string dbBackupFilePath, CancellationToken cancellationToken = default);
}

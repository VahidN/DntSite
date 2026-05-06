namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IOnlineSqliteBackupService : ISingletonService
{
    Task<bool> CreateOnlineSqliteBackupAsync(string dbBackupFilePath, CancellationToken cancellationToken = default);

    Task<bool> ValidateSqliteBackupAsync(string dbBackupFilePath, CancellationToken cancellationToken = default);
}

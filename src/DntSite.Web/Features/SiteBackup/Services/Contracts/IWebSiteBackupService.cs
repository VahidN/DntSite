namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IWebSiteBackupService : IScopedService
{
    Task CreateDatabaseBackupAsync(CancellationToken cancellationToken = default);

    Task CompressAndUploadDataFolderBackupFileAsync(CancellationToken cancellationToken);

    Task UploadSiteEPubFileAsync(string filePath, CancellationToken cancellationToken = default);
}

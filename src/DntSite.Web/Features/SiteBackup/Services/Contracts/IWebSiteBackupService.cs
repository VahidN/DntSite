namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IWebSiteBackupService : ISingletonService
{
    Task CreateBackupAsync(CancellationToken cancellationToken = default);

    Task UploadSiteEPubFileAsync(string filePath, CancellationToken cancellationToken = default);
}

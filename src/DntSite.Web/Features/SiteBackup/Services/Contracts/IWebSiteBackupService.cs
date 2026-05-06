namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IWebSiteBackupService : ISingletonService
{
    Task CreateSiteBackupAsync(CancellationToken cancellationToken = default);

    Task UploadSiteEPubFileAsync(string filePath, CancellationToken cancellationToken = default);
}

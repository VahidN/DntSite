using DntSite.Web.Features.SiteBackup.Models;

namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IBaleUploadBackupService : ISingletonService
{
    Task<PartsInfo?> UploadSiteBackupFileToBaleAsync(bool isFolder,
        string path,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default);

    Task<PartsInfo?> UploadSiteEPubFileToBaleAsync(string filePath,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default);
}

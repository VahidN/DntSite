using DntSite.Web.Features.SiteBackup.Models;

namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IBaleUploadBackupService : ISingletonService
{
    Task<PartsInfo?> UploadSiteBackupFileToBaleAsync(bool isFolder,
        string path,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default);

    Task<PartsInfo?> UploadSiteEPubFileToBaleAsync(string filePath,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default);
}

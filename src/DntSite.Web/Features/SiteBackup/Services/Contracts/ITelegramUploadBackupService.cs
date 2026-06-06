using DntSite.Web.Features.SiteBackup.Models;

namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface ITelegramUploadBackupService : ISingletonService
{
    Task<PartsInfo?> UploadSiteBackupFileToTelegramAsync(bool isFolder,
        string path,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default);

    Task<PartsInfo?> UploadSiteEPubFileToTelegramAsync(string filePath,
        string? outputFileName,
        PartsInfo? parts = null,
        CancellationToken cancellationToken = default);
}

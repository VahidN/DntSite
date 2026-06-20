namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IMegaNzUploadBackupService : IScopedService
{
    Task UploadSiteBackupFileToMegaNzAsync(bool isFolder,
        string path,
        string? outputFileName,
        CancellationToken cancellationToken = default);

    Task UploadSiteEPubFileToMegaNzAsync(string filePath,
        string? outputFileName,
        CancellationToken cancellationToken = default);
}

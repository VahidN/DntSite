namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface IUploadBackupService : ISingletonService
{
    Task UploadToHostAsync(string filePath, CancellationToken cancellationToken = default);
}

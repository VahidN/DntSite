namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface ITelegramUploadBackupService : ISingletonService
{
    Task UploadSiteBackupFileToTelegramAsync(IList<string>? partPaths, CancellationToken cancellationToken = default);

    Task UploadSiteEPubFileToTelegramAsync(IList<string>? partPaths, CancellationToken cancellationToken = default);
}

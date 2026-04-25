namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface ITelegramUploadBackupService : ISingletonService
{
    Task UploadSiteBackupFileToTelegramAsync(string filePath, CancellationToken cancellationToken = default);

    Task UploadSiteEPubFileToTelegramAsync(string filePath, CancellationToken cancellationToken = default);

    Task UploadFileToTelegramAsync(string filePath,
        string accessToken,
        string chatId,
        CancellationToken cancellationToken = default);
}

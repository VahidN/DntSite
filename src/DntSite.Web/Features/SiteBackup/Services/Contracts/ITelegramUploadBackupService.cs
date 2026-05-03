using DntSite.Web.Features.SiteBackup.Models;

namespace DntSite.Web.Features.SiteBackup.Services.Contracts;

public interface ITelegramUploadBackupService : ISingletonService
{
    Task UploadSiteBackupFileToTelegramAsync(string filePath,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default);

    Task UploadSiteEPubFileToTelegramAsync(string filePath,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default);

    Task UploadFileToTelegramAsync(string filePath,
        string accessToken,
        string chatId,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default);

    Task UploadSiteFolderContentsToTelegramAsync(string folderPath,
        string outputZipFilePath,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default);

    Task UploadFolderContentsToTelegramAsync(string folderPath,
        string outputZipFilePath,
        string accessToken,
        string chatId,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default);
}

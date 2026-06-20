using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Utils;

namespace DntSite.Web.Features.SiteBackup.Services;

public class MegaNzUploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    IHttpClientFactory httpClientFactory,
    IEmailsFactoryService emailsFactoryService,
    ILogger<MegaNzUploadBackupService> logger) : IMegaNzUploadBackupService
{
    private const string BackupUploadFolderNameOnMegaNz = "Backup";
    private const string EPubUploadFolderNameOnMegaNz = "DNT";
    private const string InReplyTo = "MegaUploadBackup";

    public async Task UploadSiteBackupFileToMegaNzAsync(bool isFolder,
        string path,
        string? outputFileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var megaNzBackup = await GetMegaNzInfoAsync();

            if (megaNzBackup is null)
            {
                return;
            }

            var tempDirectory = appFoldersService.GetTempDirectory();
            var zipPassword = megaNzBackup.ZipPassword;

            ICollection<string> partPaths = (isFolder
                ? await path.ZipAndSplitFolderToMultiplePartsAsync(tempDirectory, partSizeMB: null, outputFileName,
                    password: zipPassword, logger: logger, cancellationToken: cancellationToken)
                : await path.ZipAndSplitFileToMultiplePartsAsync(tempDirectory, partSizeMB: null, outputFileName,
                    password: zipPassword, logger: logger, cancellationToken: cancellationToken)) ?? [];

            if (partPaths.Count == 0)
            {
                return;
            }

            try
            {
                using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);

                await httpClient.UploadFilesToMegaNzAsync(megaNzBackup.MegaEmail!, megaNzBackup.MegaPassword!,
                    partPaths, megaNzBackup.MegaBackupFolder ?? BackupUploadFolderNameOnMegaNz,
                    megaNzBackup.KeepLastNFilesOnMegaNz, cancellationToken);

                await SendMessageAsync(partPaths);
            }
            finally
            {
                partPaths.TryDeleteFiles(logger);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to UploadSiteBackupFileToMegaNzAsync.");
        }
    }

    public async Task UploadSiteEPubFileToMegaNzAsync(string filePath,
        string? outputFileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var megaNzBackup = await GetMegaNzInfoAsync();

            if (megaNzBackup is null)
            {
                return;
            }

            var tempDirectory = appFoldersService.GetTempDirectory();

            ICollection<string> partPaths = await filePath.ZipAndSplitFileToMultiplePartsAsync(tempDirectory,
                                                partSizeMB: null, outputFileName, password: "", logger: logger,
                                                cancellationToken: cancellationToken) ??
                                            [
                                            ];

            if (partPaths.Count == 0)
            {
                return;
            }

            try
            {
                using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);

                await httpClient.UploadFilesToMegaNzAsync(megaNzBackup.MegaEmail!, megaNzBackup.MegaPassword!,
                    partPaths, megaNzBackup.MegaEPubFolder ?? EPubUploadFolderNameOnMegaNz,
                    megaNzBackup.KeepLastNFilesOnMegaNz, cancellationToken);

                await SendMessageAsync(partPaths);
            }
            finally
            {
                partPaths.TryDeleteFiles(logger);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to UploadSiteEPubFileToMegaNzAsync.");
        }
    }

    private async Task<MegaNzBackup?> GetMegaNzInfoAsync()
    {
        var megaNzBackup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).MegaNzBackup;

        if (megaNzBackup.IsActive && !megaNzBackup.MegaEmail.IsEmpty() && !megaNzBackup.MegaPassword.IsEmpty())
        {
            return megaNzBackup;
        }

        if (logger.IsEnabled(LogLevel.Critical))
        {
            logger.LogCritical(message: "`MegaNzBackup` is not active or set.");
        }

        return null;
    }

    private async Task SendMessageAsync(ICollection<string>? partPaths)
    {
        var text = partPaths.GetUploadMessage();

        if (text.IsEmpty())
        {
            return;
        }

        await emailsFactoryService.SendEmailToAllAdminsNormalAsync(InReplyTo, InReplyTo, InReplyTo,
            text.Replace(oldValue: "\n", newValue: "<br>", StringComparison.Ordinal)
                .WrapInDirectionalDiv(fontFamily: "inherit", fontSize: "inherit"),
            emailSubject: "ارسال فایل بک‌آپ به مگا‌ان‌زد");
    }
}

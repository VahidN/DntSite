using DntSite.Web.Features.AppConfigs.EmailLayouts;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.AppConfigs.Services;

public sealed class AppConfigsEmailsService : IAppConfigsEmailsService
{
    private readonly IDisposable? _disposableSettings;
    private readonly IEmailsFactoryService _emailsFactoryService;
    private readonly IExecuteApplicationProcess _executeApplicationProcess;
    private readonly IWebServerInfoService _webServerInfoService;
    private StartupSettingsModel _siteSettings;

    public AppConfigsEmailsService(IEmailsFactoryService emailsFactoryService,
        IExecuteApplicationProcess executeApplicationProcess,
        IWebServerInfoService webServerInfoService,
        IOptionsMonitor<StartupSettingsModel> siteSettings)
    {
        ArgumentNullException.ThrowIfNull(siteSettings);

        _emailsFactoryService = emailsFactoryService;
        _executeApplicationProcess = executeApplicationProcess;
        _webServerInfoService = webServerInfoService;
        _siteSettings = siteSettings.CurrentValue;
        _disposableSettings = siteSettings.OnChange(settings => _siteSettings = settings);
    }

    public async Task SendHasNotRemainingSpaceEmailToAdminsAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var info = await _webServerInfoService.GetWebServerInfoAsync();

        var hasRemainingSpace = info.ServerInfo.DriveInfo.AvailableFreeSpaceToCurrentUserInBytes >=
                                _siteSettings.MaxAvailableFreeSpaceInMegaBytes * 1024L * 1024L;

        if (hasRemainingSpace)
        {
            return;
        }

        await _emailsFactoryService
            .SendEmailToAllAdminsAsync<HasNotRemainingSpaceEmail, HasNotRemainingSpaceEmailModel>(
                messageId: "HasNotRemainingSpace", inReplyTo: "", references: "HasNotRemainingSpace",
                new HasNotRemainingSpaceEmailModel
                {
                    Body = $"فضای خالی باقیمانده: {info.ServerInfo.DriveInfo.AvailableSpaceToCurrentUser}"
                }, emailSubject: "جهت اطلاع: فضای خالی باقیمانده بر روی سرور", cancellationToken);
    }

    public async Task SendNewDotNetVersionEmailToAdminsAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var info = await _executeApplicationProcess.ExecuteProcessAsync(new ApplicationStartInfo
        {
            ProcessName = "dotnet",
            Arguments = "sdk check",
            AppPath = "dotnet",
            WaitForExit = TimeSpan.FromSeconds(value: 3),
            KillProcessOnStart = false
        }, cancellationToken);

        if (IsNewVersionAvailable(info))
        {
            await _emailsFactoryService.SendEmailToAllAdminsAsync<NewDotNetVersionEmail, NewDotNetVersionEmailModel>(
                messageId: "NewDotNetVersion", inReplyTo: "", references: "NewDotNetVersion",
                new NewDotNetVersionEmailModel
                {
                    Body = info
                }, emailSubject: "جهت اطلاع: نگارش جدیدی از دات‌نت برای نصب", cancellationToken);
        }
    }

    public void Dispose() => _disposableSettings?.Dispose();

    /// <summary>
    ///     https://github.com/dotnet/sdk/blob/a34f1ca17979f6cb283ad74c53ca68f8575fceb9/src/Cli/dotnet/commands/dotnet-sdk/check/LocalizableStrings.resx
    /// </summary>
    private static bool IsNewVersionAvailable(string info)
        => info.Contains(value: "is available", StringComparison.OrdinalIgnoreCase) || (!OperatingSystem.IsLinux() &&
            info.Contains(value: "newest", StringComparison.OrdinalIgnoreCase));
}

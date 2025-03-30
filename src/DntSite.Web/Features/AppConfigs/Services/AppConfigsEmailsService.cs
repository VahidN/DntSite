using DntSite.Web.Features.AppConfigs.EmailLayouts;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.Services;

public class AppConfigsEmailsService(
    IEmailsFactoryService emailsFactoryService,
    IExecuteApplicationProcess executeApplicationProcess) : IAppConfigsEmailsService
{
    public async Task SendNewDotNetVersionEmailToAdminsAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var info = await executeApplicationProcess.ExecuteProcessAsync(new ApplicationStartInfo
        {
            ProcessName = "dotnet",
            Arguments = "sdk check",
            AppPath = "dotnet",
            WaitForExit = TimeSpan.FromSeconds(value: 3),
            KillProcessOnStart = false
        });

        if (IsNewVersionAvailable(info))
        {
            await emailsFactoryService.SendEmailToAllAdminsAsync<NewDotNetVersionEmail, NewDotNetVersionEmailModel>(
                messageId: "NewDotNetVersion", inReplyTo: "", references: "NewDotNetVersion",
                new NewDotNetVersionEmailModel
                {
                    Body = info
                }, emailSubject: "جهت اطلاع: نگارش جدیدی از دات‌نت برای نصب", cancellationToken);
        }
    }

    /// <summary>
    ///     https://github.com/dotnet/sdk/blob/a34f1ca17979f6cb283ad74c53ca68f8575fceb9/src/Cli/dotnet/commands/dotnet-sdk/check/LocalizableStrings.resx
    /// </summary>
    private static bool IsNewVersionAvailable(string info)
        => info.Contains(value: "is available", StringComparison.OrdinalIgnoreCase) || (!OperatingSystem.IsLinux() &&
            info.Contains(value: "newest", StringComparison.OrdinalIgnoreCase));
}

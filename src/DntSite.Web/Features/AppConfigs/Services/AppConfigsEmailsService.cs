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
                }, emailSubject: "نگارش جدیدی از دات‌نت برای نصب", cancellationToken);
        }
    }

    private static bool IsNewVersionAvailable(string info)
        => info.Contains(value: "is available", StringComparison.OrdinalIgnoreCase) ||
           info.Contains(value: "newest", StringComparison.OrdinalIgnoreCase);
}

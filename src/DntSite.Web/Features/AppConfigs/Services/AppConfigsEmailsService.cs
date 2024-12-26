using DntSite.Web.Features.AppConfigs.EmailLayouts;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.Services;

public class AppConfigsEmailsService(
    IEmailsFactoryService emailsFactoryService,
    IExecuteApplicationProcess executeApplicationProcess) : IAppConfigsEmailsService
{
    public async Task SendNewDotNetVersionEmailToAdminsAsync()
    {
        var info = await executeApplicationProcess.ExecuteProcessAsync(new ApplicationStartInfo
        {
            ProcessName = "dotnet",
            Arguments = "sdk check",
            AppPath = "dotnet",
            WaitForExit = TimeSpan.FromSeconds(value: 3),
            KillProcessOnStart = false
        });

        if (!info.Contains(value: "is available", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await emailsFactoryService.SendEmailToAllAdminsAsync<NewDotNetVersionEmail, NewDotNetVersionEmailModel>(
            messageId: "NewDotNetVersion", inReplyTo: "", references: "NewDotNetVersion", new NewDotNetVersionEmailModel
            {
                Body = info
            }, emailSubject: "نگارش جدیدی از دات‌نت برای نصب");
    }
}

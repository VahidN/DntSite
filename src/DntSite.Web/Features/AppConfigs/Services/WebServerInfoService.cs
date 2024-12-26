using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace DntSite.Web.Features.AppConfigs.Services;

public class WebServerInfoService(
    IKeyManager keyManager,
    IWebHostEnvironment webHostEnvironment,
    IExecuteApplicationProcess executeApplicationProcess) : IWebServerInfoService
{
    public async Task<WebServerInfoModel> GetWebServerInfoAsync()
        => new()
        {
            ServerInfo = await WebServerInfoProvider.GetServerInfoAsync(),
            VersionInfo = Assembly.GetExecutingAssembly().GetBuildDateTime() ?? "",
            KeysList = [..keyManager.GetAllKeys().OrderByDescending(key => key.CreationDate)],
            WebHostEnvironment = webHostEnvironment,
            DotNerInfo = await executeApplicationProcess.ExecuteProcessAsync(new ApplicationStartInfo
            {
                ProcessName = "dotnet",
                Arguments = "--info",
                AppPath = "dotnet",
                WaitForExit = TimeSpan.FromSeconds(value: 3),
                KillProcessOnStart = false
            }),
            SdkCheckInfo = await executeApplicationProcess.ExecuteProcessAsync(new ApplicationStartInfo
            {
                ProcessName = "dotnet",
                Arguments = "sdk check",
                AppPath = "dotnet",
                WaitForExit = TimeSpan.FromSeconds(value: 3),
                KillProcessOnStart = false
            })
        };
}

using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace DntSite.Web.Features.AppConfigs.Services;

public class WebServerInfoService(
    IKeyManager keyManager,
    ICacheService cacheService,
    IWebHostEnvironment webHostEnvironment) : IWebServerInfoService
{
    public async Task<WebServerInfoModel?> GetWebServerInfoAsync()
        => await cacheService.GetOrAddAsync(nameof(GetWebServerInfoAsync), nameof(WebServerInfoService), async ()
            => new WebServerInfoModel
            {
                ServerInfo = await WebServerInfoProvider.GetServerInfoAsync(),
                VersionInfo = Assembly.GetExecutingAssembly().GetBuildDateTime() ?? "",
                KeysList = [..keyManager.GetAllKeys().OrderByDescending(key => key.CreationDate)],
                WebHostEnvironment = webHostEnvironment
            }, DateTimeOffset.UtcNow.AddMinutes(minutes: 5));
}

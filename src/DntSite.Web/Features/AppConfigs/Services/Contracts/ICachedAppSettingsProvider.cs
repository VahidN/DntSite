using DntSite.Web.Features.AppConfigs.Entities;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface ICachedAppSettingsProvider : ISingletonService
{
    public Task<AppSetting> GetAppSettingsAsync();

    public void InvalidateAppSettings();
}

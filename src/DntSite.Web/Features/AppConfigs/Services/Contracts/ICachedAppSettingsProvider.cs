using DntSite.Web.Features.AppConfigs.Entities;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface ICachedAppSettingsProvider : ISingletonService
{
    Task<AppSetting> GetAppSettingsAsync();

    void InvalidateAppSettings();
}

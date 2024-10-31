using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppSettingsService : IScopedService
{
    Task<AppSettingModel> GetAppSettingModelAsync();

    AppSetting AddAppSetting(AppSetting data);

    Task AddOrUpdateAppSettingsAsync(AppSettingModel model);

    Task<bool> IsBannedDomainAndSubDomainAsync(string url);

    Task<bool> IsBannedReferrerAsync(string? url);

    Task<bool> IsBannedSiteAsync(string url);
}

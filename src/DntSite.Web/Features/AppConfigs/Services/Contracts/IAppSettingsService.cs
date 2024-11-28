using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppSettingsService : IScopedService
{
    public Task<AppSettingModel> GetAppSettingModelAsync();

    public AppSetting AddAppSetting(AppSetting data);

    public Task AddOrUpdateAppSettingsAsync(AppSettingModel model);

    public Task<bool> IsBannedDomainAndSubDomainAsync(string url);

    public Task<bool> IsBannedReferrerAsync(string? url);

    public Task<bool> IsBannedSiteAsync(string url);
}

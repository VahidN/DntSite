using AutoMapper;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.AppConfigs.Services;

public class AppSettingsService(IUnitOfWork uow, IMapper mapper, IEmailsFactoryService emailsFactoryService)
    : IAppSettingsService
{
    private readonly DbSet<AppSetting> _blogConfigs = uow.DbSet<AppSetting>();

    private AppSetting? _appSetting;

    public async Task<bool> IsBannedReferrerAsync(string url)
    {
        ArgumentNullException.ThrowIfNull(url);

        var config = await GetAppSettingsAsync();

        if (config is null)
        {
            return false;
        }

        if (config.BannedReferrers?.Count == 0)
        {
            return false;
        }

        var bannedReferrers = config.BannedReferrers ?? new List<string>();

        return bannedReferrers.Any(pattern => url.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> IsBannedDomainAndSubDomainAsync(string url)
    {
        var config = await GetAppSettingsAsync();

        if (config is null)
        {
            return false;
        }

        if (!url.IsValidUrl())
        {
            return true;
        }

        if (config.BannedUrls?.Count == 0)
        {
            return false;
        }

        var urls = config.BannedUrls ?? new List<string>();

        foreach (var bannedUrl in urls)
        {
            if (!bannedUrl.IsValidUrl())
            {
                continue;
            }

            if (new Uri(bannedUrl).HaveTheSameDomain(new Uri(url)))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> IsBannedSiteAsync(string url)
    {
        var config = await GetAppSettingsAsync();

        if (config is null)
        {
            return false;
        }

        if (!url.IsValidUrl())
        {
            return true;
        }

        if (config.BannedSites?.Count == 0)
        {
            return false;
        }

        var urls = config.BannedSites ?? new List<string>();

        foreach (var bannedUrl in urls)
        {
            if (!bannedUrl.IsValidUrl())
            {
                continue;
            }

            if (new Uri(bannedUrl).Host.Equals(new Uri(url).Host, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<AppSetting?> GetAppSettingsAsync()
    {
        _appSetting ??= await _blogConfigs.OrderBy(x => x.Id).FirstOrDefaultAsync();

        return _appSetting;
    }

    public AppSetting AddAppSetting(AppSetting data) => _blogConfigs.Add(data).Entity;

    public async Task AddOrUpdateAppSettingsAsync(AppSettingModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var cfg = await GetAppSettingsAsync();

        if (cfg is null)
        {
            var newCfg = mapper.Map<AppSettingModel, AppSetting>(model);
            AddAppSetting(newCfg);
        }
        else
        {
            mapper.Map(model, cfg);
        }

        await uow.SaveChangesAsync();

        await emailsFactoryService.SendTextToAllAdminsAsync(text: "تنظیمات سایت با موفقیت ویرایش گردید");
    }

    public async Task<AppSettingModel> GetAppSettingModelAsync()
    {
        var cfg = await GetAppSettingsAsync() ?? new AppSetting
        {
            BlogName = "DNT",
            SiteRootUri = ""
        };

        return mapper.Map<AppSetting, AppSettingModel>(cfg);
    }
}

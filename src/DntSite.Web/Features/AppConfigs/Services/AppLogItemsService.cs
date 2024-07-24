using AutoMapper;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace DntSite.Web.Features.AppConfigs.Services;

public class AppLogItemsService(IUnitOfWork uow, IMapper mapper) : IAppLogItemsService
{
    private readonly DbSet<AppLogItem> _appLogItems = uow.DbSet<AppLogItem>();
    private readonly IConfigurationProvider _mapperConfiguration = mapper.ConfigurationProvider;

    private IQueryable<AppLogItem> BaseQuery => _appLogItems.Where(x => !x.IsDeleted);

    public Task DeleteAllAsync(LogLevel? logLevel)
    {
        if (!logLevel.HasValue)
        {
            _appLogItems.RemoveRange(BaseQuery);
        }
        else
        {
            var query = BaseQuery.Where(appLogItem => appLogItem.LogLevel == logLevel.Value.ToString());
            _appLogItems.RemoveRange(query);
        }

        return uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int logItemId)
    {
        var itemToRemove = await BaseQuery.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id.Equals(logItemId));

        if (itemToRemove is not null)
        {
            _appLogItems.Remove(itemToRemove);
            await uow.SaveChangesAsync();
        }
    }

    public Task DeleteOlderThanAsync(DateTime cutoffDateUtc, LogLevel? logLevel)
    {
        if (!logLevel.HasValue)
        {
            var query = BaseQuery.Where(appLogItem => appLogItem.AuditActions[0].CreatedAt < cutoffDateUtc);
            _appLogItems.RemoveRange(query);
        }
        else
        {
            var query = BaseQuery.Where(appLogItem
                => appLogItem.AuditActions[0].CreatedAt < cutoffDateUtc &&
                   appLogItem.LogLevel == logLevel.Value.ToString());

            _appLogItems.RemoveRange(query);
        }

        return uow.SaveChangesAsync();
    }

    public Task<int> GetCountAsync(LogLevel? logLevel)
        => !logLevel.HasValue
            ? BaseQuery.AsNoTracking().CountAsync()
            : BaseQuery.AsNoTracking().CountAsync(appLogItem => appLogItem.LogLevel == logLevel.Value.ToString());

    public Task<PagedResultModel<AppLogItemModel>> GetPagedAppLogItemsAsync(DntQueryBuilderModel state,
        LogLevel? logLevel)
    {
        var query = BaseQuery.Include(appLogItem => appLogItem.User).AsNoTracking();

        if (logLevel.HasValue)
        {
            query = query.Where(appLogItem => appLogItem.LogLevel == logLevel.Value.ToString());
        }

        return query.ApplyQueryableDntGridFilterAsync<AppLogItemModel, AppLogItem>(state, nameof(AppLogItem.Id),
            _mapperConfiguration);
    }
}

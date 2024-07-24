using DntSite.Web.Features.AppConfigs.Entities;

namespace DntSite.Web.Features.DbLogger.Services;

public class EfDbLoggerItem
{
    public AppLogItem AppLogItem { set; get; } = default!;
}

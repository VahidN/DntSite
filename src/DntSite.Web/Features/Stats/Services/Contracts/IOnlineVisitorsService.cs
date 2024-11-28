using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IOnlineVisitorsService : ISingletonService
{
    public OnlineVisitorsInfoModel GetOnlineVisitorsInfo();

    public void ProcessNewVisitor(LastSiteUrlVisitorStat item);
}

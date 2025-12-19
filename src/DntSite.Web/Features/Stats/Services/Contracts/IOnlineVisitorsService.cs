using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IOnlineVisitorsService : ISingletonService
{
    OnlineVisitorsInfoModel GetOnlineVisitorsInfo();

    void ProcessNewVisitor(LastSiteUrlVisitorStat item);
}

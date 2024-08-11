using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Components;

public partial class ShowOnlineVisitorsInfo
{
    [Inject] internal IOnlineVisitorsService OnlineVisitorsService { set; get; } = null!;
}

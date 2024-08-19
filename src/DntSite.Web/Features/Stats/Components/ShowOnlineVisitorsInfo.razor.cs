using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Components;

public partial class ShowOnlineVisitorsInfo
{
    private OnlineVisitorsInfoModel? _visitorsInfo;

    [Inject] internal IOnlineVisitorsService OnlineVisitorsService { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _visitorsInfo = OnlineVisitorsService.GetOnlineVisitorsInfo();
    }
}

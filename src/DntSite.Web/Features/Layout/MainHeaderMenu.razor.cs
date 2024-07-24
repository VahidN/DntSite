using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.Layout;

public partial class MainHeaderMenu
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private string? SiteName => ApplicationState.AppSetting?.BlogName;
}

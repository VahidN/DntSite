using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.Layout;

public partial class HeaderUserOptionsMenu
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;
}

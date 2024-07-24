using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.Common.Components;

public partial class DntSitePageTitle
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public required string PageTitle { set; get; }
}

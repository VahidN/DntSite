using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.Layout;

public partial class MainLayout
{
    private IList<BreadCrumb>? _breadCrumbs;
    private IDictionary<string, object?>? _footerMenuParameters;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _breadCrumbs = ApplicationState.BreadCrumbs;

        _footerMenuParameters = new Dictionary<string, object?>
        {
            {
                nameof(FooterMenu.SiteName), ApplicationState.AppSetting?.BlogName
            }
        };
    }
}

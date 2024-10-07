using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Common.Components;

public partial class DntSitePageTitle
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public required string PageTitle { set; get; }

    [Parameter] [EditorRequired] public required string Group { set; get; }

    [Inject] public ISitePageTitlesCacheService SitePageTitlesCacheService { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SitePageTitlesCacheService.AddSitePageTitle(ApplicationState.HttpContext.GetRawUrl(),
            $"{Group} - {PageTitle.ToPersianNumbers()}");
    }
}

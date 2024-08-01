using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.Components;

public partial class ApplicationState
{
    public AppSetting? AppSetting { set; get; }

    public CurrentUserModel? CurrentUser { set; get; }

    public IList<BreadCrumb> BreadCrumbs { set; get; } = [AppConfigsBreadCrumbs.RootBreadCrumb];

    public Uri CurrentAbsoluteUri => NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

    [CascadingParameter] public HttpContext HttpContext { set; get; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [InjectComponentScoped] internal IAppSettingsService AppSettingsService { set; get; } = null!;

    [InjectComponentScoped] internal ICurrentUserService CurrentUserService { set; get; } = null!;

    [Inject] public NavigationManager NavigationManager { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AppSetting = await AppSettingsService.GetAppSettingsAsync();
        CurrentUser = await CurrentUserService.GetCurrentUserAsync();
    }

    public void NavigateToUnauthorizedPage() => NavigateTo(uri: "/error/401");

    public void NavigateToNotFoundPage() => NavigateTo(uri: "/error/404");

    public void NavigateTo([StringSyntax(syntax: "Uri")] string uri, bool forceLoad = false, bool replace = false)
        => NavigationManager.NavigateTo(uri, forceLoad, replace);
}

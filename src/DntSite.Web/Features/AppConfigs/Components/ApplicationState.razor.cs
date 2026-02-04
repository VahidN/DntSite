using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.Components;

public partial class ApplicationState
{
    public AppSetting? AppSetting { set; get; }

    public CurrentUserModel? CurrentUser { set; get; }

    public bool IsCurrentUserAdmin => CurrentUser?.IsAdmin == true;

    public bool SiteIsActive => AppSetting?.SiteIsActive == true;

    public IList<BreadCrumb> BreadCrumbs { set; get; } = [AppConfigsBreadCrumbs.RootBreadCrumb];

    public Uri CurrentAbsoluteUri => NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

    public string RootUrl => AppSetting?.SiteRootUri ?? CurrentAbsoluteUri.ToString();

    public bool DoNotLogPageReferrer { set; get; }

    [CascadingParameter] public HttpContext HttpContext { set; get; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] internal ICachedAppSettingsProvider AppSettingsService { set; get; } = null!;

    [InjectComponentScoped] internal ICurrentUserService CurrentUserService { set; get; } = null!;

    [Inject] public NavigationManager NavigationManager { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AppSetting = await AppSettingsService.GetAppSettingsAsync();
        CurrentUser = await CurrentUserService.GetCurrentUserAsync();

        CheckSiteIsActive();
    }

    public void NavigateToUnauthorizedPage() => NavigateTo(uri: "/error/401");

    public void NavigateToTemporarilyUnavailablePage() => NavigateTo(uri: "/error/503");

    /// <summary>
    ///     Sends user to `/error/404` address
    /// </summary>
    public void NavigateToNotFoundPage() => NavigateTo(uri: "/error/404");

    /// <summary>
    ///     Handles setting the NotFound state. It's new in .NET 10x.
    /// </summary>
    public void NavigateToNotFound() => NavigationManager.NotFound();

    public void NavigateTo([StringSyntax(syntax: "Uri")] string uri, bool forceLoad = false, bool replace = false)
        => NavigationManager.NavigateTo(uri, forceLoad, replace);

    public bool IsCurrentPage(string route)
    {
        ArgumentNullException.ThrowIfNull(route);

        var relative = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        var path = relative.Split(separator: '?')[0];

        return path.StartsWith(route.TrimStart(trimChar: '/'), StringComparison.OrdinalIgnoreCase);
    }

    private void CheckSiteIsActive()
    {
        if (IsCurrentUserAdmin || IsCurrentPage(UserProfilesRoutingConstants.Login))
        {
            return;
        }

        if (!SiteIsActive)
        {
            NavigateToTemporarilyUnavailablePage();
        }
    }
}

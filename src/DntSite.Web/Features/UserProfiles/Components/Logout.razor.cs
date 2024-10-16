using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class Logout
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        ApplicationState.DoNotLogPageReferrer = true;

        await ApplicationState.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        ApplicationState.HttpContext.SsrRedirectTo("/");
    }
}

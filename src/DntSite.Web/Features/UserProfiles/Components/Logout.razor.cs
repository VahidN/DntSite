using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class Logout
{
    [CascadingParameter] private HttpContext HttpContext { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.SsrRedirectTo("/");
    }
}

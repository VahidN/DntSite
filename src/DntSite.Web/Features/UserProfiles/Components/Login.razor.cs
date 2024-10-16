using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class Login
{
    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [Inject] internal IOptionsSnapshot<StartupSettingsModel> SiteSettings { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [InjectComponentScoped] internal IUserRolesService UserRolesService { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UserProfilesManagerService { set; get; } = null!;

    [InjectComponentScoped] internal ICurrentUserService CurrentUserService { set; get; } = null!;

    [Inject] internal IAppFoldersService AppFoldersService { set; get; } = null!;

    [SupplyParameterFromForm] public AccountModel Model { get; set; } = new();

    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        ApplicationState.DoNotLogPageReferrer = true;
        await CurrentUserService.ClearExistingAuthenticationCookiesAsync(clearAdminCookies: true);
    }

    private async Task PerformLoginAsync()
    {
        if (Model is null)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!",
                message: "اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.");

            return;
        }

        var user = await UsersService.FindUserAsync(Model.Username, Model.Password);

        if (user is null)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!",
                "نام کاربری یا کلمه‌ی عبور وارد شده صحیح نیست. " +
                "اگر از کاربران قدیمی سایت هستید و برای اولین بار است که به برنامه‌ی جدید مراجعه می‌کنید، لطفا از گزینه‌ی فراموشی کلمه‌ی عبور استفاده نمائید.");

            return;
        }

        if (!user.EmailIsValidated)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!",
                "لطفا به صندوق پستی خود مراجعه نموده (بالک و اسپم را هم بررسی کنید) و آدرس ایمیل خود را تائید و فعال نمائید. " +
                "اگر ایمیل فعال سازی را دریافت نکرده‌اید، لطفا اندکی صبر نمائید تا توسط مدیریت سایت ارسال شود.");

            return;
        }

        if (!user.IsActive)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", message: "اکانت شما غیرفعال شده‌است.");

            return;
        }

        var loginCookieExpirationDays = SiteSettings.Value.DataProtectionOptions.LoginCookieExpirationDays;
        var cookieClaims = await UserRolesService.CreateCookieClaimsAsync(user);

        await ApplicationState.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cookieClaims,
            new AuthenticationProperties
            {
                IsPersistent = Model.RememberMe,
                IssuedUtc = DateTimeOffset.UtcNow,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(loginCookieExpirationDays),
                RedirectUri = ApplicationState.NavigationManager.GetSafeRedirectUri(ReturnUrl)
            });

        await UserProfilesManagerService.UpdateUserLastActivityDateAsync(user.Id);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([UserProfilesBreadCrumbs.Users, UserProfilesBreadCrumbs.Login]);
}

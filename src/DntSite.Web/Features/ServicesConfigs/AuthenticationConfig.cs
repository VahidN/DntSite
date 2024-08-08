using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace DntSite.Web.Features.ServicesConfigs;

public static class AuthenticationConfig
{
    public static IServiceCollection AddCustomizedAuthentication(this IServiceCollection services,
        StartupSettingsModel siteSettings,
        IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(siteSettings);

        services.AddCascadingAuthenticationState();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(CustomRoles.Admin, policy => policy.RequireRole(CustomRoles.Admin));
            options.AddPolicy(CustomRoles.User, policy => policy.RequireRole(CustomRoles.User));
        });

        services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = UserProfilesRoutingConstants.Login;
                options.LogoutPath = UserProfilesRoutingConstants.Logout;
                options.AccessDeniedPath = "/error/403";
                options.Cookie.Name = ".dnt.site.cookie";
                options.Cookie.HttpOnly = true;

                options.Cookie.SecurePolicy = environment.IsDevelopment()
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.Always;

                // A cookie with "SameSite=Lax" will be sent with a same-site request,
                // or a cross-site top-level navigation with a "safe" HTTP method.
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.SlidingExpiration = false;

                options.ExpireTimeSpan =
                    TimeSpan.FromDays(siteSettings.DataProtectionOptions.LoginCookieExpirationDays);

                options.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = context =>
                    {
                        var cookieValidatorService =
                            context.HttpContext.RequestServices.GetRequiredService<ICookieValidatorService>();

                        return cookieValidatorService.ValidateAsync(context);
                    }
                };
            });

        services.Configure<AntiforgeryOptions>(opts => { opts.Cookie.SameSite = SameSiteMode.Lax; });

        return services;
    }
}

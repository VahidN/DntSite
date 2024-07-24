using System.Security.Claims;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace DntSite.Web.Features.UserProfiles.Services;

public class IdentityRevalidatingAuthenticationStateProvider(
    ILoggerFactory loggerFactory,
    IServiceScopeFactory scopeFactory) : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    private readonly IServiceScopeFactory _scopeFactory =
        scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

    protected override TimeSpan RevalidationInterval { get; } = TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        // Get the user from a new scope to ensure it fetches fresh data
        var scope = _scopeFactory.CreateScope();

        try
        {
            var cookieValidatorService = scope.ServiceProvider.GetRequiredService<ICookieValidatorService>();

            return await cookieValidatorService.IsValidateUserAsync(
                authenticationState?.User.Identity as ClaimsIdentity);
        }
        finally
        {
            if (scope is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else
            {
                scope.Dispose();
            }
        }
    }
}

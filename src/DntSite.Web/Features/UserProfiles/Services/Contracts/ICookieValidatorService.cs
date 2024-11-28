using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface ICookieValidatorService : IScopedService
{
    public Task ValidateAsync(CookieValidatePrincipalContext context);

    public Task<bool> IsValidateUserAsync(ClaimsIdentity? claimsIdentity);
}

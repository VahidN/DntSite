using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface ICookieValidatorService : IScopedService
{
    Task ValidateAsync(CookieValidatePrincipalContext context);

    Task<bool> IsValidateUserAsync(ClaimsIdentity? claimsIdentity);
}

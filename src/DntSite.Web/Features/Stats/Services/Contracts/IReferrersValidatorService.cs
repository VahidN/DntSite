namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IReferrersValidatorService : ISingletonService
{
    Task<bool> ShouldSkipThisRequestAsync(HttpContext context, string referrerUrl, string destinationUrl);
}

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IReferrersValidatorService : ISingletonService
{
    Task<bool> ShouldSkipThisRequestAsync(string referrerUrl,
        string destinationUrl,
        string baseUrl,
        bool isSpider,
        bool isProtectedRoute);

    Task<string?> GetNormalizedUrlAsync(string? url);
}

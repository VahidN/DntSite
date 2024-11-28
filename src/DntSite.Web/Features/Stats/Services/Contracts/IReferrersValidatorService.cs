namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IReferrersValidatorService : ISingletonService
{
    public Task<bool> ShouldSkipThisRequestAsync([NotNullWhen(returnValue: false)] string? referrerUrl,
        [NotNullWhen(returnValue: false)] string? destinationUrl,
        string baseUrl,
        bool isProtectedRoute);

    public Task<string?> GetNormalizedUrlAsync([NotNullIfNotNull(nameof(url))] string? url);
}

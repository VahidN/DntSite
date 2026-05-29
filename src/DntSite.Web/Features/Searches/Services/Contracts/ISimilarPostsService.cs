namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface ISimilarPostsService : ISingletonService
{
    Task<string> GetSimilarPostsHtmlBodyAsync(string? documentTypeIdHash, int maxItems = 11);
}

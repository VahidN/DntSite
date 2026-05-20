namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface ISimilarPostsService : ISingletonService
{
    string GetSimilarPostsHtmlBody(string? documentTypeIdHash, int maxItems = 11);
}

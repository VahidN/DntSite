namespace DntSite.Web.Features.Searches.RoutingConstants;

public static class SearchesRoutingConstants
{
    public const string SearchedItems = "/searched-items";
    public const string SearchedItemsPageCurrentPage = $"{SearchedItems}/page/{{CurrentPage:int}}";

    public const string SearchResultsBase = "/search-results";
    public const string SearchResults = $"{SearchResultsBase}/{{Term}}";
    public const string SearchResultsTermPageCurrentPage = $"{SearchResults}/page/{{CurrentPage:int}}";

    public const string MoreLikeThisBase = "/more-like-this";
    public const string MoreLikeThis = $"{MoreLikeThisBase}/{{DocumentTypeIdHash}}";
    public const string MoreLikeThisDocumentTypeIdHashPageCurrentPage = $"{MoreLikeThis}/page/{{CurrentPage:int}}";
}

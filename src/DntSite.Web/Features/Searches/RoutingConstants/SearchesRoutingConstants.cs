namespace DntSite.Web.Features.Searches.RoutingConstants;

public static class SearchesRoutingConstants
{
    public const string SearchedItems = "/searched-items";

    public const string SearchedItemsPageCurrentPage = $"{SearchedItems}/page/{{CurrentPage:int}}";
}

namespace DntSite.Web.Features.Common.RoutingConstants;

public static class CommonRoutingConstants
{
    public const string Error = "/error";
    public const string ErrorResponseCode = "/error/{responseCode:int?}";

    public const string CatchAllPhpRequests = "/{data}.php";
    public const string CatchWordPressRequests1 = "/{SubDomain}/wp-includes/{Folder}";
    public const string CatchWordPressRequests2 = "/wp-includes/{Folder}";
    public const string CatchAllPhpRequestsPath = "/not-found.php";
}

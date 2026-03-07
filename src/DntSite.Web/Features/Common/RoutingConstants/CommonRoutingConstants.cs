namespace DntSite.Web.Features.Common.RoutingConstants;

public static class CommonRoutingConstants
{
    public const string Error = "/error";
    public const string ErrorResponseCode = "/error/{responseCode:int?}";

    public const string UnauthorizedPage = "/error/401";
    public const string TemporarilyUnavailablePage = "/error/503";
    public const string NotFoundPage = "/error/404";
}

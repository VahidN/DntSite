namespace DntSite.Web.Features.AppConfigs.RoutingConstants;

public static class AppConfigsRoutingConstants
{
    public const string DatabaseInfo = "/database-info";
    public const string ServerInfo = "/server-info";
    public const string SiteConfig = "/site-config";
    public const string SystemLogs = "/system-logs";
    public const string SystemLogsCurrentLogLevel = "/system-logs/{CurrentLogLevel}";

    public const string SystemLogsCurrentLogLevelPageCurrentPage =
        "/system-logs/{CurrentLogLevel}/page/{CurrentPage:int?}";

    public const string SystemLogsPageCurrentPage = "/system-logs/page/{CurrentPage:int?}";
}

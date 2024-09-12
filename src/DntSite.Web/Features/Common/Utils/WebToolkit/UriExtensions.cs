using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace DntSite.Web.Features.Common.Utils.WebToolkit;

public static partial class UriExtensions
{
    public const string FromFeedKey = "utm_source";

    public static bool IsFromFeed(this NavigationManager navigationManager)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);

        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

        return QueryHelpers.ParseQuery(uri.Query).TryGetValue(FromFeedKey, out _);
    }

    public static bool IsNoneAspNetCoreRequest(this HttpContext? httpContext)
        => httpContext is null || AllNoneAspNetCorePagesRegex().IsMatch(httpContext.GetCurrentUrl());

    [GeneratedRegex(
        pattern:
        @".*\.aspx|asax|asp|ashx|asmx|axd|master|svc|php|ph|sphp|cfm|ps|stm|htaccess|htpasswd|phtml|cgi|pl|py|rb|sh|jsp|cshtml|vbhtml|swf|xap|asptxt|xamlx(/.*)?",
        RegexOptions.Compiled | RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 3000)]
    private static partial Regex AllNoneAspNetCorePagesRegex();
}

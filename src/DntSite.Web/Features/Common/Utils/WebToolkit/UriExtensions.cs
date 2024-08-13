using Microsoft.AspNetCore.WebUtilities;
using UAParser;

namespace DntSite.Web.Features.Common.Utils.WebToolkit;

public static class UriExtensions
{
    public const string FromFeedKey = "utm_source";

    public static bool IsFromFeed(this NavigationManager navigationManager)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);

        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

        return QueryHelpers.ParseQuery(uri.Query).TryGetValue(FromFeedKey, out _);
    }
}

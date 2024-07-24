using Microsoft.AspNetCore.WebUtilities;

namespace DntSite.Web.Features.Common.Utils.WebToolkit;

public static class UriExtensions
{
    public const string FromFeedKey = "updated";

    public static bool IsFromFeed(this NavigationManager navigationManager)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);

        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

        return QueryHelpers.ParseQuery(uri.Query).TryGetValue(FromFeedKey, out _);
    }
}

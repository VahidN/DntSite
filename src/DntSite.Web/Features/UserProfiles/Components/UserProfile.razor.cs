using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class UserProfile
{
    [Parameter] [EditorRequired] public User? User { set; get; }

    private bool HasSocialNetworkItem => !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.LinkedInProfileId) ||
                                         !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.TwitterName) ||
                                         !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.FacebookName) ||
                                         !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.TelegramId) ||
                                         !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.GithubId) ||
                                         !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.NugetId) ||
                                         !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.CodeProjectId) ||
                                         !string.IsNullOrWhiteSpace(User?.UserSocialNetwork?.SourceforgeId);

    /// <summary>
    ///     Its default value is `1`.
    /// </summary>
    [Parameter]
    public int MarginTop { get; set; } = 1;

    private static string GetEscapedUrl(string href, string? queryStringValue)
        => string.IsNullOrWhiteSpace(queryStringValue)
            ? href
            : href.CombineUrl($"/{Uri.EscapeDataString(queryStringValue)}");

    private static string GetEscapedUrl(string href, int? queryStringValue)
        => !queryStringValue.HasValue
            ? href
            : href.CombineUrl(
                $"/{Uri.EscapeDataString(queryStringValue.Value.ToString(CultureInfo.InvariantCulture))}");

    private string? GetUserHomePage()
    {
        var homePage = User?.HomePageUrl;

        if (!string.IsNullOrWhiteSpace(homePage) &&
            !homePage.StartsWith(value: "http", StringComparison.OrdinalIgnoreCase))
        {
            homePage = $"http://{homePage}";
        }

        return homePage;
    }
}

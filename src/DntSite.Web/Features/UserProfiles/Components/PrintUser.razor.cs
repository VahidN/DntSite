using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class PrintUser
{
    private string? _homeUrl;
    private string? _imageUrl;
    private bool _isExternal;
    private string? _name;

    [Parameter] public string DefaultImageUrl { set; get; } = "/images/not_known.png";

    [Parameter] [EditorRequired] public required User User { set; get; }

    /// <summary>
    ///     Its default value is `true`
    /// </summary>
    [Parameter]
    public bool ShowAsUrl { set; get; } = true;

    /// <summary>
    ///     Its default value is `true`
    /// </summary>
    [Parameter]
    public bool IsNavLink { set; get; } = true;

    private string CssClass => IsNavLink ? "nav-link" : "post-tag rounded";

    protected override void OnParametersSet() => (_name, _imageUrl, _homeUrl, _isExternal) = GetUserInfo();

    private (string Name, string ImageUrl, string HomeUrl, bool isExternal) GetUserInfo()
    {
        var userImageUrl = GetUserImageUrl();

        if (User is null)
        {
            return (SharedConstants.GuestUserName, userImageUrl, "", true);
        }

        if (!string.IsNullOrWhiteSpace(User.FriendlyName))
        {
            return (User.FriendlyName, userImageUrl,
                $"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(User.FriendlyName)}", false);
        }

        if (!string.IsNullOrWhiteSpace(User.GuestUser.UserName))
        {
            return !string.IsNullOrWhiteSpace(User.GuestUser.HomeUrl)
                ? (User.GuestUser.UserName, userImageUrl, User.GuestUser.HomeUrl, true)
                : (User.GuestUser.UserName, userImageUrl, "", true);
        }

        return (SharedConstants.GuestUserName, userImageUrl, "", true);
    }

    private string GetUserImageUrl(int size = 23, GravatarRating rating = GravatarRating.PG)
    {
        if (User is null)
        {
            return DefaultImageUrl;
        }

        if (!string.IsNullOrWhiteSpace(User.Photo))
        {
            return $"{ApiUrlsRoutingConstants.File.HttpAny.Avatar}?name={Uri.EscapeDataString(User.Photo)}";
        }

        if (!string.IsNullOrWhiteSpace(User.EMail))
        {
            return User.EMail.CalculateGravatar(size, rating) ?? DefaultImageUrl;
        }

        if (!string.IsNullOrWhiteSpace(User.GuestUser.Email))
        {
            return User.GuestUser.Email.CalculateGravatar(size, rating) ?? DefaultImageUrl;
        }

        return DefaultImageUrl;
    }
}

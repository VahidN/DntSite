using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.UserProfiles.Models;

public class CurrentUserModel
{
    public User? User { set; get; }

    public int? UserId { set; get; }

    public bool IsAuthenticated { set; get; }

    public bool IsAdmin { set; get; }

    public string FriendlyName => User?.FriendlyName ?? User?.GuestUser.UserName ?? SharedConstants.GuestUserName;

    public string ProfileUrl => $"{UserProfilesRoutingConstants.Users}/{Uri.EscapeDataString(FriendlyName)}";
}

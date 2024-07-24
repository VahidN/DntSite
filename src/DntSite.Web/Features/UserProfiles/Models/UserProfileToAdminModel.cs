using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class UserProfileToAdminModel : BaseEmailModel
{
    public required string FriendlyName { get; set; }

    public required string UserId { get; set; }

    public required string Username { get; set; }

    public required string UserEmail { get; set; }

    public required string HomePageUrl { get; set; }

    public required string Photo { get; set; }

    public required string Description { get; set; }
}

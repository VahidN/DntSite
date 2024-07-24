using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class FileUploadEmailModel : BaseEmailModel
{
    public required string ActionUrl { get; set; }

    public required string FriendlyName { get; set; }
}

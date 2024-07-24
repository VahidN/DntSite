using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.PrivateMessages.Models;

public class ContactUsEmailModel : BaseEmailModel
{
    public required string FriendlyName { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }

    public required string PmId { get; set; }
}

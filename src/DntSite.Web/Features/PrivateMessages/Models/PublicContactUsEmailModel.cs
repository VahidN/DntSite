using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.PrivateMessages.Models;

public class PublicContactUsEmailModel : BaseEmailModel
{
    public required string FromUserName { get; set; }

    public required string FromUserNameEmail { get; set; }

    public required string Title { get; set; }

    public required string DescriptionText { get; set; }
}

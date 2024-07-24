using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Advertisements.Models;

public class AdvertisementReplyToPersonEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string ReplyToComment { get; set; }

    public required string Username { get; set; }

    public required string Body { get; set; }

    public required string AdvertisementId { get; set; }

    public required string CommentId { get; set; }
}

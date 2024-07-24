using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Surveys.Models;

public class VoteReplyToAdminsEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Username { get; set; }

    public required string Body { get; set; }

    public required string Stat { get; set; }

    public required string VoteId { get; set; }

    public required string CommentId { get; set; }
}

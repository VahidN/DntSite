using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class TopicReplyToAdminsEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Username { get; set; }

    public required string Body { get; set; }

    public required string Stat { get; set; }

    public required string CId { get; set; }

    public required string PmId { get; set; }

    public required string CommentId { get; set; }
}

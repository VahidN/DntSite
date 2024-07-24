using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class CourseQuestionReplyToPersonEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string ReplyToComment { get; set; }

    public required string Username { get; set; }

    public required string Body { get; set; }

    public required string CourseId { get; set; }

    public required string PmId { get; set; }

    public required string CommentId { get; set; }
}

using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class NewQuestionSendEmailToWritersModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Username { get; set; }

    public required string Body { get; set; }

    public required string CourseId { get; set; }

    public required string PmId { get; set; }
}

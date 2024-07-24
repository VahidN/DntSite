using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.StackExchangeQuestions.Models;

public class QuestionsToAdminsEmailsModel : BaseEmailModel
{
    public required string FriendlyName { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }

    public required string Stat { get; set; }
}

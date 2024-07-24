using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.PrivateMessages.Models;

public class DailyNewsletterEmailModel : BaseEmailModel
{
    public required string Body { get; set; }

    public required string BaseUrl { get; set; }
}

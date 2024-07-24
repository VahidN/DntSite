using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Surveys.Models;

public class VoteEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Options { get; set; }

    public required string PmId { get; set; }
}

using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.AppConfigs.Models;

public class HasNotRemainingSpaceEmailModel : BaseEmailModel
{
    public required string Body { get; set; }
}

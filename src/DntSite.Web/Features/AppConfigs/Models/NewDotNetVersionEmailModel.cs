using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.AppConfigs.Models;

public class NewDotNetVersionEmailModel : BaseEmailModel
{
    public required string Body { get; set; }
}

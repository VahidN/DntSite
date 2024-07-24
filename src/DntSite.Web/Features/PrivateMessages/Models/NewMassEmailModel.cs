using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.PrivateMessages.Models;

public class NewMassEmailModel : BaseEmailModel
{
    public required string Body { get; set; }
}

using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class ForgottenPasswordConfirmEmailModel : BaseEmailModel
{
    public required string QueryString { get; set; }
}

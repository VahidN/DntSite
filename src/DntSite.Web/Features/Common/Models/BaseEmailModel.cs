namespace DntSite.Web.Features.Common.Models;

public abstract class BaseEmailModel
{
    public string SiteTitle { set; get; } = null!;

    public string EmailSig { set; get; } = null!;

    public string MsgDateTime { set; get; } = null!;

    public string SiteRootUri { get; set; } = null!;
}

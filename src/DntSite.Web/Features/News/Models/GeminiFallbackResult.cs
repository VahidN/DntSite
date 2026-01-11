namespace DntSite.Web.Features.News.Models;

public class GeminiFallbackResult : GeminiApiResult
{
    public GeminiFallbackReason? Reason { get; set; }
}

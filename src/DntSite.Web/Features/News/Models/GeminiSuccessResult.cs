namespace DntSite.Web.Features.News.Models;

public class GeminiSuccessResult : GeminiApiResult
{
    public string? Title { get; set; }

    public string? Summary { get; set; }

    public List<string>? Tags { get; set; }
}

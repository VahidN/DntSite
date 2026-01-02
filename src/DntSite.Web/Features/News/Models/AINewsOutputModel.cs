namespace DntSite.Web.Features.News.Models;

public class AINewsOutputModel
{
    [JsonPropertyName(name: "status")] public string? Status { set; get; }

    [JsonPropertyName(name: "reason")] public string? Reason { set; get; }

    [JsonPropertyName(name: "title")] public string? Title { set; get; }

    [JsonPropertyName(name: "summary")] public string? Summary { set; get; }

    [JsonPropertyName(name: "tags")] public IList<string>? Tags { set; get; }
}

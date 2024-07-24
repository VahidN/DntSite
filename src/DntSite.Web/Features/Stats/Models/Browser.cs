namespace DntSite.Web.Features.Stats.Models;

public class Browser
{
    public required string Name { set; get; }

    public int InUseCount { set; get; }

    public int InUsePercent { set; get; }
}

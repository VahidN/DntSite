namespace DntSite.Web.Features.Common.Utils.Pagings.Models;

public class GridifyMap<T>
    where T : class
{
    public required string From { set; get; }

    public required Expression<Func<T, object?>> To { set; get; }

    public Func<string, object>? Convertor { set; get; }

    public bool OverrideIfExists { set; get; } = true;
}

using UAParser;

namespace DntSite.Web.Features.Stats.Entities;

[ComplexType]
public class LastSiteUrlVisitorStat : IEqualityComparer<LastSiteUrlVisitorStat>
{
    public DateTime VisitTime { set; get; } = default!;

    [StringLength(maximumLength: 100)] public string Ip { set; get; } = default!;

    [StringLength(maximumLength: 1000)] public string UserAgent { set; get; } = default!;

    [StringLength(maximumLength: 1000)] public string? DisplayName { set; get; }

    public bool IsSpider { set; get; }

    [NotMapped] public ClientInfo? ClientInfo { set; get; }

    public bool Equals(LastSiteUrlVisitorStat? x, LastSiteUrlVisitorStat? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return string.Equals(x.Ip, y.Ip, StringComparison.Ordinal) &&
               string.Equals(x.UserAgent, y.UserAgent, StringComparison.Ordinal) &&
               string.Equals(x.DisplayName, y.DisplayName, StringComparison.Ordinal);
    }

    public int GetHashCode(LastSiteUrlVisitorStat obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return HashCode.Combine(obj.Ip, obj.UserAgent, obj.DisplayName);
    }
}

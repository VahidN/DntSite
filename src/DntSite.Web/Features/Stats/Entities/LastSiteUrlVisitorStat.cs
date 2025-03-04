using System.Collections;
using UAParser;

namespace DntSite.Web.Features.Stats.Entities;

[ComplexType]
public class LastSiteUrlVisitorStat : IEqualityComparer<LastSiteUrlVisitorStat>, IEqualityComparer
{
    public DateTime VisitTime { set; get; } = default!;

    [StringLength(maximumLength: 100)] public string Ip { set; get; } = default!;

    [StringLength(maximumLength: 1000)] public string UserAgent { set; get; } = default!;

    [StringLength(maximumLength: 1000)] public string? DisplayName { set; get; }

    public bool IsSpider { set; get; }

    [NotMapped] public ClientInfo? ClientInfo { set; get; }

    public new bool Equals(object? x, object? y)
    {
        if (x == y)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        if (x is LastSiteUrlVisitorStat a && y is LastSiteUrlVisitorStat b)
        {
            return Equals(a, b);
        }

        throw new ArgumentException(message: "", nameof(x));
    }

    public int GetHashCode(object? obj)
    {
        if (obj is null)
        {
            return 0;
        }

        if (obj is LastSiteUrlVisitorStat x)
        {
            return GetHashCode(x);
        }

        throw new ArgumentException(message: "", nameof(obj));
    }

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
               string.Equals(x.UserAgent, y.UserAgent, StringComparison.Ordinal);
    }

    public int GetHashCode(LastSiteUrlVisitorStat obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return HashCode.Combine(obj.Ip, obj.UserAgent);
    }
}

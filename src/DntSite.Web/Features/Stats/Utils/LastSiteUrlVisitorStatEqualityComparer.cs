using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.Utils;

public class LastSiteUrlVisitorStatEqualityComparer : IEqualityComparer<LastSiteUrlVisitorStat>
{
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

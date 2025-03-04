using System.Collections;
using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseEntity : IEqualityComparer<BaseEntity>, IEqualityComparer
{
    [IgnoreAudit] public int Id { get; set; }

    public bool IsDeleted { set; get; } // NOTE: if you don't want it, apply the [IgnoreSoftDelete] to that class

    [IgnoreAudit] public AuditBase Audit { set; get; } = new();

    public GuestUser GuestUser { set; get; } = new();

    public virtual User? User { set; get; } // It's nullable because we can have guests with no user-id

    public int? UserId { set; get; }

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

        if (x is BaseEntity a && y is BaseEntity b)
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

        if (obj is BaseEntity x)
        {
            return GetHashCode(x);
        }

        throw new ArgumentException(message: "", nameof(obj));
    }

    public bool Equals(BaseEntity? x, BaseEntity? y)
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

        return x.Id == y.Id;
    }

    public int GetHashCode(BaseEntity obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return obj.Id ^ 1000;
    }
}

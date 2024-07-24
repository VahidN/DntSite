using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[IgnoreSoftDelete]
public abstract class ParentReactionEntity : BaseEntity
{
    public ReactionType Reaction { set; get; }

    [ForeignKey(nameof(ForUserId))] public virtual User? ForUser { set; get; }

    public int? ForUserId { set; get; }
}

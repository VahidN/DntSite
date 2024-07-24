using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Stats.Models;

public class ReactionModel
{
    public ReactionType Reaction { set; get; }

    public int FormId { set; get; }
}

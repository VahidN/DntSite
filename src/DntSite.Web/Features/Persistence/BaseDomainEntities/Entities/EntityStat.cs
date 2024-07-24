namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class EntityStat
{
    public int NumberOfViews { set; get; }

    public int NumberOfViewsFromFeed { set; get; }

    public int NumberOfComments { set; get; }

    public int NumberOfTags { set; get; }

    public int NumberOfReactions { set; get; }

    public int NumberOfBookmarks { set; get; }
}

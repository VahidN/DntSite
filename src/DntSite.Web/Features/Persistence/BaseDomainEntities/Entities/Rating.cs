namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class Rating
{
    public long TotalRating { get; set; }

    public int TotalRaters { get; set; }

    public decimal AverageRating { get; set; }
}

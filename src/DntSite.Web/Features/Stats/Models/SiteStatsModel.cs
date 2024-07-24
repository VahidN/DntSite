namespace DntSite.Web.Features.Stats.Models;

public class SiteStatsModel
{
    public required SiteStatsDate SiteStatsDate { set; get; }

    public required int VisitorsCount { set; get; }

    public required int UniqueVisitorsCount { get; set; }

    public required int VisitsCount { set; get; }
}

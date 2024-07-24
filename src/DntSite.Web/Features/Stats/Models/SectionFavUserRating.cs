using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Models;

public class SectionFavUserRating
{
    public double TotalRatingValue { get; set; }

    public int FkId { get; set; }

    public string? Title { get; set; }

    public DateTime? DateTime { get; set; }

    public User? ForUser { set; get; }
}

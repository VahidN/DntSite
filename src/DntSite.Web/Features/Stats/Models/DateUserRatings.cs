using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Models;

public class DateUserRatings
{
    public double TotalRatingValue { get; set; }

    public User? ForUser { set; get; }
}

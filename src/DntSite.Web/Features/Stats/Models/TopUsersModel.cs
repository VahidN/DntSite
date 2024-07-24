using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Models;

public class TopUsersModel
{
    public IList<DateUserRatings>? TopUserRatings { set; get; }

    public IList<User>? TopWiters { set; get; }

    public IList<DateUserRatings>? TopUserRatingsThisWeak { set; get; }
}

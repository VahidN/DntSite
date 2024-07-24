namespace DntSite.Web.Features.Stats.Models;

public class ShowReactionsModel
{
    public bool AreReactionsDisabled { set; get; }

    public bool IsCurrentUserReacted { set; get; }

    public int ThumbsUpUsersCount { set; get; }

    public int ThumbsDownUsersCount { set; get; }
}

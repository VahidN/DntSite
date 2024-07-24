using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Models;

public class ShowReactionsUsersListModel
{
    public ShowReactionsModel ReactionsInfo { set; get; } = new();

    public IList<User?> ThumbsDownUsers { set; get; } = new List<User?>();

    public IList<User?> ThumbsUpUsers { set; get; } = new List<User?>();

    /// <summary>
    ///     This will allows you to convert this object to a tuple.
    /// </summary>
    public void Deconstruct(out IList<User?> thumbsDownUsers,
        out IList<User?> thumbsUpUsers,
        out ShowReactionsModel reactionsInfo)
    {
        thumbsDownUsers = ThumbsDownUsers;
        thumbsUpUsers = ThumbsUpUsers;
        reactionsInfo = ReactionsInfo;
    }
}

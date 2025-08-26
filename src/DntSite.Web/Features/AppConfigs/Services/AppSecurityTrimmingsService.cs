using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.AppConfigs.Services;

public static class AppSecurityTrimmingsService
{
    public static bool CanCurrentUserCreateALearningPath(this ApplicationState? applicationState)
    {
        var currentUser = applicationState?.CurrentUser;
        var requiredPosts = applicationState?.AppSetting?.MinimumRequiredPosts;
        var minNumberOfPosts = requiredPosts?.MinPostsToCreateALearningPath ?? 1;
        var minNumberOfLinks = requiredPosts?.MinNumberOfLinksToCreateALearningPath ?? 1;

        return currentUser is not null && (currentUser.IsAdmin ||
                                           currentUser.User?.UserStat.NumberOfPosts >= minNumberOfPosts ||
                                           currentUser.User?.UserStat.NumberOfLinks >= minNumberOfLinks);
    }

    public static bool CanCurrentUserCreateANewBacklog(this ApplicationState? applicationState)
    {
        var currentUser = applicationState?.CurrentUser;
        var requiredPosts = applicationState?.AppSetting?.MinimumRequiredPosts;
        var minNumberOfLinks = requiredPosts?.MinNumberOfLinksToCreateANewBacklog ?? 2;

        return currentUser?.User?.UserStat.NumberOfLinks >= minNumberOfLinks;
    }

    public static bool CanCurrentUserCreateANewSurvey(this ApplicationState? applicationState)
    {
        var currentUser = applicationState?.CurrentUser;
        var requiredPosts = applicationState?.AppSetting?.MinimumRequiredPosts;
        var minNumberOfLinks = requiredPosts?.MinNumberOfLinksCreateANewSurvey ?? 2;

        return currentUser?.User?.UserStat.NumberOfLinks >= minNumberOfLinks;
    }

    public static bool CanCurrentUserCreateANewCourse(this ApplicationState? applicationState)
    {
        var currentUser = applicationState?.CurrentUser;
        var requiredPosts = applicationState?.AppSetting?.MinimumRequiredPosts;
        var minNumberOfPosts = requiredPosts?.MinPostsToCreateANewCourse ?? 2;

        return currentUser?.User?.UserStat.NumberOfPosts >= minNumberOfPosts;
    }

    public static bool CanCurrentUserCreateANewProject(this ApplicationState? applicationState)
    {
        var currentUser = applicationState?.CurrentUser;
        var requiredPosts = applicationState?.AppSetting?.MinimumRequiredPosts;
        var minNumberOfPosts = requiredPosts?.MinPostsToCreateANewProject ?? 1;

        return currentUser?.User?.UserStat.NumberOfPosts >= minNumberOfPosts;
    }

    public static bool
        CanUserViewThisPost(this CurrentUserModel? userModel, [NotNullWhen(returnValue: true)] BlogPost? post)
        => post is not null && (post.NumberOfRequiredPoints is null or 0 || (userModel?.UserId is not null &&
                                                                             userModel.User is not null &&
                                                                             userModel.IsAuthenticated &&
                                                                             (userModel.IsAdmin ||
                                                                              IsTheSameAuthor(userModel, post.UserId) ||
                                                                              (userModel.User.IsActive &&
                                                                               userModel.User.UserStat.NumberOfPosts >=
                                                                               post.NumberOfRequiredPoints.Value))));

    public static bool CanUserViewThisPost(this CurrentUserModel? userModel,
        [NotNullWhen(returnValue: true)] DailyNewsItem? post)
        => post is not null && userModel?.UserId is not null && userModel.User is not null &&
           userModel.IsAuthenticated && (userModel.IsAdmin || IsTheSameAuthor(userModel, post.UserId) ||
                                         userModel.User.IsActive);

    public static bool CanCurrentUserEditThisItem(this ApplicationState? applicationState,
        int? itemUserId,
        DateTime? createdAt = null)
    {
        var user = applicationState?.CurrentUser;

        if (user?.UserId is null || user.User is null || !user.IsAuthenticated)
        {
            return false;
        }

        if (user.IsAdmin)
        {
            return true;
        }

        if (!IsTheSameAuthor(user, itemUserId))
        {
            return false;
        }

        return !applicationState.IsPostClosed(createdAt);
    }

    public static bool CanCurrentUserPostAComment(this ApplicationState? applicationState,
        CommentAction model,
        int? replyUserId,
        DateTime? createdAt)
    {
        var user = applicationState?.CurrentUser;

        if (user is null)
        {
            return false;
        }

        switch (model)
        {
            case CommentAction.SubmitNewComment:
            case CommentAction.ReplyToComment:
            case CommentAction.ReplyToPost:
                if (!user.IsAuthenticated)
                {
                    return false;
                }

                break;
            case CommentAction.SubmitEditedComment:
            case CommentAction.Edit:
                if (!CanCurrentUserEditThisItem(applicationState, replyUserId, createdAt))
                {
                    return false;
                }

                break;
            case CommentAction.Delete:
                if (!user.IsAdmin)
                {
                    return false;
                }

                break;
            default:
                return false;
        }

        return true;
    }

    private static bool IsTheSameAuthor(CurrentUserModel user, int? postUserId)
        => user.UserId is not null && postUserId is not null && postUserId.Value == user.UserId.Value;

    private static bool IsPostClosed(this ApplicationState? applicationState, DateTime? createdAt)
    {
        if (!createdAt.HasValue)
        {
            return false;
        }

        var daysToClose = applicationState?.AppSetting?.MinimumRequiredPosts.MaxDaysToCloseATopic ?? 7;

        return DateTime.UtcNow.AddDays(-daysToClose) > createdAt.Value;
    }

    public static bool CanUserEditThisDraft(this CurrentUserModel? userModel,
        [NotNullWhen(returnValue: true)] BlogPostDraft? post)
        => post is not null && userModel?.UserId is not null && userModel.User is not null &&
           userModel.IsAuthenticated && userModel.User.IsActive && !post.IsConverted &&
           (userModel.IsAdmin || IsTheSameAuthor(userModel, post.UserId));
}

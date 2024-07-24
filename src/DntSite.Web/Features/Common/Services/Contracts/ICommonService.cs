using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Common.Services.Contracts;

public interface ICommonService : IScopedService // to avoid bidirectional/circular dependencies
{
    Task<List<Role>> FindListOfActualRolesAsync(IList<string> roles);

    Task<List<User>> NotValidatedEmailsUsersAsync(DateTime? from);

    Task<AppSetting?> GetBlogConfigAsync();

    Task<List<User>> GetAllActiveAdminsAsNoTrackingAsync();

    Task<List<User>> AllValidatedEmailsUsersAsync();

    Task<User?> FindUserAsync(string friendlyName);

    ValueTask<User?> FindUserAsync(int? userId);

    Task<List<BlogPostTag>> FindListOfActualArticleTagsAsync(IList<string> tagsList);

    ValueTask<BlogPost?> FindCommentPostAsync(int blogPostId);

    ValueTask<BlogPostComment?> FindCommentAsync(int replyId);

    Task<List<DailyNewsItemTag>> FindListOfActualLinkItemTagsAsync(IList<string> tagsList);

    Task<List<User>> GetAllActiveGroupUsersAsNoTrackingAsync(string groupName);

    Task<List<User>> GetAllActiveReaderUsersAsNoTrackingAsync();

    Task<List<ProjectTag>> FindListOfActualProjectTagsAsync(IList<string> tagsList);

    ValueTask<DailyNewsItem?> FindNewsCommentPostAsync(int postId);

    ValueTask<StackExchangeQuestion?> FindStackExchangeQuestionCommentPostAsync(int postId);

    ValueTask<DailyNewsItemComment?> FindNewsCommentAsync(int replyId);

    ValueTask<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentAsync(int replyId);

    ValueTask<Project?> FindProjectAsync(int projectId);

    ValueTask<ProjectIssueComment?> FindIssueCommentAsync(int replyId);

    //Task<List<FavoritePageTag>> FindListOfActualFavoritePageTagsAsync(IList<string> tagsList);
    Task<List<SurveyTag>> FindListOfActualVoteTagsAsync(IList<string> tagsList);

    ValueTask<SurveyComment?> FindVoteCommentAsync(int id);

    ValueTask<AdvertisementComment?> FindAdvertisementCommentAsync(int id);

    Task<List<CourseTag>> FindListOfActualCourseTagsAsync(IList<string> tagsList);

    ValueTask<CourseTopicComment?> FindTopicCommentAsync(int id);

    ValueTask<Course?> FindCourseAsync(int id);

    ValueTask<CourseQuestionComment?> FindCourseQuestionCommentAsync(int id);
}

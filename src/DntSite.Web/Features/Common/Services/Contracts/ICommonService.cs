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
    public Task<List<Role>> FindListOfActualRolesAsync(IList<string> roles);

    public Task<List<User>> NotValidatedEmailsUsersAsync(DateTime? from);

    public Task<AppSetting?> GetBlogConfigAsync();

    public Task<List<User>> GetAllActiveAdminsAsNoTrackingAsync();

    public Task<List<User>> AllValidatedEmailsUsersAsync();

    public Task<User?> FindUserAsync(string friendlyName);

    public ValueTask<User?> FindUserAsync(int? userId);

    public Task<List<BlogPostTag>> FindListOfActualArticleTagsAsync(IList<string> tagsList);

    public ValueTask<BlogPost?> FindCommentPostAsync(int blogPostId);

    public ValueTask<BlogPostComment?> FindCommentAsync(int replyId);

    public Task<List<DailyNewsItemTag>> FindListOfActualLinkItemTagsAsync(IList<string> tagsList);

    public Task<List<User>> GetAllActiveGroupUsersAsNoTrackingAsync(string groupName);

    public Task<List<User>> GetAllActiveReaderUsersAsNoTrackingAsync();

    public Task<List<ProjectTag>> FindListOfActualProjectTagsAsync(IList<string> tagsList);

    public ValueTask<DailyNewsItem?> FindNewsCommentPostAsync(int postId);

    public ValueTask<StackExchangeQuestion?> FindStackExchangeQuestionCommentPostAsync(int postId);

    public ValueTask<DailyNewsItemComment?> FindNewsCommentAsync(int replyId);

    public ValueTask<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentAsync(int replyId);

    public ValueTask<Project?> FindProjectAsync(int projectId);

    public ValueTask<ProjectIssueComment?> FindIssueCommentAsync(int replyId);

    //Task<List<FavoritePageTag>> FindListOfActualFavoritePageTagsAsync(IList<string> tagsList);
    public Task<List<SurveyTag>> FindListOfActualVoteTagsAsync(IList<string> tagsList);

    public ValueTask<SurveyComment?> FindVoteCommentAsync(int id);

    public ValueTask<AdvertisementComment?> FindAdvertisementCommentAsync(int id);

    public Task<List<CourseTag>> FindListOfActualCourseTagsAsync(IList<string> tagsList);

    public ValueTask<CourseTopicComment?> FindTopicCommentAsync(int id);

    public ValueTask<Course?> FindCourseAsync(int id);

    public ValueTask<CourseQuestionComment?> FindCourseQuestionCommentAsync(int id);
}

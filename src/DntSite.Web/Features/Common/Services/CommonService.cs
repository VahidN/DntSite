using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Common.Services;

public class CommonService(IUnitOfWork uow) : ICommonService
{
    private readonly DbSet<AdvertisementComment> _advertisementComment = uow.DbSet<AdvertisementComment>();
    private readonly DbSet<BlogPostComment> _blogComments = uow.DbSet<BlogPostComment>();
    private readonly DbSet<AppSetting> _blogConfigs = uow.DbSet<AppSetting>();
    private readonly DbSet<BlogPost> _blogPosts = uow.DbSet<BlogPost>();
    private readonly DbSet<CourseQuestionComment> _courseQuestionComment = uow.DbSet<CourseQuestionComment>();
    private readonly DbSet<Course> _courses = uow.DbSet<Course>();
    private readonly DbSet<CourseTag> _courseTags = uow.DbSet<CourseTag>();
    private readonly DbSet<CourseTopicComment> _courseTopicComments = uow.DbSet<CourseTopicComment>();
    private readonly DbSet<DailyNewsItemComment> _dailyNewsItemComments = uow.DbSet<DailyNewsItemComment>();
    private readonly DbSet<DailyNewsItem> _dailyNewsItems = uow.DbSet<DailyNewsItem>();
    private readonly DbSet<DailyNewsItemTag> _dailyNewsItemTag = uow.DbSet<DailyNewsItemTag>();
    private readonly DbSet<ProjectIssueComment> _projectIssueComments = uow.DbSet<ProjectIssueComment>();
    private readonly DbSet<Project> _projects = uow.DbSet<Project>();
    private readonly DbSet<ProjectTag> _projectTags = uow.DbSet<ProjectTag>();
    private readonly DbSet<Role> _roles = uow.DbSet<Role>();

    private readonly DbSet<StackExchangeQuestionComment> _stackExchangeQuestionComments =
        uow.DbSet<StackExchangeQuestionComment>();

    private readonly DbSet<StackExchangeQuestion> _stackExchangeQuestions = uow.DbSet<StackExchangeQuestion>();
    private readonly DbSet<BlogPostTag> _tags = uow.DbSet<BlogPostTag>();
    private readonly DbSet<User> _users = uow.DbSet<User>();
    private readonly DbSet<SurveyComment> _voteComments = uow.DbSet<SurveyComment>();
    private readonly DbSet<SurveyTag> _voteTags = uow.DbSet<SurveyTag>();

    public ValueTask<BlogPost?> FindCommentPostAsync(int blogPostId) => _blogPosts.FindAsync(blogPostId);

    public Task<List<Role>> FindListOfActualRolesAsync(IList<string> roles)
        => _roles.Where(x => roles.Contains(x.Name)).ToListAsync();

    public Task<List<User>> NotValidatedEmailsUsersAsync(DateTime? from)
        => !from.HasValue
            ? _users.AsNoTracking().Where(user => user.IsActive && !user.EmailIsValidated).ToListAsync()
            : _users.AsNoTracking()
                .Where(user => user.IsActive && !user.EmailIsValidated && user.Audit.CreatedAt >= from.Value)
                .ToListAsync();

    public Task<List<User>> AllValidatedEmailsUsersAsync()
        => _users.AsNoTracking().Where(x => x.IsActive && x.EmailIsValidated).ToListAsync();

    public Task<AppSetting?> GetBlogConfigAsync() => _blogConfigs.OrderBy(x => x.Id).FirstOrDefaultAsync();

    public Task<List<User>> GetAllActiveAdminsAsNoTrackingAsync()
        => (from u in _users.AsNoTracking()
            where u.IsActive
            from r in u.Roles
            where r.Name == CustomRoles.Admin
            select u).ToListAsync();

    public Task<List<User>> GetAllActiveGroupUsersAsNoTrackingAsync(string groupName)
        => (from u in _users.AsNoTracking()
            where u.IsActive
            from r in u.Roles
            where r.Name == groupName
            select u).ToListAsync();

    public Task<List<User>> GetAllActiveReaderUsersAsNoTrackingAsync()
        => (from u in _users.AsNoTracking()
            where u.IsActive
            from r in u.Roles
            where r.Name != CustomRoles.Admin && r.Name != CustomRoles.Writer
            select u).ToListAsync();

    public Task<User?> FindUserAsync(string friendlyName)
        => _users.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.FriendlyName == friendlyName);

    public ValueTask<User?> FindUserAsync(int? userId)
        => userId is null ? ValueTask.FromResult<User?>(result: null) : _users.FindAsync(userId.Value);

    public Task<List<BlogPostTag>> FindListOfActualArticleTagsAsync(IList<string> tagsList)
    {
        tagsList = tagsList.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select<string, string>(x => x.GetCleanedTag()!)
            .ToArray();

        return _tags.Where(x => tagsList.Contains(x.Name)).ToListAsync();
    }

    public Task<List<DailyNewsItemTag>> FindListOfActualLinkItemTagsAsync(IList<string> tagsList)
    {
        tagsList = tagsList.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select<string, string>(x => x.GetCleanedTag()!)
            .ToArray();

        return _dailyNewsItemTag.Where(x => tagsList.Contains(x.Name)).ToListAsync();
    }

    public Task<List<ProjectTag>> FindListOfActualProjectTagsAsync(IList<string> tagsList)
    {
        tagsList = tagsList.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select<string, string>(x => x.GetCleanedTag()!)
            .ToArray();

        return _projectTags.Where(x => tagsList.Contains(x.Name)).ToListAsync();
    }

    public ValueTask<DailyNewsItem?> FindNewsCommentPostAsync(int postId) => _dailyNewsItems.FindAsync(postId);

    public ValueTask<StackExchangeQuestion?> FindStackExchangeQuestionCommentPostAsync(int postId)
        => _stackExchangeQuestions.FindAsync(postId);

    public ValueTask<DailyNewsItemComment?> FindNewsCommentAsync(int replyId)
        => _dailyNewsItemComments.FindAsync(replyId);

    public ValueTask<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentAsync(int replyId)
        => _stackExchangeQuestionComments.FindAsync(replyId);

    public ValueTask<Project?> FindProjectAsync(int projectId) => _projects.FindAsync(projectId);

    public ValueTask<ProjectIssueComment?> FindIssueCommentAsync(int replyId)
        => _projectIssueComments.FindAsync(replyId);

    public ValueTask<AdvertisementComment?> FindAdvertisementCommentAsync(int id)
        => _advertisementComment.FindAsync(id);

    public Task<List<CourseTag>> FindListOfActualCourseTagsAsync(IList<string> tagsList)
    {
        tagsList = tagsList.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select<string, string>(x => x.GetCleanedTag()!)
            .ToArray();

        return _courseTags.AsNoTracking().Where(x => tagsList.Contains(x.Name)).ToListAsync();
    }

    public ValueTask<CourseTopicComment?> FindTopicCommentAsync(int id) => _courseTopicComments.FindAsync(id);

    public ValueTask<Course?> FindCourseAsync(int id) => _courses.FindAsync(id);

    public ValueTask<CourseQuestionComment?> FindCourseQuestionCommentAsync(int id)
        => _courseQuestionComment.FindAsync(id);

    public Task<List<SurveyTag>> FindListOfActualVoteTagsAsync(IList<string> tagsList)
    {
        tagsList = tagsList.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select<string, string>(x => x.GetCleanedTag()!)
            .ToArray();

        return _voteTags.AsNoTracking().Where(x => tagsList.Contains(x.Name)).ToListAsync();
    }

    public ValueTask<SurveyComment?> FindVoteCommentAsync(int id) => _voteComments.FindAsync(id);

    public ValueTask<BlogPostComment?> FindCommentAsync(int replyId) => _blogComments.FindAsync(replyId);
}

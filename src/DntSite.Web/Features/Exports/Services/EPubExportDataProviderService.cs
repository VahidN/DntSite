using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using EFCoreSecondLevelCacheInterceptor;

namespace DntSite.Web.Features.Exports.Services;

public class EPubExportDataProviderService(IUnitOfWork uow) : IEPubExportDataProviderService
{
    private readonly DbSet<BlogPost> _blogPosts = uow.DbSet<BlogPost>();
    private readonly DbSet<BlogPostTag> _blogPostsTags = uow.DbSet<BlogPostTag>();
    private readonly DbSet<Course> _courses = uow.DbSet<Course>();
    private readonly DbSet<DailyNewsItem> _dailyNews = uow.DbSet<DailyNewsItem>();
    private readonly DbSet<LearningPath> _learningPaths = uow.DbSet<LearningPath>();
    private readonly DbSet<StackExchangeQuestion> _stackExchangeQuestions = uow.DbSet<StackExchangeQuestion>();
    private readonly DbSet<User> _users = uow.DbSet<User>();

    public async Task<EPubTocItems> GetEPubTocItemsAsync(CancellationToken cancellationToken)
        => new(await GetArticlesQuery().CountAsync(cancellationToken),
            await GetAuthorsQuery().CountAsync(cancellationToken),
            await GetArticleGroupsQuery().CountAsync(cancellationToken),
            await GetLearningPathsQuery().CountAsync(cancellationToken),
            await GetCoursesQuery().CountAsync(cancellationToken), await GetNewsQuery().CountAsync(cancellationToken),
            await GetExchangeQuestionsQuery().CountAsync(cancellationToken));

    public Task<PagedResultModel<EPubListItem>> GetArticlesAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken)
        => GetArticlesQuery().ApplyQueryablePagingAsync(pageNumber, recordsPerPage, cancellationToken);

    public Task<List<EPubListItem>> GetAllArticlesAsync(CancellationToken cancellationToken)
        => GetArticlesQuery().ToListAsync(cancellationToken);

    public Task<PagedResultModel<EPubListItem>> GetAuthorsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken)
        => GetAuthorsQuery().ApplyQueryablePagingAsync(pageNumber, recordsPerPage, cancellationToken);

    public Task<PagedResultModel<EPubListItem>> GetArticleGroupsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken)
        => GetArticleGroupsQuery().ApplyQueryablePagingAsync(pageNumber, recordsPerPage, cancellationToken);

    public Task<PagedResultModel<EPubListItem>> GetLearningPathsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken)
        => GetLearningPathsQuery().ApplyQueryablePagingAsync(pageNumber, recordsPerPage, cancellationToken);

    public Task<PagedResultModel<EPubListItem>> GetNewsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken)
        => GetNewsQuery().ApplyQueryablePagingAsync(pageNumber, recordsPerPage, cancellationToken);

    public Task<List<EPubListItem>> GetAllNewsAsync(CancellationToken cancellationToken)
        => GetNewsQuery().ToListAsync(cancellationToken);

    public Task<PagedResultModel<EPubListItem>> GetCoursesAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken)
        => GetCoursesQuery().ApplyQueryablePagingAsync(pageNumber, recordsPerPage, cancellationToken);

    public Task<List<EPubListItem>> GetAllCoursesAsync(CancellationToken cancellationToken)
        => GetCoursesQuery().ToListAsync(cancellationToken);

    public Task<PagedResultModel<EPubListItem>> GetExchangeQuestionsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken)
        => GetExchangeQuestionsQuery().ApplyQueryablePagingAsync(pageNumber, recordsPerPage, cancellationToken);

    public Task<List<EPubListItem>> GetAllExchangeQuestionsAsync(CancellationToken cancellationToken)
        => GetExchangeQuestionsQuery().ToListAsync(cancellationToken);

    private IQueryable<EPubListItem> GetAuthorsQuery()
        => _users.NotCacheable()
            .AsNoTracking()
            .Include(user => user.BlogPosts)
            .Where(user => !user.IsDeleted && user.IsActive && user.BlogPosts.Any(blogPost => !blogPost.IsDeleted))
            .OrderBy(user => user.Id)
            .Select(user => new
            {
                Author = user,
                AuthorPosts = user.BlogPosts
            })
            .Select(ap => new EPubListItem(
                new EPubContentItem(ap.Author.Id, ap.Author.FriendlyName, Content: null, DisplayId: null, null, null,
                    null,
                    null),
                SubItems: ap.AuthorPosts.Select(userBlogPost => new EPubContentItem(userBlogPost.Id, userBlogPost.Title,
                        Content: null, DisplayId: null, null, userBlogPost.Audit.CreatedAt, null, null))
                    .ToList()));

    private IQueryable<EPubListItem> GetArticleGroupsQuery()
        => _blogPostsTags.NotCacheable()
            .AsNoTracking()
            .Include(postTag => postTag.AssociatedEntities)
            .ThenInclude(tagBlogPost => tagBlogPost.User)
            .Where(postTag => !postTag.IsDeleted)
            .OrderBy(postTag => postTag.Name)
            .Select(postTag => new EPubListItem(
                new EPubContentItem(postTag.Id, postTag.Name, Content: null, DisplayId: null, null, null, null, null),
                SubItems: postTag.AssociatedEntities.Select(tagBlogPost => new EPubContentItem(tagBlogPost.Id,
                        tagBlogPost.Title, Content: null, DisplayId: null, tagBlogPost.User!.FriendlyName,
                        tagBlogPost.Audit.CreatedAt, null, null))
                    .ToList()));

    private IQueryable<EPubListItem> GetLearningPathsQuery()
        => _learningPaths.NotCacheable()
            .AsNoTracking()
            .Where(path => !path.IsDeleted)
            .OrderByDescending(path => path.Id)
            .Select(path => new EPubListItem(
                new EPubContentItem(path.Id, path.Title, path.Description, null, null, null, null, null), null));

    private IQueryable<EPubListItem> GetArticlesQuery()
        => _blogPosts.NotCacheable()
            .AsNoTracking()
            .Include(blogPost => blogPost.User)
            .Where(blogPost => !blogPost.IsDeleted)
            .OrderByDescending(blogPost => blogPost.Id)
            .Select(blogPost => new EPubListItem(new EPubContentItem(blogPost.Id, blogPost.Title, null, null,
                blogPost.User!.FriendlyName, blogPost.Audit.CreatedAt, null, new PageSeoMetadata
                {
                    Description = blogPost.BriefDescription,
                    Title = blogPost.Title,
                    AuthorName = blogPost.User!.FriendlyName,
                    DatePublished = blogPost.Audit.CreatedAt,
                    ShowLlmsTxt = false
                }), null));

    private IQueryable<EPubListItem> GetNewsQuery()
        => _dailyNews.NotCacheable()
            .AsNoTracking()
            .Include(newsItem => newsItem.User)
            .Where(newsItem => !newsItem.IsDeleted)
            .OrderByDescending(newsItem => newsItem.Id)
            .Select(newsItem => new EPubListItem(new EPubContentItem(newsItem.Id, newsItem.Title, null, null,
                newsItem.User!.FriendlyName, newsItem.Audit.CreatedAt, newsItem.Url, new PageSeoMetadata
                {
                    Description = newsItem.BriefDescription,
                    Title = newsItem.Title,
                    AuthorName = newsItem.User!.FriendlyName,
                    DatePublished = newsItem.Audit.CreatedAt,
                    ShowLlmsTxt = false
                }), null));

    private IQueryable<EPubListItem> GetCoursesQuery()
        => _courses.NotCacheable()
            .AsNoTracking()
            .Include(course => course.CourseTopics)
            .ThenInclude(courseTopic => courseTopic.User)
            .Where(course => !course.IsDeleted)
            .OrderByDescending(course => course.Id)
            .Select(course => new EPubListItem(
                new EPubContentItem(course.Id, course.Title, Content: null, DisplayId: null, null, null, null, null),
                SubItems: course.CourseTopics.Select(courseTopic => new EPubContentItem(courseTopic.Id,
                        courseTopic.Title,
                        Content: null, DisplayId: courseTopic.DisplayId, courseTopic.User!.FriendlyName,
                        courseTopic.Audit.CreatedAt, null, new PageSeoMetadata
                        {
                            Description = courseTopic.Body.GetBriefDescription(450),
                            Title = courseTopic.Title,
                            AuthorName = courseTopic.User!.FriendlyName,
                            DatePublished = courseTopic.Audit.CreatedAt,
                            ShowLlmsTxt = false
                        }))
                    .ToList()));

    private IQueryable<EPubListItem> GetExchangeQuestionsQuery()
        => _stackExchangeQuestions.NotCacheable()
            .AsNoTracking()
            .Include(question => question.User)
            .Where(question => !question.IsDeleted)
            .OrderByDescending(question => question.Id)
            .Select(question => new EPubListItem(new EPubContentItem(question.Id, question.Title, null, null,
                question.User!.FriendlyName, question.Audit.CreatedAt, null, new PageSeoMetadata
                {
                    Description = question.Description.GetBriefDescription(450),
                    Title = question.Title,
                    AuthorName = question.User!.FriendlyName,
                    DatePublished = question.Audit.CreatedAt,
                    ShowLlmsTxt = false
                }), null));
}

using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.ModelsMappings;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Courses.Services;

public class CourseTopicCommentsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IStatService statService,
    IAntiXssService antiXssService,
    ICoursesEmailsService emailsService,
    IFullTextSearchService fullTextSearchService) : ICourseTopicCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<CourseTopicComment, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Parent.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<CourseTopicComment> _comments = uow.DbSet<CourseTopicComment>();

    public ValueTask<CourseTopicComment?> FindTopicCommentAsync(int commentId) => _comments.FindAsync(commentId);

    public Task<CourseTopicComment?> FindTopicCommentIncludeParentAsync(int commentId)
        => _comments.Include(x => x.Parent).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == commentId);

    public CourseTopicComment AddTopicComment(CourseTopicComment comment) => _comments.Add(comment).Entity;

    public async Task<List<CourseTopicComment>> GetRootCommentsOfTopicsAsync(int postId,
        int count = 1000,
        bool onlyActive = true)
    {
        var courseTopicComments = await _comments.AsNoTracking()
            .Include(x => x.Reactions)
            .Include(x => x.User)
            .Where(x => x.ParentId == postId && x.IsDeleted != onlyActive && x.Parent.IsDeleted != onlyActive &&
                        x.Parent.Course.IsDeleted != onlyActive)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return courseTopicComments.ToSelfReferencingTree();
    }

    public Task<List<CourseTopicComment>>
        GetFlatCommentsOfTopicsAsync(int postId, int count = 1000, bool onlyActive = true)
        => _comments.AsNoTracking()
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => x.ParentId == postId && x.IsDeleted != onlyActive && x.Parent.IsDeleted != onlyActive &&
                        x.Parent.Course.IsDeleted != onlyActive)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<CourseTopicCommentReaction, CourseTopicComment>(fkId, reactionType,
            fromUserId);

    public Task<List<CourseTopicComment>>
        GetLastTopicCommentsIncludePostAndUserAsync(int count, bool onlyActives = true)
        => _comments.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActives && x.Parent.IsDeleted != onlyActives &&
                        x.Parent.Course.IsDeleted != onlyActives)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Include(x => x.Parent)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _comments.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActives && x.Parent.IsDeleted != onlyActives &&
                        x.Parent.Course.IsDeleted != onlyActives)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsAsNoTrackingAsync(string userFriendlyName,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _comments.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActives && x.Parent.IsDeleted != onlyActives &&
                        x.Parent.Course.IsDeleted != onlyActives && x.User!.FriendlyName == userFriendlyName)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsOfCourseAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _comments.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActives && x.Parent.IsDeleted != onlyActives &&
                        x.Parent.Course.IsDeleted != onlyActives && x.Parent.CourseId == courseId)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task DeleteCommentAsync(int? modelFormCommentId)
    {
        if (!modelFormCommentId.HasValue)
        {
            return;
        }

        var comment = await FindTopicCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash);
        await UpdateStatAsync(comment.ParentId, comment.Parent.CourseId, comment.UserId);
        await emailsService.CourseTopicCommentSendEmailToAdminsAsync(comment);
    }

    public async Task EditReplyAsync(int? modelFormCommentId, string modelComment)
    {
        if (!modelFormCommentId.HasValue)
        {
            return;
        }

        var comment = await FindTopicCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(modelComment);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: ""));

        await emailsService.CourseTopicCommentSendEmailToAdminsAsync(comment);
        await UpdateStatAsync(comment.ParentId, comment.Parent.CourseId, comment.UserId);
    }

    public async Task AddReplyAsync(int? modelFormCommentId,
        int modelFormPostId,
        string modelComment,
        int currentUserUserId)
    {
        if (string.IsNullOrWhiteSpace(modelComment))
        {
            return;
        }

        var comment = new CourseTopicComment
        {
            ParentId = modelFormPostId,
            ReplyId = modelFormCommentId,
            Body = antiXssService.GetSanitizedHtml(modelComment),
            UserId = currentUserUserId
        };

        var result = AddTopicComment(comment);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        await NotifyNewCommentAsync(modelFormPostId, currentUserUserId, result);
    }

    public Task IndexCourseTopicCommentsAsync()
    {
        var items = _comments.AsNoTracking()
            .Where(x => !x.IsDeleted && !x.Parent.IsDeleted && !x.Parent.Course.IsDeleted)
            .Include(x => x.User)
            .Include(x => x.Parent)
            .OrderByDescending(x => x.Id)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }

    private async Task NotifyNewCommentAsync(int modelFormPostId, int currentUserUserId, CourseTopicComment result)
    {
        var comment = await FindTopicCommentIncludeParentAsync(result.Id);
        await SendEmailsAsync(comment);
        await UpdateStatAsync(modelFormPostId, comment?.Parent.CourseId, currentUserUserId);
    }

    private async Task UpdateStatAsync(int postId, int? courseId, int? userId)
    {
        await statService.RecalculateThisCourseTopicCommentsCountsAsync(postId);

        if (courseId.HasValue)
        {
            await statService.UpdateCourseNumberOfTopicsCommentsAsync(courseId.Value);
        }

        if (userId.HasValue)
        {
            await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(userId.Value);
        }
    }

    private async Task SendEmailsAsync(CourseTopicComment? result)
    {
        if (result is null)
        {
            return;
        }

        await emailsService.CourseTopicCommentSendEmailToAdminsAsync(result);
        await emailsService.CourseTopicCommentSendEmailToWritersAsync(result);
        await emailsService.CourseTopicCommentSendEmailToPersonAsync(result);
    }
}

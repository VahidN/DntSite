using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Courses.Services;

public class CourseQuestionCommentsService(IUnitOfWork uow, IUserRatingsService userRatingsService)
    : ICourseQuestionCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<CourseQuestionComment, object?>>> CustomOrders =
        new()
        {
            [PagerSortBy.Date] = x => x.Id,
            [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
            [PagerSortBy.Title] = x => x.Parent.Title,
            [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
            [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
            [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
        };

    private readonly DbSet<CourseQuestionComment> _courseQuestionComments = uow.DbSet<CourseQuestionComment>();

    public async Task<List<CourseQuestionComment>> GetRootCommentsOfQuestionsAsync(int postId,
        int count = 1000,
        bool onlyActives = true)
    {
        var courseQuestionComments = await _courseQuestionComments.AsNoTracking()
            .Include(x => x.Reactions)
            .Include(x => x.User)
            .Where(x => x.ParentId == postId && x.IsDeleted != onlyActives)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return courseQuestionComments.ToSelfReferencingTree();
    }

    public CourseQuestionComment AddCourseQuestionComment(CourseQuestionComment comment)
        => _courseQuestionComments.Add(comment).Entity;

    public ValueTask<CourseQuestionComment?> FindCourseQuestionCommentAsync(int commentId)
        => _courseQuestionComments.FindAsync(commentId);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<CourseQuestionCommentReaction, CourseQuestionComment>(fkId, reactionType,
            fromUserId);

    public Task<PagedResultModel<CourseQuestionComment>> GetLastPagedCommentsAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        var query = _courseQuestionComments.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActives && x.Parent.CourseId == courseId)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Where(x => x.Parent.IsDeleted != onlyActives);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }
}

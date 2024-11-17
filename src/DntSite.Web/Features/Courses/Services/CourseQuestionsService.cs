using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Courses.Services;

public class CourseQuestionsService(IUnitOfWork uow, IUserRatingsService userRatingsService) : ICourseQuestionsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<CourseQuestion, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<CourseQuestion> _courseQuestions = uow.DbSet<CourseQuestion>();

    public CourseQuestion AddCourseQuestion(CourseQuestion courseQuestion)
        => _courseQuestions.Add(courseQuestion).Entity;

    public ValueTask<CourseQuestion?> FindCourseQuestionAsync(int courseQuestionId)
        => _courseQuestions.FindAsync(courseQuestionId);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<CourseQuestionReaction, CourseQuestion>(fkId, reactionType, fromUserId);

    public async Task<CourseQuestionDetailsModel> GetCourseQuestionLastAndNextPostIncludeAuthorTagsAsync(int courseId,
            int questionId,
            bool onlyShowActives = true)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentItem =
                await _courseQuestions.AsNoTracking()
                    .Where(x => x.IsDeleted != onlyShowActives && x.CourseId == courseId && x.Id == questionId)
                    .Include(x => x.Course)
                    .Include(x => x.User)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem = await _courseQuestions.AsNoTracking()
                .Where(x => x.IsDeleted != onlyShowActives && x.CourseId == courseId && x.Id > questionId)
                .OrderBy(x => x.Id)
                .Include(x => x.Course)
                .Include(x => x.User)
                .FirstOrDefaultAsync(),
            PreviousItem = await _courseQuestions.AsNoTracking()
                .Where(x => x.IsDeleted != onlyShowActives && x.CourseId == courseId && x.Id < questionId)
                .OrderByDescending(x => x.Id)
                .Include(x => x.Course)
                .Include(x => x.User)
                .FirstOrDefaultAsync(),
            CommentsList = []
        };

    public async Task UpdateNumberOfViewsAsync(int id, bool fromFeed)
    {
        var post = await FindCourseQuestionAsync(id);

        if (post is null)
        {
            return;
        }

        post.EntityStat.NumberOfViews++;

        if (fromFeed)
        {
            post.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public Task<PagedResultModel<CourseQuestion>> GetLastPagedCourseQuestionsAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _courseQuestions.Where(x => x.IsDeleted != onlyActive && x.CourseId == courseId)
            .Include(x => x.Course)
            .Include(x => x.User)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }
}

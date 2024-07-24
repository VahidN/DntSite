using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Projects.Services;

public class ProjectIssueCommentsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IAntiXssService antiXssService,
    IProjectsEmailsService projectsEmailsService,
    IStatService statService) : IProjectIssueCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<ProjectIssueComment, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Parent.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<ProjectIssueComment> _projectIssueComments = uow.DbSet<ProjectIssueComment>();

    public async Task<List<ProjectIssueComment>> GetRootCommentsOfIssuesAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false)
    {
        var projectIssueComments = await _projectIssueComments.AsNoTracking()
            .Include(x => x.Reactions)
            .Include(x => x.User)
            .Where(x => x.ParentId == postId && x.IsDeleted == showDeletedItems)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return projectIssueComments.ToSelfReferencingTree();
    }

    public ValueTask<ProjectIssueComment?> FindIssueCommentAsync(int commentId)
        => _projectIssueComments.FindAsync(commentId);

    public ProjectIssueComment AddIssueComment(ProjectIssueComment comment)
        => _projectIssueComments.Add(comment).Entity;

    public Task<PagedResultModel<ProjectIssueComment>> GetLastPagedIssueCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectIssueComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<List<ProjectIssueComment>>
        GetLastIssueCommentsIncludeBlogPostAndUserAsync(int count, bool showDeletedItems = false)
        => _projectIssueComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<List<ProjectIssueComment>> GetLastProjectIssueCommentsIncludeBlogPostAndUserAsync(int projectId,
        int count,
        bool showDeletedItems = false)
        => _projectIssueComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => x.Parent.ProjectId == projectId && !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<PagedResultModel<ProjectIssueComment>> GetPagedLastIssueCommentsIncludeBlogPostAndUserAsync(
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectIssueComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectIssueComment>> GetPagedLastProjectIssueCommentsIncludeBlogPostAndUserAsync(
        int projectId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectIssueComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => x.Parent.ProjectId == projectId && !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectIssueComment>> GetLastPagedProjectIssuesCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectIssueComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<ProjectIssueCommentReaction, ProjectIssueComment>(fkId, reactionType,
            fromUserId);

    public Task<ProjectIssueComment?> FindIssueCommentIncludeParentAsync(int commentId)
        => _projectIssueComments.Include(x => x.Parent).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == commentId);

    public async Task DeleteCommentAsync(int? modelFormCommentId)
    {
        if (!modelFormCommentId.HasValue)
        {
            return;
        }

        var comment = await FindIssueCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        await uow.SaveChangesAsync();

        await UpdateStatAsync(comment.ParentId, comment.Parent.ProjectId, comment.UserId);
        await projectsEmailsService.ProjectIssueCommentSendEmailToAdminsAsync(comment);
    }

    public async Task EditReplyAsync(int? modelFormCommentId, string modelComment)
    {
        if (!modelFormCommentId.HasValue)
        {
            return;
        }

        var comment = await FindIssueCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(modelComment);
        await uow.SaveChangesAsync();

        await projectsEmailsService.ProjectIssueCommentSendEmailToAdminsAsync(comment);
        await UpdateStatAsync(comment.ParentId, comment.Parent.ProjectId, comment.UserId);
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

        var comment = new ProjectIssueComment
        {
            ParentId = modelFormPostId,
            ReplyId = modelFormCommentId,
            Body = antiXssService.GetSanitizedHtml(modelComment),
            UserId = currentUserUserId
        };

        var result = AddIssueComment(comment);
        await uow.SaveChangesAsync();

        await NotifyNewCommentAsync(modelFormPostId, currentUserUserId, result);
    }

    private async Task NotifyNewCommentAsync(int modelFormPostId, int currentUserUserId, ProjectIssueComment result)
    {
        var comment = await FindIssueCommentIncludeParentAsync(result.Id);
        await SendEmailsAsync(comment);
        await UpdateStatAsync(modelFormPostId, comment?.Parent.ProjectId, currentUserUserId);
    }

    private async Task UpdateStatAsync(int postId, int? projectId, int? userId)
    {
        await statService.RecalculateThisProjectIssueCommentsCountsAsync(postId);

        if (projectId.HasValue)
        {
            await statService.UpdateProjectNumberOfIssuesCommentsAsync(projectId.Value);
        }

        if (userId.HasValue)
        {
            await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(userId.Value);
        }
    }

    private async Task SendEmailsAsync(ProjectIssueComment? result)
    {
        if (result is null)
        {
            return;
        }

        await projectsEmailsService.ProjectIssueCommentSendEmailToAdminsAsync(result);
        await projectsEmailsService.ProjectIssueCommentSendEmailToWritersAsync(result);
        await projectsEmailsService.ProjectIssueCommentSendEmailToPersonAsync(result);
    }
}

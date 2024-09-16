using AutoMapper;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.ModelsMappings;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services;

public class ProjectIssuesService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IProjectsEmailsService emailsService,
    IEmailsFactoryService emailsFactoryService,
    IStatService statService,
    IMapper mapper,
    IFullTextSearchService fullTextSearchService) : IProjectIssuesService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<ProjectIssue, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<ProjectIssue> _projectIssue = uow.DbSet<ProjectIssue>();

    public ValueTask<ProjectIssue?> FindProjectIssueAsync(int id) => _projectIssue.FindAsync(id);

    public Task<ProjectIssue?> GetProjectIssueAsync(int id, bool showDeletedItems = false)
        => _projectIssue.Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public ProjectIssue AddProjectIssue(ProjectIssue data) => _projectIssue.Add(data).Entity;

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<ProjectIssueReaction, ProjectIssue>(fkId, reactionType, fromUserId);

    public async Task<ProjectIssueDetailsModel> GetProjectIssueLastAndNextPostIncludeAuthorTagsAsync(int projectId,
            int issueId,
            bool showDeletedItems = false)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentItem =
                await _projectIssue.AsNoTracking()
                    .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id == issueId)
                    .Include(x => x.Project)
                    .Include(x => x.User)
                    .Include(x => x.IssuePriority)
                    .Include(x => x.IssueStatus)
                    .Include(x => x.IssueType)
                    .Include(x => x.Reactions)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem =
                await _projectIssue.AsNoTracking()
                    .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id > issueId)
                    .OrderBy(x => x.Id)
                    .Include(x => x.Project)
                    .Include(x => x.User)
                    .Include(x => x.IssuePriority)
                    .Include(x => x.IssueStatus)
                    .Include(x => x.IssueType)
                    .Include(x => x.Reactions)
                    .FirstOrDefaultAsync(),
            PreviousItem = await _projectIssue.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id < issueId)
                .OrderByDescending(x => x.Id)
                .Include(x => x.Project)
                .Include(x => x.User)
                .Include(x => x.IssuePriority)
                .Include(x => x.IssueStatus)
                .Include(x => x.IssueType)
                .Include(x => x.Reactions)
                .FirstOrDefaultAsync(),
            CommentsList = new List<ProjectIssueComment>()
        };

    public async Task UpdateNumberOfViewsAsync(int issueId, bool fromFeed)
    {
        var data = await FindProjectIssueAsync(issueId);

        if (data is null)
        {
            return;
        }

        data.EntityStat.NumberOfViews++;

        if (fromFeed)
        {
            data.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public Task<int> GetAllProjectIssuesCountAsync(bool showDeletedItems = false)
        => _projectIssue.AsNoTracking().CountAsync(x => x.IsDeleted == showDeletedItems);

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedAllProjectsIssuesAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectIssue.Where(projectIssue => projectIssue.IsDeleted == showDeletedItems)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(x => x.IssuePriority)
            .Include(x => x.IssueStatus)
            .Include(x => x.IssueType)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingAsync(int projectId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectIssue.Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(x => x.IssuePriority)
            .Include(x => x.IssueStatus)
            .Include(x => x.IssueType)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssuePriorityIdAsync(
        int projectId,
        int? issuePriorityId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        if (issuePriorityId.HasValue && issuePriorityId.Value == 0)
        {
            issuePriorityId = null;
        }

        var query = _projectIssue
            .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId &&
                        x.IssuePriorityId == issuePriorityId)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(x => x.IssuePriority)
            .Include(x => x.IssueStatus)
            .Include(x => x.IssueType)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssueStatusIdAsync(int projectId,
        int? issueStatusId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        if (issueStatusId.HasValue && issueStatusId.Value == 0)
        {
            issueStatusId = null;
        }

        var query = _projectIssue
            .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.IssueStatusId == issueStatusId)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(x => x.IssuePriority)
            .Include(x => x.IssueStatus)
            .Include(x => x.IssueType)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssueTypeIdAsync(int projectId,
        int? issueTypeId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        if (issueTypeId.HasValue && issueTypeId.Value == 0)
        {
            issueTypeId = null;
        }

        var query = _projectIssue
            .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.IssueTypeId == issueTypeId)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(x => x.IssuePriority)
            .Include(x => x.IssueStatus)
            .Include(x => x.IssueType)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectIssue.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task MarkAsDeletedAsync(ProjectIssue? projectIssue)
    {
        if (projectIssue is null)
        {
            return;
        }

        projectIssue.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(projectIssue.MapToProjectsIssuesWhatsNewItemModel(siteRootUri: "")
            .DocumentTypeIdHash);
    }

    public async Task NotifyDeleteChangesAsync(ProjectIssue? projectIssue, User? currentUserUser)
    {
        if (projectIssue is null)
        {
            return;
        }

        await emailsFactoryService.SendTextToAllAdminsAsync($"بازخورد پروژه حذف شد؛ عنوان: {projectIssue.Title}");

        await UpdateStatAsync(projectIssue);
    }

    public async Task UpdateProjectIssueAsync(ProjectIssue? projectIssue, IssueModel issueModel)
    {
        ArgumentNullException.ThrowIfNull(issueModel);

        if (projectIssue is null)
        {
            return;
        }

        mapper.Map(issueModel, projectIssue);

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(
            projectIssue.MapToProjectsIssuesWhatsNewItemModel(siteRootUri: ""));
    }

    public async Task<ProjectIssue?> AddProjectIssueAsync(IssueModel issueModel, User? user, int projectId)
    {
        ArgumentNullException.ThrowIfNull(issueModel);

        var projectIssue = mapper.Map<IssueModel, ProjectIssue>(issueModel);
        projectIssue.UserId = user?.Id;
        projectIssue.ProjectId = projectId;
        var result = AddProjectIssue(projectIssue);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToProjectsIssuesWhatsNewItemModel(siteRootUri: ""));

        return result;
    }

    public async Task NotifyAddOrUpdateChangesAsync(ProjectIssue? projectIssue, IssueModel issueModel, User? user)
    {
        if (projectIssue is null)
        {
            return;
        }

        await SendIssueEmailsAsync(projectIssue);
        await UpdateStatAsync(projectIssue);
    }

    public async Task IndexProjectIssuesAsync()
    {
        var items = await _projectIssue.Where(projectIssue => !projectIssue.IsDeleted)
            .Include(x => x.Project)
            .Include(x => x.User)
            .AsNoTracking()
            .ToListAsync();

        await fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToProjectsIssuesWhatsNewItemModel(siteRootUri: "")));
    }

    private async Task SendIssueEmailsAsync(ProjectIssue projectIssue)
    {
        await emailsService.NewIssueSendEmailToAdminsAsync(projectIssue);
        await emailsService.NewIssueSendEmailToProjectWritersAsync(projectIssue);
    }

    private async Task UpdateStatAsync(ProjectIssue projectIssue)
    {
        await statService.RecalculateThisProjectIssuesCountsAsync(projectIssue.ProjectId);

        if (projectIssue.UserId.HasValue)
        {
            await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(projectIssue.UserId.Value);
        }
    }
}

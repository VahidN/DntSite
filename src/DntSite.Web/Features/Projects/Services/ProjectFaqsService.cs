using AutoMapper;
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

public class ProjectFaqsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IProjectsEmailsService emailsService,
    IStatService statService,
    IMapper mapper,
    IFullTextSearchService fullTextSearchService) : IProjectFaqsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<ProjectFaq, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<ProjectFaq> _projectFaqs = uow.DbSet<ProjectFaq>();

    public ValueTask<ProjectFaq?> FindProjectFaqAsync(int id) => _projectFaqs.FindAsync(id);

    public Task<ProjectFaq?> GetProjectFaqAsync(int id, bool showDeletedItems = false)
        => _projectFaqs.Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public ProjectFaq AddProjectFaq(ProjectFaq data) => _projectFaqs.Add(data).Entity;

    public Task<PagedResultModel<ProjectFaq>> GetLastPagedProjectFaqsAsync(int projectId,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectFaqs.Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId)
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectFaq>> GetLastPagedProjectFaqsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectFaqs.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectFaq>> GetLastPagedAllProjectsFaqsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectFaqs.Where(projectIssue => projectIssue.IsDeleted == showDeletedItems)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<List<ProjectFaq>> GetAllLastPagedProjectFaqsAsync(int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        return _projectFaqs.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<ProjectFaqReaction, ProjectFaq>(fkId, reactionType, fromUserId);

    public async Task<FaqModel> ShowFaqAsync(int projectId, int faqId, bool showDeletedItems = false)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentItem =
                await _projectFaqs
                    .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id == faqId)
                    .Include(x => x.Project)
                    .Include(x => x.User)
                    .Include(x => x.Reactions)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem = await _projectFaqs.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id > faqId)
                .OrderBy(x => x.Id)
                .Include(x => x.Project)
                .Include(x => x.User)
                .Include(x => x.Reactions)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(),
            PreviousItem = await _projectFaqs.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id < faqId)
                .OrderByDescending(x => x.Id)
                .Include(x => x.Project)
                .Include(x => x.Reactions)
                .Include(x => x.User)
                .FirstOrDefaultAsync()
        };

    public Task<int> GetAllProjectFaqsCountAsync(bool showDeletedItems = false)
        => _projectFaqs.AsNoTracking().CountAsync(x => x.IsDeleted == showDeletedItems);

    public async Task UpdateProjectFaqAsync(ProjectFaq? projectFaq, ProjectFaqFormModel writeProjectFaqFormModel)
    {
        ArgumentNullException.ThrowIfNull(writeProjectFaqFormModel);

        if (projectFaq is null)
        {
            return;
        }

        mapper.Map(writeProjectFaqFormModel, projectFaq);

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(projectFaq.MapToProjectsFaqsWhatsNewItemModel(siteRootUri: ""));
    }

    public async Task<ProjectFaq?> AddProjectFaqAsync(ProjectFaqFormModel writeProjectFaqFormModel,
        User? user,
        int projectId)
    {
        ArgumentNullException.ThrowIfNull(writeProjectFaqFormModel);

        var projectFaq = mapper.Map<ProjectFaqFormModel, ProjectFaq>(writeProjectFaqFormModel);
        projectFaq.UserId = user?.Id;
        projectFaq.ProjectId = projectId;
        var result = AddProjectFaq(projectFaq);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToProjectsFaqsWhatsNewItemModel(siteRootUri: ""));

        return result;
    }

    public async Task NotifyAddOrUpdateChangesAsync(ProjectFaq? projectFaq,
        ProjectFaqFormModel writeProjectFaqFormModel,
        User? user)
    {
        if (projectFaq is null)
        {
            return;
        }

        await statService.UpdateProjectNumberOfFaqsAsync(projectFaq.ProjectId);

        await emailsService.SendNewFaqEmailAsync(projectFaq.ProjectId, projectFaq.Id, projectFaq.Title,
            projectFaq.Description);
    }

    public async Task MarkAsDeletedAsync(ProjectFaq? projectFaq)
    {
        if (projectFaq is null)
        {
            return;
        }

        projectFaq.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(projectFaq.MapToProjectsFaqsWhatsNewItemModel(siteRootUri: ""));
    }

    public async Task NotifyDeleteChangesAsync(ProjectFaq? projectFaq, User? currentUserUser)
    {
        if (projectFaq is null)
        {
            return;
        }

        await statService.UpdateProjectNumberOfFaqsAsync(projectFaq.ProjectId);

        await emailsService.SendNewFaqEmailAsync(projectFaq.ProjectId, projectFaq.Id, $"{projectFaq.Title} حذف شد ",
            projectFaq.Description);
    }

    public Task IndexProjectFaqsAsync()
    {
        var items = _projectFaqs.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.User)
            .Include(x => x.Project)
            .OrderByDescending(x => x.Id)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToProjectsFaqsWhatsNewItemModel(siteRootUri: "")));
    }
}

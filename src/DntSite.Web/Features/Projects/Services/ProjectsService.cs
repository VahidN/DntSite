using AutoMapper;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ModelsMappings;
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

public class ProjectsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IUploadFileService uploadFileService,
    IStatService statService,
    IMapper mapper,
    ITagsService tagsService,
    IAppFoldersService appFoldersService,
    IProjectsEmailsService emailsService,
    IFullTextSearchService fullTextSearchService,
    ILogger<ProjectsService> logger) : IProjectsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<Project, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<Project> _projects = uow.DbSet<Project>();

    public ValueTask<Project?> FindProjectAsync(int id) => _projects.FindAsync(id);

    public Task<Project?> FindProjectIncludeTagsAndUserAsync(int id, bool showDeletedItems = false)
        => _projects.Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public Project AddProject(Project data) => _projects.Add(data).Entity;

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<ProjectReaction, Project>(fkId, reactionType, fromUserId);

    public Task<int> GetAllProjectsCountAsync(bool showDeletedItems = false)
        => _projects.AsNoTracking().CountAsync(x => x.IsDeleted == showDeletedItems);

    public Task<List<Project>> GetAllPublicProjectsOfDateAsync(DateTime date)
        => _projects.AsNoTracking()
            .Include(project => project.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Where(project => !project.IsDeleted && project.Audit.CreatedAt.Year == date.Year &&
                              project.Audit.CreatedAt.Month == date.Month && project.Audit.CreatedAt.Day == date.Day)
            .OrderBy(project => project.Id)
            .ToListAsync();

    public Task<PagedResultModel<Project>> GetPagedProjectItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projects.Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Project>> GetLastProjectsByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _projects.AsNoTracking() from t in b.Tags where t.Name == tag select b;

        query = query.Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Project>> GetLastProjectsByAuthorIncludeAuthorTagsAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projects.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Where(x => x.User!.FriendlyName == authorName && x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Project>> GetLastPagedProjectsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false)
    {
        var query = _projects.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(Project.Id), [
            .. GridifyMapings.GetDefaultMappings<Project>(), new GridifyMap<Project>
            {
                From = ProjectsMappingsProfiles.ProjectTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public async Task<ProjectsModel> GetProjectsLastAndNextAsync(int id, bool showDeletedItems = false)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentItem =
                await _projects.Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
                    .Include(x => x.User)
                    .Include(blogPost => blogPost.Reactions)
                    .Include(x => x.Tags)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem = await _projects.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id > id)
                .OrderBy(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(),
            PreviousItem = await _projects.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id < id)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync()
        };

    public async Task MarkAsDeletedAsync(Project? project)
    {
        if (project is null)
        {
            return;
        }

        project.IsDeleted = true;
        await uow.SaveChangesAsync();

        logger.LogWarning(message: "Deleted a Project record with Id={Id} and Title={Text}", project.Id, project.Title);

        fullTextSearchService.DeleteLuceneDocument(project
            .MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)
            .DocumentTypeIdHash);
    }

    public async Task NotifyDeleteChangesAsync(Project? project, User? currentUserUser)
    {
        if (project is null)
        {
            return;
        }

        await emailsService.NewProjectEmailToAdminsAsync(project.Id, new ProjectModel
        {
            DescriptionText = "حذف شد",
            Title = project.Title,
            DevelopersDescriptionText = "حذف شد",
            LicenseText = "حذف شد",
            SourcecodeRepositoryUrl = "حذف شد",
            RequiredDependenciesText = "حذف شد",
            RelatedArticlesText = "حذف شد"
        });

        await UpdateStatAsync(new ProjectModel(), currentUserUser);
    }

    public async Task UpdateProjectAsync(Project? project, ProjectModel writeProjectModel)
    {
        ArgumentNullException.ThrowIfNull(writeProjectModel);

        if (project is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveProjectItemTagsAsync(writeProjectModel.Tags);

        mapper.Map(writeProjectModel, project);
        await SavePostedPhotoAsync(project, writeProjectModel);
        project.Tags = listOfActualTags;

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(
            project.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false));
    }

    public async Task<Project?> AddProjectAsync(ProjectModel writeProjectModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeProjectModel);

        var listOfActualTags = await tagsService.SaveProjectItemTagsAsync(writeProjectModel.Tags);

        var project = mapper.Map<ProjectModel, Project>(writeProjectModel);
        project.Tags = listOfActualTags;
        project.UserId = user?.Id;
        await SavePostedPhotoAsync(project, writeProjectModel);
        var result = AddProject(project);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: "",
            showBriefDescription: false));

        return result;
    }

    public async Task NotifyAddOrUpdateChangesAsync(Project? project, ProjectModel writeProjectModel, User? user)
    {
        if (project is null || user is null)
        {
            return;
        }

        await emailsService.NewProjectEmailToAdminsAsync(project.Id, writeProjectModel);

        await UpdateStatAsync(writeProjectModel, user);
    }

    public async Task IndexProjectsAsync()
    {
        var items = await _projects.Where(x => !x.IsDeleted)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking()
            .ToListAsync();

        await fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)));
    }

    private async Task SavePostedPhotoAsync(Project? project, ProjectModel writeProjectModel)
    {
        if (project is null)
        {
            return;
        }

        if (writeProjectModel.PhotoFiles is { Count: > 0 })
        {
            var photoFile = writeProjectModel.PhotoFiles[index: 0];

            if (photoFile is null)
            {
                return;
            }

            var savePath = appFoldersService.GetFolderPath(FileType.Avatar);

            var (isSaved, savedFilePath) =
                await uploadFileService.SavePostedFileAsync(photoFile, savePath, allowOverwrite: false);

            if (isSaved && !string.IsNullOrWhiteSpace(savedFilePath))
            {
                project.Logo = Path.GetFileName(savedFilePath);
            }
        }
    }

    private async Task UpdateStatAsync(ProjectModel? writeProjectModel, User? user)
    {
        if (writeProjectModel is null || user is null)
        {
            return;
        }

        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(user.Id);
        await statService.RecalculateTagsInUseCountsAsync<ProjectTag, Project>();
    }
}

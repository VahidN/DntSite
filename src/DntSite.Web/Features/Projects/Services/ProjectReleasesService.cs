using AutoMapper;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
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

public class ProjectReleasesService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IUploadFileService uploadFileService,
    IStatService statService,
    IAppFoldersService appFoldersService,
    IMapper mapper,
    IEmailsFactoryService emailsService,
    IFullTextSearchService fullTextSearchService) : IProjectReleasesService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<ProjectRelease, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Project.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<ProjectRelease> _projectReleases = uow.DbSet<ProjectRelease>();

    public ValueTask<ProjectRelease?> FindProjectReleaseAsync(int id) => _projectReleases.FindAsync(id);

    public Task<ProjectRelease?> GetProjectReleaseAsync(int id, bool showDeletedItems = false)
        => _projectReleases.Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .Include(x => x.Project)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<ProjectReleaseReaction, ProjectRelease>(fkId, reactionType, fromUserId);

    public ProjectRelease AddProjectRelease(int userId,
        int projectId,
        string fileName,
        long fileSize,
        string path,
        string description)
        => _projectReleases.Add(new ProjectRelease
            {
                UserId = userId,
                FileName = fileName,
                FileSize = fileSize,
                FileDescription = description,
                ProjectId = projectId
            })
            .Entity;

    public Task<PagedResultModel<ProjectRelease>> GetAllProjectsReleasesIncludeProjectsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectReleases.Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectRelease>> GetAllProjectReleasesAsync(int projectId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectReleases.Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId)
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<ProjectRelease>> GetLastPagedProjectReleasesOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _projectReleases.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task MarkAsDeletedAsync(ProjectRelease? projectRelease)
    {
        if (projectRelease is null)
        {
            return;
        }

        projectRelease.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(projectRelease
            .MapToProjectsReleasesWhatsNewItemModel(siteRootUri: "")
            .DocumentTypeIdHash);
    }

    public async Task NotifyDeleteChangesAsync(ProjectRelease? projectRelease, User? currentUserUser)
    {
        if (projectRelease is null)
        {
            return;
        }

        await emailsService.SendTextToAllAdminsAsync(
            $"فایلی از پروژه\u200cها حذف شد: {projectRelease.FileDescription}");

        await statService.UpdateProjectNumberOfReleasesStatAsync(projectRelease.ProjectId);
    }

    public async Task UpdateProjectReleaseAsync(ProjectRelease? projectRelease,
        ProjectPostFileModel projectPostFileModel)
    {
        ArgumentNullException.ThrowIfNull(projectPostFileModel);

        if (projectRelease is null)
        {
            return;
        }

        mapper.Map(projectPostFileModel, projectRelease);
        await SavePostedFileAsync(projectRelease, projectPostFileModel);

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(
            projectRelease.MapToProjectsReleasesWhatsNewItemModel(siteRootUri: ""));
    }

    public async Task<ProjectRelease?> AddProjectReleaseAsync(ProjectPostFileModel projectPostFileModel,
        User? user,
        int projectId)
    {
        ArgumentNullException.ThrowIfNull(projectPostFileModel);

        var project = mapper.Map<ProjectPostFileModel, ProjectRelease>(projectPostFileModel);
        project.ProjectId = projectId;
        project.UserId = user?.Id;
        await SavePostedFileAsync(project, projectPostFileModel);
        var result = _projectReleases.Add(project).Entity;
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToProjectsReleasesWhatsNewItemModel(siteRootUri: ""));

        return result;
    }

    public async Task NotifyAddOrUpdateChangesAsync(ProjectRelease? projectRelease,
        ProjectPostFileModel projectPostFileModel,
        User? user)
    {
        if (projectRelease is null)
        {
            return;
        }

        await emailsService.SendTextToAllAdminsAsync(
            $"فایلی به پروژه\u200cها اضافه شد: {projectRelease.FileDescription}");

        await statService.UpdateProjectNumberOfReleasesStatAsync(projectRelease.ProjectId);
    }

    public async Task<ReleaseModel> ShowReleaseAsync(int projectId, int faqId, bool showDeletedItems = false)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentItem =
                await _projectReleases
                    .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id == faqId)
                    .Include(x => x.Project)
                    .Include(x => x.User)
                    .Include(x => x.Reactions)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem = await _projectReleases.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id > faqId)
                .OrderBy(x => x.Id)
                .Include(x => x.Project)
                .Include(x => x.User)
                .Include(x => x.Reactions)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(),
            PreviousItem = await _projectReleases.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && x.Id < faqId)
                .OrderByDescending(x => x.Id)
                .Include(x => x.Project)
                .Include(x => x.Reactions)
                .Include(x => x.User)
                .FirstOrDefaultAsync()
        };

    public Task IndexProjectReleasesAsync()
    {
        var items = _projectReleases.Where(x => !x.IsDeleted)
            .Include(x => x.User)
            .Include(x => x.Project)
            .Include(x => x.Reactions)
            .AsNoTracking()
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToProjectsReleasesWhatsNewItemModel(siteRootUri: "")));
    }

    private async Task SavePostedFileAsync(ProjectRelease? projectRelease, ProjectPostFileModel model)
    {
        if (projectRelease is null)
        {
            return;
        }

        if (model.ProjectFiles is { Count: > 0 })
        {
            var file = model.ProjectFiles[index: 0];

            if (file is null)
            {
                return;
            }

            var savePath = appFoldersService.GetFolderPath(FileType.ProjectFiles);

            var (isSaved, savedFilePath) =
                await uploadFileService.SavePostedFileAsync(file, savePath, allowOverwrite: false);

            if (isSaved && !string.IsNullOrWhiteSpace(savedFilePath))
            {
                projectRelease.FileName = Path.GetFileName(savedFilePath);
                projectRelease.FileSize = new FileInfo(savedFilePath).Length;
            }
        }
    }
}

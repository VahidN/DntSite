using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectsService : IScopedService
{
    public ValueTask<Project?> FindProjectAsync(int id);

    public Task<ProjectsModel> GetProjectsLastAndNextAsync(int id, bool showDeletedItems = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Project AddProject(Project data);

    public Task<PagedResultModel<Project>> GetPagedProjectItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<Project?> FindProjectIncludeTagsAndUserAsync(int id, bool showDeletedItems = false);

    public Task<PagedResultModel<Project>> GetLastProjectsByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Project>> GetLastProjectsByAuthorIncludeAuthorTagsAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<int> GetAllProjectsCountAsync(bool showDeletedItems = false);

    public Task<List<Project>> GetAllPublicProjectsOfDateAsync(DateTime date);

    public Task<PagedResultModel<Project>> GetLastPagedProjectsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    public Task MarkAsDeletedAsync(Project? project);

    public Task NotifyDeleteChangesAsync(Project? project, User? currentUserUser);

    public Task UpdateProjectAsync(Project? project, ProjectModel writeProjectModel);

    public Task<Project?> AddProjectAsync(ProjectModel writeProjectModel, User? user);

    public Task NotifyAddOrUpdateChangesAsync(Project? project, ProjectModel writeProjectModel, User? user);

    public Task IndexProjectsAsync();
}

using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectsService : IScopedService
{
    ValueTask<Project?> FindProjectAsync(int id);

    Task<ProjectsModel> GetProjectsLastAndNextAsync(int id, bool showDeletedItems = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Project AddProject(Project data);

    Task<PagedResultModel<Project>> GetPagedProjectItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<Project?> FindProjectIncludeTagsAndUserAsync(int id, bool showDeletedItems = false);

    Task<PagedResultModel<Project>> GetLastProjectsByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Project>> GetLastProjectsByAuthorIncludeAuthorTagsAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<int> GetAllProjectsCountAsync(bool showDeletedItems = false);

    Task<List<Project>> GetAllPublicProjectsOfDateAsync(DateTime date);

    Task<PagedResultModel<Project>> GetLastPagedProjectsAsync(
        DntQueryBuilderModel state,
        bool showDeletedItems = false);

    Task MarkAsDeletedAsync(Project? project);

    Task NotifyDeleteChangesAsync(Project? project, User? currentUserUser);

    Task UpdateProjectAsync(Project? project, ProjectModel writeProjectModel);

    Task<Project?> AddProjectAsync(ProjectModel writeProjectModel, User? user);

    Task NotifyAddOrUpdateChangesAsync(Project? project, ProjectModel writeProjectModel, User? user);

    Task IndexProjectsAsync();
}

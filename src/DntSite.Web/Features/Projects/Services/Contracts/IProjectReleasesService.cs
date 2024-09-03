using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectReleasesService : IScopedService
{
    Task<ReleaseModel> ShowReleaseAsync(int projectId, int faqId, bool showDeletedItems = false);

    ValueTask<ProjectRelease?> FindProjectReleaseAsync(int id);

    Task<ProjectRelease?> GetProjectReleaseAsync(int id, bool showDeletedItems = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    ProjectRelease AddProjectRelease(int userId,
        int projectId,
        string fileName,
        long fileSize,
        string path,
        string description);

    Task<PagedResultModel<ProjectRelease>> GetAllProjectsReleasesIncludeProjectsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectRelease>> GetAllProjectReleasesAsync(int projectId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectRelease>> GetLastPagedProjectReleasesOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task MarkAsDeletedAsync(ProjectRelease? projectRelease);

    Task NotifyDeleteChangesAsync(ProjectRelease? projectRelease, User? currentUserUser);

    Task UpdateProjectReleaseAsync(ProjectRelease? projectRelease, ProjectPostFileModel projectPostFileModel);

    Task<ProjectRelease?> AddProjectReleaseAsync(ProjectPostFileModel projectPostFileModel, User? user, int projectId);

    Task NotifyAddOrUpdateChangesAsync(ProjectRelease? projectRelease,
        ProjectPostFileModel projectPostFileModel,
        User? user);

    Task IndexProjectReleasesAsync();
}

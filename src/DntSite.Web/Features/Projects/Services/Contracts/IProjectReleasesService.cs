using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectReleasesService : IScopedService
{
    public Task<ReleaseModel> ShowReleaseAsync(int projectId, int faqId, bool showDeletedItems = false);

    public ValueTask<ProjectRelease?> FindProjectReleaseAsync(int id);

    public Task<ProjectRelease?> GetProjectReleaseAsync(int id, bool showDeletedItems = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public ProjectRelease AddProjectRelease(int userId,
        int projectId,
        string fileName,
        long fileSize,
        string path,
        string description);

    public Task<PagedResultModel<ProjectRelease>> GetAllProjectsReleasesIncludeProjectsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectRelease>> GetAllProjectReleasesAsync(int projectId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectRelease>> GetLastPagedProjectReleasesOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task MarkAsDeletedAsync(ProjectRelease? projectRelease);

    public Task NotifyDeleteChangesAsync(ProjectRelease? projectRelease, User? currentUserUser);

    public Task UpdateProjectReleaseAsync(ProjectRelease? projectRelease, ProjectPostFileModel projectPostFileModel);

    public Task<ProjectRelease?> AddProjectReleaseAsync(ProjectPostFileModel projectPostFileModel,
        User? user,
        int projectId);

    public Task NotifyAddOrUpdateChangesAsync(ProjectRelease? projectRelease,
        ProjectPostFileModel projectPostFileModel,
        User? user);

    public Task IndexProjectReleasesAsync();
}

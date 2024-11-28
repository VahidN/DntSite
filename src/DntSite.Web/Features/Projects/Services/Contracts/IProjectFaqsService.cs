using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectFaqsService : IScopedService
{
    public ValueTask<ProjectFaq?> FindProjectFaqAsync(int id);

    public Task<ProjectFaq?> GetProjectFaqAsync(int id, bool showDeletedItems = false);

    public ProjectFaq AddProjectFaq(ProjectFaq data);

    public Task<List<ProjectFaq>> GetAllLastPagedProjectFaqsAsync(int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false);

    public Task<PagedResultModel<ProjectFaq>> GetLastPagedProjectFaqsAsync(int projectId,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectFaq>> GetLastPagedAllProjectsFaqsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectFaq>> GetLastPagedProjectFaqsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<FaqModel> ShowFaqAsync(int projectId, int faqId, bool showDeletedItems = false);

    public Task<int> GetAllProjectFaqsCountAsync(bool showDeletedItems = false);

    public Task UpdateProjectFaqAsync(ProjectFaq? projectFaq, ProjectFaqFormModel writeProjectFaqFormModel);

    public Task<ProjectFaq?>
        AddProjectFaqAsync(ProjectFaqFormModel writeProjectFaqFormModel, User? user, int projectId);

    public Task NotifyAddOrUpdateChangesAsync(ProjectFaq? projectFaq,
        ProjectFaqFormModel writeProjectFaqFormModel,
        User? user);

    public Task MarkAsDeletedAsync(ProjectFaq? projectFaq);

    public Task NotifyDeleteChangesAsync(ProjectFaq? projectFaq, User? currentUserUser);

    public Task IndexProjectFaqsAsync();
}

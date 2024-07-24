using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectFaqsService : IScopedService
{
    ValueTask<ProjectFaq?> FindProjectFaqAsync(int id);

    Task<ProjectFaq?> GetProjectFaqAsync(int id, bool showDeletedItems = false);

    ProjectFaq AddProjectFaq(ProjectFaq data);

    Task<List<ProjectFaq>> GetAllLastPagedProjectFaqsAsync(int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false);

    Task<PagedResultModel<ProjectFaq>> GetLastPagedProjectFaqsAsync(int projectId,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectFaq>> GetLastPagedAllProjectsFaqsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectFaq>> GetLastPagedProjectFaqsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<FaqModel> ShowFaqAsync(int projectId, int faqId, bool showDeletedItems = false);

    Task<int> GetAllProjectFaqsCountAsync(bool showDeletedItems = false);

    Task UpdateProjectFaqAsync(ProjectFaq? projectFaq, ProjectFaqFormModel writeProjectFaqFormModel);

    Task<ProjectFaq?> AddProjectFaqAsync(ProjectFaqFormModel writeProjectFaqFormModel, User? user, int projectId);

    Task NotifyAddOrUpdateChangesAsync(ProjectFaq? projectFaq,
        ProjectFaqFormModel writeProjectFaqFormModel,
        User? user);

    Task MarkAsDeletedAsync(ProjectFaq? projectFaq);

    Task NotifyDeleteChangesAsync(ProjectFaq? projectFaq, User? currentUserUser);
}

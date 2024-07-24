using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IIssuePrioritiesService : IScopedService
{
    ProjectIssuePriority AddIssuePriority(ProjectIssuePriority data);

    ValueTask<ProjectIssuePriority?> FindIssuePriorityAsync(int id);

    Task<List<ProjectIssuePriority>> GetAllProjectIssuePrioritiesListAsNoTrackingAsync(int count, bool isActive = true);
}

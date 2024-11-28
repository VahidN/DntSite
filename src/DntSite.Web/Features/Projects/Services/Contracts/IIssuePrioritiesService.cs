using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IIssuePrioritiesService : IScopedService
{
    public ProjectIssuePriority AddIssuePriority(ProjectIssuePriority data);

    public ValueTask<ProjectIssuePriority?> FindIssuePriorityAsync(int id);

    public Task<List<ProjectIssuePriority>> GetAllProjectIssuePrioritiesListAsNoTrackingAsync(int count,
        bool isActive = true);
}

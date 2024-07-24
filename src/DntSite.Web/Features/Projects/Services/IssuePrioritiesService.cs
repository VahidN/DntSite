using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Services;

public class IssuePrioritiesService(IUnitOfWork uow) : IIssuePrioritiesService
{
    private readonly DbSet<ProjectIssuePriority> _issuePriorities = uow.DbSet<ProjectIssuePriority>();

    public ValueTask<ProjectIssuePriority?> FindIssuePriorityAsync(int id) => _issuePriorities.FindAsync(id);

    public ProjectIssuePriority AddIssuePriority(ProjectIssuePriority data) => _issuePriorities.Add(data).Entity;

    public Task<List<ProjectIssuePriority>> GetAllProjectIssuePrioritiesListAsNoTrackingAsync(int count,
        bool isActive = true)
        => _issuePriorities.AsNoTracking()
            .Where(x => x.IsDeleted != isActive)
            .OrderBy(x => x.Name)
            .Take(count)
            .ToListAsync();
}

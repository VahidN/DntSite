using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Services;

public class ProjectsTagsService(IUnitOfWork uow) : IProjectsTagsService
{
    private readonly DbSet<ProjectTag> _projectTags = uow.DbSet<ProjectTag>();

    public ValueTask<ProjectTag?> FindProjectTagAsync(int id) => _projectTags.FindAsync(id);

    public ProjectTag AddProjectTag(ProjectTag data) => _projectTags.Add(data).Entity;

    public Task<List<ProjectTag>> GetAllProjectTagsListAsNoTrackingAsync(int count, bool isActive = true)
        => _projectTags.AsNoTracking()
            .Where(x => x.IsDeleted != isActive)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .ToListAsync();
}

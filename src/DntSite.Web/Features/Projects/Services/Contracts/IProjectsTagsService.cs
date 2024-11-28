using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectsTagsService : IScopedService
{
    public ValueTask<ProjectTag?> FindProjectTagAsync(int id);

    public ProjectTag AddProjectTag(ProjectTag data);

    public Task<List<ProjectTag>> GetAllProjectTagsListAsNoTrackingAsync(int count, bool isActive = true);
}

using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectsTagsService : IScopedService
{
    ValueTask<ProjectTag?> FindProjectTagAsync(int id);

    ProjectTag AddProjectTag(ProjectTag data);

    Task<List<ProjectTag>> GetAllProjectTagsListAsNoTrackingAsync(int count, bool isActive = true);
}

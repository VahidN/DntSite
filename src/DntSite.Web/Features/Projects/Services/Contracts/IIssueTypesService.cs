using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IIssueTypesService : IScopedService
{
    public ValueTask<ProjectIssueType?> FindIssueTypeAsync(int id);

    public ProjectIssueType AddIssueType(ProjectIssueType data);

    public Task<List<ProjectIssueType>> GetAllProjectIssueTypesListAsNoTrackingAsync(int count, bool isActive = true);

    public Task<List<SimpleItemModel>> GetProjectIssueTypesListAsync(int projectId, int count, bool isActive = true);

    public Task<int> GetNewProjectIssueTypesCountAsync(int projectId, bool showDeletedItems = false);
}

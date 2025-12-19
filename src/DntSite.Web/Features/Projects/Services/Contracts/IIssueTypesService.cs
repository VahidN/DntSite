using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IIssueTypesService : IScopedService
{
    ValueTask<ProjectIssueType?> FindIssueTypeAsync(int id);

    ProjectIssueType AddIssueType(ProjectIssueType data);

    Task<List<ProjectIssueType>> GetAllProjectIssueTypesListAsNoTrackingAsync(int count, bool isActive = true);

    Task<List<SimpleItemModel>> GetProjectIssueTypesListAsync(int projectId, int count, bool isActive = true);

    Task<int> GetNewProjectIssueTypesCountAsync(int projectId, bool showDeletedItems = false);
}

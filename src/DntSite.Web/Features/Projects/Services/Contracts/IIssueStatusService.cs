using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IIssueStatusService : IScopedService
{
    public ValueTask<ProjectIssueStatus?> FindIssueStatusAsync(int id);

    public ProjectIssueStatus AddIssueStatus(ProjectIssueStatus data);

    public Task<List<ProjectIssueStatus>>
        GetAllProjectIssueStatusListAsNoTrackingAsync(int count, bool isActive = true);

    public Task<List<SimpleItemModel>> GetProjectIssueStatusListAsync(int projectId, int count, bool isActive = true);

    public Task<int> GetNewProjectIssueStatusCountAsync(int projectId, bool showDeletedItems = false);
}

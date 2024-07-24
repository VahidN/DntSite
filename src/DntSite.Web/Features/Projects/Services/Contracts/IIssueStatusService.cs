using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IIssueStatusService : IScopedService
{
    ValueTask<ProjectIssueStatus?> FindIssueStatusAsync(int id);

    ProjectIssueStatus AddIssueStatus(ProjectIssueStatus data);

    Task<List<ProjectIssueStatus>> GetAllProjectIssueStatusListAsNoTrackingAsync(int count, bool isActive = true);

    Task<List<SimpleItemModel>> GetProjectIssueStatusListAsync(int projectId, int count, bool isActive = true);

    Task<int> GetNewProjectIssueStatusCountAsync(int projectId, bool showDeletedItems = false);
}

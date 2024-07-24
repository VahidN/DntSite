using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Services;

public class IssueStatusService(IUnitOfWork uow) : IIssueStatusService
{
    private readonly DbSet<ProjectIssueStatus> _issueStatus = uow.DbSet<ProjectIssueStatus>();
    private readonly DbSet<ProjectIssue> _projectIssues = uow.DbSet<ProjectIssue>();

    public ValueTask<ProjectIssueStatus?> FindIssueStatusAsync(int id) => _issueStatus.FindAsync(id);

    public ProjectIssueStatus AddIssueStatus(ProjectIssueStatus data) => _issueStatus.Add(data).Entity;

    public Task<List<ProjectIssueStatus>> GetAllProjectIssueStatusListAsNoTrackingAsync(int count, bool isActive = true)
        => _issueStatus.AsNoTracking()
            .Where(x => x.IsDeleted != isActive)
            .OrderBy(x => x.Name)
            .Take(count)
            .ToListAsync();

    public Task<List<SimpleItemModel>> GetProjectIssueStatusListAsync(int projectId, int count, bool isActive = true)
        => _issueStatus.AsNoTracking()
            .Where(x => x.IsDeleted != isActive)
            .Select(g => new SimpleItemModel
            {
                Id = g.Id,
                Name = g.Name,
                Count = g.AssociatedEntities.Count(p => p.ProjectId == projectId && p.IsDeleted == !isActive)
            })
            .OrderBy(x => x.Name)
            .Take(count)
            .ToListAsync();

    public Task<int> GetNewProjectIssueStatusCountAsync(int projectId, bool showDeletedItems = false)
        => _projectIssues.AsNoTracking()
            .CountAsync(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && !x.IssueStatusId.HasValue);
}

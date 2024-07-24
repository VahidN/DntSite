using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Services;

public class IssueTypesService(IUnitOfWork uow) : IIssueTypesService
{
    private readonly DbSet<ProjectIssueType> _issueType = uow.DbSet<ProjectIssueType>();
    private readonly DbSet<ProjectIssue> _projectIssues = uow.DbSet<ProjectIssue>();

    public ValueTask<ProjectIssueType?> FindIssueTypeAsync(int id) => _issueType.FindAsync(id);

    public ProjectIssueType AddIssueType(ProjectIssueType data) => _issueType.Add(data).Entity;

    public Task<List<ProjectIssueType>> GetAllProjectIssueTypesListAsNoTrackingAsync(int count, bool isActive = true)
        => _issueType.AsNoTracking().Where(x => x.IsDeleted != isActive).OrderBy(x => x.Name).Take(count).ToListAsync();

    public Task<List<SimpleItemModel>> GetProjectIssueTypesListAsync(int projectId, int count, bool isActive = true)
        => _issueType.AsNoTracking()
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

    public Task<int> GetNewProjectIssueTypesCountAsync(int projectId, bool showDeletedItems = false)
        => _projectIssues.AsNoTracking()
            .CountAsync(x => x.IsDeleted == showDeletedItems && x.ProjectId == projectId && !x.IssueTypeId.HasValue);
}

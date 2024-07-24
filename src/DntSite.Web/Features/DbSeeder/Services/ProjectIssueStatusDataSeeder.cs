using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.DbSeeder.Services;

public class ProjectIssueStatusDataSeeder(IUnitOfWork uow) : IDataSeeder
{
    private readonly DbSet<ProjectIssueStatus> _projectIssueStatus = uow.DbSet<ProjectIssueStatus>();
    private readonly IUnitOfWork _uow = uow ?? throw new ArgumentNullException(nameof(uow));

    public int Order { get; set; } = 4;

    public void SeedData()
    {
        if (_projectIssueStatus.Any())
        {
            return;
        }

        _projectIssueStatus.AddRange(new ProjectIssueStatus
        {
            Name = "در حال انجام"
        }, new ProjectIssueStatus
        {
            Name = "برطرف شد"
        }, new ProjectIssueStatus
        {
            Name = "رد شد"
        }, new ProjectIssueStatus
        {
            Name = "راهنمایی شد"
        });

        _uow.SaveChanges();
    }
}

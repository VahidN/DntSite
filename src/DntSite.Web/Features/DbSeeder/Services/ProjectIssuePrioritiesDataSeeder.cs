using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.DbSeeder.Services;

public class ProjectIssuePrioritiesDataSeeder(IUnitOfWork uow) : IDataSeeder
{
    private readonly DbSet<ProjectIssuePriority> _projectIssuePriorities = uow.DbSet<ProjectIssuePriority>();
    private readonly IUnitOfWork _uow = uow ?? throw new ArgumentNullException(nameof(uow));

    public int Order { get; set; } = 3;

    public void SeedData()
    {
        if (_projectIssuePriorities.Any())
        {
            return;
        }

        _projectIssuePriorities.AddRange(new ProjectIssuePriority
        {
            Name = "خیلی مهم"
        }, new ProjectIssuePriority
        {
            Name = "مهم"
        }, new ProjectIssuePriority
        {
            Name = "عادی"
        });

        _uow.SaveChanges();
    }
}

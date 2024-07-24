using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.DbSeeder.Services;

public class ProjectIssueTypesDataSeeder(IUnitOfWork uow) : IDataSeeder
{
    private readonly DbSet<ProjectIssueType> _projectIssueTypes = uow.DbSet<ProjectIssueType>();
    private readonly IUnitOfWork _uow = uow ?? throw new ArgumentNullException(nameof(uow));

    public int Order { get; set; } = 5;

    public void SeedData()
    {
        if (_projectIssueTypes.Any())
        {
            return;
        }

        _projectIssueTypes.AddRange(new ProjectIssueType
        {
            Name = "گزارش خطا"
        }, new ProjectIssueType
        {
            Name = "درخواست راهنمایی"
        }, new ProjectIssueType
        {
            Name = "درخواست قابلیت جدید"
        }, new ProjectIssueType
        {
            Name = "ارائه وصله"
        });

        _uow.SaveChanges();
    }
}

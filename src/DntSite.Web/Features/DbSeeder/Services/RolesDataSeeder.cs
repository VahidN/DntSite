using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.DbSeeder.Services;

public class RolesDataSeeder(IUnitOfWork uow) : IDataSeeder
{
    private readonly DbSet<Role> _roles = uow.DbSet<Role>();
    private readonly IUnitOfWork _uow = uow ?? throw new ArgumentNullException(nameof(uow));

    public int Order { get; set; } = 1;

    public void SeedData()
    {
        if (_roles.Any())
        {
            return;
        }

        // Add default roles
        var adminRole = new Role
        {
            Name = CustomRoles.Admin
        };

        var writeRole = new Role
        {
            Name = CustomRoles.Write
        };

        var readRole = new Role
        {
            Name = CustomRoles.Read
        };

        var changeRole = new Role
        {
            Name = CustomRoles.Change
        };

        var deleteRole = new Role
        {
            Name = CustomRoles.Delete
        };

        var userRole = new Role
        {
            Name = CustomRoles.User
        };

        var editorRole = new Role
        {
            Name = CustomRoles.Editor
        };

        _roles.AddRange(adminRole, writeRole, readRole, changeRole, deleteRole, userRole, editorRole);
        _uow.SaveChanges();
    }
}

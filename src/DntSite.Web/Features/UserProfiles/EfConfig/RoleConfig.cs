using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(role => role.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(role => role.Name).IsUnique();

        builder.HasMany(role => role.AssociatedEntities).WithMany(user => user.Roles);

        builder.HasOne(role => role.User)
            .WithMany(user => user.CreatedRoles)
            .HasForeignKey(role => role.UserId)
            .IsRequired(false);
    }
}

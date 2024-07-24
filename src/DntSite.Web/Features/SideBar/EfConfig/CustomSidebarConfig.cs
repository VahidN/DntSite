using DntSite.Web.Features.SideBar.Entities;

namespace DntSite.Web.Features.SideBar.EfConfig;

public class CustomSidebarConfig : IEntityTypeConfiguration<CustomSidebar>
{
    public void Configure(EntityTypeBuilder<CustomSidebar> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CustomSidebars)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

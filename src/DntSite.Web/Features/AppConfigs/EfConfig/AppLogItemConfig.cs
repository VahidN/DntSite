using DntSite.Web.Features.AppConfigs.Entities;

namespace DntSite.Web.Features.AppConfigs.EfConfig;

public class AppLogItemConfig : IEntityTypeConfiguration<AppLogItem>
{
    public void Configure(EntityTypeBuilder<AppLogItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AppLogItems)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

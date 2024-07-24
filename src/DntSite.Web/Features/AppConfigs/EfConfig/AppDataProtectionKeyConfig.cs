using DntSite.Web.Features.AppConfigs.Entities;

namespace DntSite.Web.Features.AppConfigs.EfConfig;

public class AppDataProtectionKeyConfig : IEntityTypeConfiguration<AppDataProtectionKey>
{
    public void Configure(EntityTypeBuilder<AppDataProtectionKey> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AppDataProtectionKeys)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

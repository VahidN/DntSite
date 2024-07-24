using DntSite.Web.Features.AppConfigs.Entities;

namespace DntSite.Web.Features.AppConfigs.EfConfig;

public class AppSettingConfig : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AppSettings)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

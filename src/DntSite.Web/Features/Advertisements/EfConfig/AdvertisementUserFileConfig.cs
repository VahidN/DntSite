using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementUserFileConfig : IEntityTypeConfiguration<AdvertisementUserFile>
{
    public void Configure(EntityTypeBuilder<AdvertisementUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

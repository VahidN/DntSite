using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementTagConfig : IEntityTypeConfiguration<AdvertisementTag>
{
    public void Configure(EntityTypeBuilder<AdvertisementTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(advertisementTag => advertisementTag.AssociatedEntities)
            .WithMany(advertisement => advertisement.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

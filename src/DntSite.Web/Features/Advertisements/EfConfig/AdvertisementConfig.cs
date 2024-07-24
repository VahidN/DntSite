using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementConfig : IEntityTypeConfiguration<Advertisement>
{
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(advertisement => advertisement.Title).HasMaxLength(450).IsRequired();

        builder.Property(advertisement => advertisement.Body).IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.Advertisements)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

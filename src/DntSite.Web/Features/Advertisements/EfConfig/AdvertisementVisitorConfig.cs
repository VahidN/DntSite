using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementVisitorConfig : IEntityTypeConfiguration<AdvertisementVisitor>
{
    public void Configure(EntityTypeBuilder<AdvertisementVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(advertisement => advertisement.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

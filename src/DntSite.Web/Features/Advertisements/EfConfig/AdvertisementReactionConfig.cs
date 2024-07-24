using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementReactionConfig : IEntityTypeConfiguration<AdvertisementReaction>
{
    public void Configure(EntityTypeBuilder<AdvertisementReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(advertisement => advertisement.Reactions)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.AdvertisementReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementCommentReactionConfig : IEntityTypeConfiguration<AdvertisementCommentReaction>
{
    public void Configure(EntityTypeBuilder<AdvertisementCommentReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(commentReaction => commentReaction.Parent)
            .WithMany(advertisementComment => advertisementComment.Reactions)
            .HasForeignKey(commentReaction => commentReaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementCommentReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.AdvertisementCommentReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

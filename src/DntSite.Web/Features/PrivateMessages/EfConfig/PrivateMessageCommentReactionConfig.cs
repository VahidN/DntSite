using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.EfConfig;

public class PrivateMessageCommentReactionConfig : IEntityTypeConfiguration<PrivateMessageCommentReaction>
{
    public void Configure(EntityTypeBuilder<PrivateMessageCommentReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.PrivateMessageCommentReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.PrivateMessageCommentReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.EfConfig;

public class PrivateMessageReactionConfig : IEntityTypeConfiguration<PrivateMessageReaction>
{
    public void Configure(EntityTypeBuilder<PrivateMessageReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.PrivateMessageReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.PrivateMessageReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

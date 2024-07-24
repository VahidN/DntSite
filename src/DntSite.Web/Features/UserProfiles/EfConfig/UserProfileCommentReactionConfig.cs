using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserProfileCommentReactionConfig : IEntityTypeConfiguration<UserProfileCommentReaction>
{
    public void Configure(EntityTypeBuilder<UserProfileCommentReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.UserCommentReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.UserProfileCommentReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserProfileReactionConfig : IEntityTypeConfiguration<UserProfileReaction>
{
    public void Configure(EntityTypeBuilder<UserProfileReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.UserReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.UserProfileReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

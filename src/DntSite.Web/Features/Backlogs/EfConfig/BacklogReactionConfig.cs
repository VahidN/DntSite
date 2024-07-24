using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogReactionConfig : IEntityTypeConfiguration<BacklogReaction>
{
    public void Configure(EntityTypeBuilder<BacklogReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(backlog => backlog.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.BacklogReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

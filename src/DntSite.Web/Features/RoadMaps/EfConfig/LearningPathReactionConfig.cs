using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.EfConfig;

public class LearningPathReactionConfig : IEntityTypeConfiguration<LearningPathReaction>
{
    public void Configure(EntityTypeBuilder<LearningPathReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.LearningPathReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.LearningPathReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

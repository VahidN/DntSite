using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemCommentReactionConfig : IEntityTypeConfiguration<DailyNewsItemCommentReaction>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemCommentReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(reaction => reaction.Parent)
            .WithMany(entity => entity.Reactions)
            .HasForeignKey(reaction => reaction.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItemCommentReactions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(entity => entity.ForUser)
            .WithMany(user => user.DailyNewsItemCommentReactionsForUsers)
            .HasForeignKey(entity => entity.ForUserId)
            .IsRequired(false);
    }
}

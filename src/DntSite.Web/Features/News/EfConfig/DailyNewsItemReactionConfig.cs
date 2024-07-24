using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemReactionConfig : IEntityTypeConfiguration<DailyNewsItemReaction>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemReaction> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(dailyNewsItemReaction => dailyNewsItemReaction.Parent)
            .WithMany(dailyNewsItem => dailyNewsItem.Reactions)
            .HasForeignKey(dailyNewsItemReaction => dailyNewsItemReaction.ParentId)
            .IsRequired();

        builder.HasOne(dailyNewsItemReaction => dailyNewsItemReaction.User)
            .WithMany(user => user.DailyNewsItemReactions)
            .HasForeignKey(dailyNewsItemReaction => dailyNewsItemReaction.UserId)
            .IsRequired(false);

        builder.HasOne(dailyNewsItemReaction => dailyNewsItemReaction.ForUser)
            .WithMany(user => user.DailyNewsItemReactionsForUsers)
            .HasForeignKey(dailyNewsItemReaction => dailyNewsItemReaction.ForUserId)
            .IsRequired(false);
    }
}

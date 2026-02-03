using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemConfig : IEntityTypeConfiguration<DailyNewsItem>
{
    public void Configure(EntityTypeBuilder<DailyNewsItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.Url).HasMaxLength(maxLength: 1000).IsRequired();

        builder.Property(entity => entity.UrlHash).HasMaxLength(maxLength: 50).IsRequired();
        builder.HasIndex(entity => entity.UrlHash).IsUnique();

        builder.Property(entity => entity.Title).HasMaxLength(maxLength: 450).IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItems)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(required: false);

        builder.HasMany(dailyNewsItem => dailyNewsItem.Reactions)
            .WithOne(dailyNewsItemReaction => dailyNewsItemReaction.Parent)
            .HasForeignKey(dailyNewsItemReaction => dailyNewsItemReaction.ParentId)
            .IsRequired(required: false);

        builder.HasOne(entity => entity.DailyNewsItemAIBacklog)
            .WithOne(aiBacklog => aiBacklog.DailyNewsItem)
            .HasForeignKey<DailyNewsItemAIBacklog>(aiBacklog => aiBacklog.DailyNewsItemId)
            .IsRequired(required: false);
    }
}

using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemAIBacklogConfig : IEntityTypeConfiguration<DailyNewsItemAIBacklog>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemAIBacklog> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.Url).HasMaxLength(maxLength: 1000).IsRequired();

        builder.Property(entity => entity.UrlHash).HasMaxLength(maxLength: 50).IsRequired();
        builder.HasIndex(entity => entity.UrlHash).IsUnique();

        builder.Property(entity => entity.Title).HasMaxLength(maxLength: 450).IsRequired(required: false);

        builder.HasOne(entity => entity.DailyNewsItem)
            .WithOne(dailyNewsItem => dailyNewsItem.DailyNewsItemAIBacklog)
            .HasForeignKey<DailyNewsItem>(dailyNewsItem => dailyNewsItem.DailyNewsItemAIBacklogId)
            .IsRequired(required: false);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItemAIBacklogs)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(required: false);
    }
}

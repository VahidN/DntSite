using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.EfConfig;

public class LearningPathBookmarkConfig : IEntityTypeConfiguration<LearningPathBookmark>
{
    public void Configure(EntityTypeBuilder<LearningPathBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.LearningPathBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogBookmarkConfig : IEntityTypeConfiguration<BacklogBookmark>
{
    public void Configure(EntityTypeBuilder<BacklogBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(backlog => backlog.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserProfileBookmarkConfig : IEntityTypeConfiguration<UserProfileBookmark>
{
    public void Configure(EntityTypeBuilder<UserProfileBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.UserBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

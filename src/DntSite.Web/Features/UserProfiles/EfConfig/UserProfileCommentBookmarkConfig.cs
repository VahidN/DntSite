using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserProfileCommentBookmarkConfig : IEntityTypeConfiguration<UserProfileCommentBookmark>
{
    public void Configure(EntityTypeBuilder<UserProfileCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.UserCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

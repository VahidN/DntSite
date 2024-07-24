using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementCommentBookmarkConfig : IEntityTypeConfiguration<AdvertisementCommentBookmark>
{
    public void Configure(EntityTypeBuilder<AdvertisementCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(commentBookmark => commentBookmark.Parent)
            .WithMany(comment => comment.Bookmarks)
            .HasForeignKey(commentBookmark => commentBookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

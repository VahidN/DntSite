using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectCommentBookmarkConfig : IEntityTypeConfiguration<ProjectCommentBookmark>
{
    public void Configure(EntityTypeBuilder<ProjectCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

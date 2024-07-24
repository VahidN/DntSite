using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectBookmarkConfig : IEntityTypeConfiguration<ProjectBookmark>
{
    public void Configure(EntityTypeBuilder<ProjectBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

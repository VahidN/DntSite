using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectFaqBookmarkConfig : IEntityTypeConfiguration<ProjectFaqBookmark>
{
    public void Configure(EntityTypeBuilder<ProjectFaqBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectFaqBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectReleaseBookmarkConfig : IEntityTypeConfiguration<ProjectReleaseBookmark>
{
    public void Configure(EntityTypeBuilder<ProjectReleaseBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectReleaseBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

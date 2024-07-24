using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssueBookmarkConfig : IEntityTypeConfiguration<ProjectIssueBookmark>
{
    public void Configure(EntityTypeBuilder<ProjectIssueBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssueBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

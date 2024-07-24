using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseBookmarkConfig : IEntityTypeConfiguration<CourseBookmark>
{
    public void Configure(EntityTypeBuilder<CourseBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

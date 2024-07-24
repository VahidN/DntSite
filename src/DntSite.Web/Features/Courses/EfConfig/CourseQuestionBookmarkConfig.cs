using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseQuestionBookmarkConfig : IEntityTypeConfiguration<CourseQuestionBookmark>
{
    public void Configure(EntityTypeBuilder<CourseQuestionBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseQuestionBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

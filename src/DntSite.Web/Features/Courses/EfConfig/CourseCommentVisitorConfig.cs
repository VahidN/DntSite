using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseCommentVisitorConfig : IEntityTypeConfiguration<CourseCommentVisitor>
{
    public void Configure(EntityTypeBuilder<CourseCommentVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseCommentVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

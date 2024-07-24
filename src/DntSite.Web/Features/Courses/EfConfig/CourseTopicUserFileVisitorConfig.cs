using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseTopicUserFileVisitorConfig : IEntityTypeConfiguration<CourseTopicUserFileVisitor>
{
    public void Configure(EntityTypeBuilder<CourseTopicUserFileVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseTopicUserFileVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

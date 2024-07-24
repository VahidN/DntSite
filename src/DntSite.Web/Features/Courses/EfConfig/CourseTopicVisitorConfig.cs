using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseTopicVisitorConfig : IEntityTypeConfiguration<CourseTopicVisitor>
{
    public void Configure(EntityTypeBuilder<CourseTopicVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseTopicVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

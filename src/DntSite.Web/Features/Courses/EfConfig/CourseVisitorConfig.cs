using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseVisitorConfig : IEntityTypeConfiguration<CourseVisitor>
{
    public void Configure(EntityTypeBuilder<CourseVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseQuestionVisitorConfig : IEntityTypeConfiguration<CourseQuestionVisitor>
{
    public void Configure(EntityTypeBuilder<CourseQuestionVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseQuestionVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

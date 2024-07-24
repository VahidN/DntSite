using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseTopicConfig : IEntityTypeConfiguration<CourseTopic>
{
    public void Configure(EntityTypeBuilder<CourseTopic> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(courseQuestion => courseQuestion.Title).HasMaxLength(450).IsRequired();

        builder.HasOne(entity => entity.Course)
            .WithMany(course => course.CourseTopics)
            .HasForeignKey(entity => entity.CourseId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseTopics)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

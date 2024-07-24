using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseQuestionConfig : IEntityTypeConfiguration<CourseQuestion>
{
    public void Configure(EntityTypeBuilder<CourseQuestion> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(courseQuestion => courseQuestion.Title).HasMaxLength(450).IsRequired();

        builder.HasOne(entity => entity.Course)
            .WithMany(course => course.CourseQuestions)
            .HasForeignKey(entity => entity.CourseId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseQuestions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

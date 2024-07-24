using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(course => course.Title).HasMaxLength(450).IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.Courses)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

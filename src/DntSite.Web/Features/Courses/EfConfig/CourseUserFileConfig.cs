using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseUserFileConfig : IEntityTypeConfiguration<CourseUserFile>
{
    public void Configure(EntityTypeBuilder<CourseUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

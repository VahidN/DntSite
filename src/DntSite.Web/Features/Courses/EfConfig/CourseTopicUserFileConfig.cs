using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseTopicUserFileConfig : IEntityTypeConfiguration<CourseTopicUserFile>
{
    public void Configure(EntityTypeBuilder<CourseTopicUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseTopicUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

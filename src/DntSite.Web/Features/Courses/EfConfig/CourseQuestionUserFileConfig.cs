using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseQuestionUserFileConfig : IEntityTypeConfiguration<CourseQuestionUserFile>
{
    public void Configure(EntityTypeBuilder<CourseQuestionUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseQuestionUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseTagConfig : IEntityTypeConfiguration<CourseTag>
{
    public void Configure(EntityTypeBuilder<CourseTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

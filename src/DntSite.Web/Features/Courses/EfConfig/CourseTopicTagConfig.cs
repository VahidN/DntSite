using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.EfConfig;

public class CourseTopicTagConfig : IEntityTypeConfiguration<CourseTopicTag>
{
    public void Configure(EntityTypeBuilder<CourseTopicTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.CourseTopicTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

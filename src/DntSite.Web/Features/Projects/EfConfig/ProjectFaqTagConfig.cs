using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectFaqTagConfig : IEntityTypeConfiguration<ProjectFaqTag>
{
    public void Configure(EntityTypeBuilder<ProjectFaqTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectFaqTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

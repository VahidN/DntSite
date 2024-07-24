using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectReleaseTagConfig : IEntityTypeConfiguration<ProjectReleaseTag>
{
    public void Configure(EntityTypeBuilder<ProjectReleaseTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectReleaseTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

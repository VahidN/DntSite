using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectReleaseConfig : IEntityTypeConfiguration<ProjectRelease>
{
    public void Configure(EntityTypeBuilder<ProjectRelease> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(release => release.Project)
            .WithMany(project => project.ProjectReleases)
            .HasForeignKey(release => release.ProjectId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectReleases)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectFaqConfig : IEntityTypeConfiguration<ProjectFaq>
{
    public void Configure(EntityTypeBuilder<ProjectFaq> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(projectFaq => projectFaq.Project)
            .WithMany(project => project.ProjectFaqs)
            .HasForeignKey(projectFaq => projectFaq.ProjectId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectFaqs)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

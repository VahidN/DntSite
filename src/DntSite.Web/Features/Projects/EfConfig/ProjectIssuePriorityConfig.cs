using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssuePriorityConfig : IEntityTypeConfiguration<ProjectIssuePriority>
{
    public void Configure(EntityTypeBuilder<ProjectIssuePriority> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(issuePriority => issuePriority.AssociatedEntities)
            .WithOne(projectIssue => projectIssue.IssuePriority)
            .HasForeignKey(projectIssue => projectIssue.IssuePriorityId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssuePriorities)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

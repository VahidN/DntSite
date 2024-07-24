using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssueStatusConfig : IEntityTypeConfiguration<ProjectIssueStatus>
{
    public void Configure(EntityTypeBuilder<ProjectIssueStatus> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(issueStatus => issueStatus.AssociatedEntities)
            .WithOne(projectIssue => projectIssue.IssueStatus)
            .HasForeignKey(projectIssue => projectIssue.IssueStatusId)
            .IsRequired(false);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssueStatus)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

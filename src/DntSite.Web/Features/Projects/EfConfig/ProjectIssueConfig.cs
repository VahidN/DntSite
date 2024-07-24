using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssueConfig : IEntityTypeConfiguration<ProjectIssue>
{
    public void Configure(EntityTypeBuilder<ProjectIssue> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(issue => issue.Project)
            .WithMany(project => project.ProjectIssues)
            .HasForeignKey(issue => issue.ProjectId)
            .IsRequired();

        builder.HasOne(issue => issue.IssuePriority)
            .WithMany(priority => priority.AssociatedEntities)
            .HasForeignKey(issue => issue.IssuePriorityId)
            .IsRequired();

        builder.HasOne(issue => issue.IssueType)
            .WithMany(issueType => issueType.AssociatedEntities)
            .HasForeignKey(issue => issue.IssueTypeId)
            .IsRequired(false);

        builder.HasOne(issue => issue.IssueStatus)
            .WithMany(issueStatus => issueStatus.AssociatedEntities)
            .HasForeignKey(issue => issue.IssueStatusId)
            .IsRequired(false);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssues)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

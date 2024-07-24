using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssueVisitorConfig : IEntityTypeConfiguration<ProjectIssueVisitor>
{
    public void Configure(EntityTypeBuilder<ProjectIssueVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssueVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

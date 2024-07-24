using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssueCommentVisitorConfig : IEntityTypeConfiguration<ProjectIssueCommentVisitor>
{
    public void Configure(EntityTypeBuilder<ProjectIssueCommentVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssueCommentVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

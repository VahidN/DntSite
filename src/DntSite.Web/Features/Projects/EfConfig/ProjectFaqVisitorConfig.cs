using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectFaqVisitorConfig : IEntityTypeConfiguration<ProjectFaqVisitor>
{
    public void Configure(EntityTypeBuilder<ProjectFaqVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectFaqVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

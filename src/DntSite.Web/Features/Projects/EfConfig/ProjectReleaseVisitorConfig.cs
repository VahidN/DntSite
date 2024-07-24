using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectReleaseVisitorConfig : IEntityTypeConfiguration<ProjectReleaseVisitor>
{
    public void Configure(EntityTypeBuilder<ProjectReleaseVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectReleaseVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

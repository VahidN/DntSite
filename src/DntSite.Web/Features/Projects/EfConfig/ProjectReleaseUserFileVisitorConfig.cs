using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectReleaseUserFileVisitorConfig : IEntityTypeConfiguration<ProjectReleaseUserFileVisitor>
{
    public void Configure(EntityTypeBuilder<ProjectReleaseUserFileVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectReleaseUserFileVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

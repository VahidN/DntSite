using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectVisitorConfig : IEntityTypeConfiguration<ProjectVisitor>
{
    public void Configure(EntityTypeBuilder<ProjectVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

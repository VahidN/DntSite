using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssueTypeConfig : IEntityTypeConfiguration<ProjectIssueType>
{
    public void Configure(EntityTypeBuilder<ProjectIssueType> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities)
            .WithOne(projectIssue => projectIssue.IssueType)
            .HasForeignKey(projectIssue => projectIssue.IssueTypeId)
            .IsRequired(false);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssueTypes)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

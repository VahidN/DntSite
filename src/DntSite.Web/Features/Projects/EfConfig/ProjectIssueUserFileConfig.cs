using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectIssueUserFileConfig : IEntityTypeConfiguration<ProjectIssueUserFile>
{
    public void Configure(EntityTypeBuilder<ProjectIssueUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectIssueUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectReleaseUserFileConfig : IEntityTypeConfiguration<ProjectReleaseUserFile>
{
    public void Configure(EntityTypeBuilder<ProjectReleaseUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectReleaseUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

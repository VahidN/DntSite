using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectUserFileConfig : IEntityTypeConfiguration<ProjectUserFile>
{
    public void Configure(EntityTypeBuilder<ProjectUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

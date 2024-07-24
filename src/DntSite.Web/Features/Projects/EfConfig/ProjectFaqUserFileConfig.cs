using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.EfConfig;

public class ProjectFaqUserFileConfig : IEntityTypeConfiguration<ProjectFaqUserFile>
{
    public void Configure(EntityTypeBuilder<ProjectFaqUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.ProjectFaqUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogUserFileConfig : IEntityTypeConfiguration<BacklogUserFile>
{
    public void Configure(EntityTypeBuilder<BacklogUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

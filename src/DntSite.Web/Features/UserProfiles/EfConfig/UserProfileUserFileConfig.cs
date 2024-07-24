using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserProfileUserFileConfig : IEntityTypeConfiguration<UserProfileUserFile>
{
    public void Configure(EntityTypeBuilder<UserProfileUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.UserProfileUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

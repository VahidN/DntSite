using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.EfConfig;

public class PrivateMessageUserFileConfig : IEntityTypeConfiguration<PrivateMessageUserFile>
{
    public void Configure(EntityTypeBuilder<PrivateMessageUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.PrivateMessageUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

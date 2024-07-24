using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.EfConfig;

public class PrivateMessageConfig : IEntityTypeConfiguration<PrivateMessage>
{
    public void Configure(EntityTypeBuilder<PrivateMessage> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.Title).HasMaxLength(450).IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SentPrivateMessages)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        builder.HasOne(privateMessage => privateMessage.ToUser)
            .WithMany(user => user.ReceivedPrivateMessages)
            .HasForeignKey(privateMessage => privateMessage.ToUserId)
            .IsRequired();
    }
}

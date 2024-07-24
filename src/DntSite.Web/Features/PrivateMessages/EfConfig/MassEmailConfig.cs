using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.EfConfig;

public class MassEmailConfig : IEntityTypeConfiguration<MassEmail>
{
    public void Configure(EntityTypeBuilder<MassEmail> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.NewsTitle).HasMaxLength(450).IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.MassEmails)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

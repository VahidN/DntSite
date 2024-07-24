using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.EfConfig;

public class PrivateMessageTagConfig : IEntityTypeConfiguration<PrivateMessageTag>
{
    public void Configure(EntityTypeBuilder<PrivateMessageTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.PrivateMessageTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

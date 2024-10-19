using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.EfConfig;

public class SiteUrlConfig : IEntityTypeConfiguration<SiteUrl>
{
    public void Configure(EntityTypeBuilder<SiteUrl> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.Title).HasMaxLength(maxLength: 1500).IsRequired();
        builder.Property(entity => entity.Url).HasMaxLength(maxLength: 1500).IsRequired();
        builder.Property(entity => entity.UrlHash).HasMaxLength(maxLength: 50).IsRequired();

        builder.HasIndex(entity => entity.UrlHash).IsUnique();
        builder.HasIndex(entity => entity.Url);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SiteUrls)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(required: false);
    }
}

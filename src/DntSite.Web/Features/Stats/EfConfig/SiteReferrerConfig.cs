using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.EfConfig;

public class SiteReferrerConfig : IEntityTypeConfiguration<SiteReferrer>
{
    public void Configure(EntityTypeBuilder<SiteReferrer> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(entity => entity.ReferrerTitle).HasMaxLength(maxLength: 1500).IsRequired();
        builder.Property(entity => entity.ReferrerUrl).HasMaxLength(maxLength: 1500).IsRequired();
        builder.Property(entity => entity.VisitHash).HasMaxLength(maxLength: 50).IsRequired();

        builder.HasIndex(entity => entity.VisitHash).IsUnique();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SiteReferrers)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(required: false);

        builder.HasOne(entity => entity.DestinationSiteUrl)
            .WithMany(siteUrl => siteUrl.SiteReferrers)
            .HasForeignKey(entity => entity.DestinationSiteUrlId)
            .IsRequired(required: false);
    }
}

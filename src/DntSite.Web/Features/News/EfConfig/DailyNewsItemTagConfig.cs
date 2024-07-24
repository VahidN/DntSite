using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemTagConfig : IEntityTypeConfiguration<DailyNewsItemTag>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItemTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemTagConfig : IEntityTypeConfiguration<SearchItemTag>
{
    public void Configure(EntityTypeBuilder<SearchItemTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItemTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

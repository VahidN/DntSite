using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemConfig : IEntityTypeConfiguration<SearchItem>
{
    public void Configure(EntityTypeBuilder<SearchItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItems)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

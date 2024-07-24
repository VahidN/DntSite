using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemVisitorConfig : IEntityTypeConfiguration<SearchItemVisitor>
{
    public void Configure(EntityTypeBuilder<SearchItemVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItemVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

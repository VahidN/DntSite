using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemUserFileVisitorConfig : IEntityTypeConfiguration<SearchItemUserFileVisitor>
{
    public void Configure(EntityTypeBuilder<SearchItemUserFileVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItemUserFileVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemCommentVisitorConfig : IEntityTypeConfiguration<SearchItemCommentVisitor>
{
    public void Configure(EntityTypeBuilder<SearchItemCommentVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItemCommentVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

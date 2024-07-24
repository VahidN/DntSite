using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementCommentVisitorConfig : IEntityTypeConfiguration<AdvertisementCommentVisitor>
{
    public void Configure(EntityTypeBuilder<AdvertisementCommentVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(commentVisitor => commentVisitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(commentVisitor => commentVisitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementCommentVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

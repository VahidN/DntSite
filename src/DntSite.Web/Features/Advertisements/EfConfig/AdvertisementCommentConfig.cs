using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementCommentConfig : IEntityTypeConfiguration<AdvertisementComment>
{
    public void Configure(EntityTypeBuilder<AdvertisementComment> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(comment => comment.Body).IsRequired();

        builder.HasOne(advertisementComment => advertisementComment.Parent)
            .WithMany(advertisement => advertisement.Comments)
            .HasForeignKey(advertisementComment => advertisementComment.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementComments)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        // Self Referencing Entity
        builder.HasIndex(comment => comment.ReplyId);

        // By convention, a property whose CLR type can contain null will be configured as `optional`.
        builder.HasOne(comment => comment.Reply)
            .WithMany(advertisementComment => advertisementComment.Children)
            .HasForeignKey(comment => comment.ReplyId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.PrivateMessages.Entities;

namespace DntSite.Web.Features.PrivateMessages.EfConfig;

public class PrivateMessageVisitorConfig : IEntityTypeConfiguration<PrivateMessageVisitor>
{
    public void Configure(EntityTypeBuilder<PrivateMessageVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.PrivateMessageVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

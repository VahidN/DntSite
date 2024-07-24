using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.EfConfig;

public class UserProfileVisitorConfig : IEntityTypeConfiguration<UserProfileVisitor>
{
    public void Configure(EntityTypeBuilder<UserProfileVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.UserVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

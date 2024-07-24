using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogUserFileVisitorConfig : IEntityTypeConfiguration<BacklogUserFileVisitor>
{
    public void Configure(EntityTypeBuilder<BacklogUserFileVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogUserFileVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

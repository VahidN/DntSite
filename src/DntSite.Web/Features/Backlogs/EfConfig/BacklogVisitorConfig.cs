using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogVisitorConfig : IEntityTypeConfiguration<BacklogVisitor>
{
    public void Configure(EntityTypeBuilder<BacklogVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(backlog => backlog.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

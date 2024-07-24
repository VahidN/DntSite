using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemVisitorConfig : IEntityTypeConfiguration<DailyNewsItemVisitor>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItemVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.EfConfig;

public class LearningPathVisitorConfig : IEntityTypeConfiguration<LearningPathVisitor>
{
    public void Configure(EntityTypeBuilder<LearningPathVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.LearningPathVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

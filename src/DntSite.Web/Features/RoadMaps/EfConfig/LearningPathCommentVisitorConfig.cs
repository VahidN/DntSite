using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.EfConfig;

public class LearningPathCommentVisitorConfig : IEntityTypeConfiguration<LearningPathCommentVisitor>
{
    public void Configure(EntityTypeBuilder<LearningPathCommentVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.LearningPathCommentVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

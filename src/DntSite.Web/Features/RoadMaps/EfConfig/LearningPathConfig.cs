using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.EfConfig;

public class LearningPathConfig : IEntityTypeConfiguration<LearningPath>
{
    public void Configure(EntityTypeBuilder<LearningPath> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.LearningPaths)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

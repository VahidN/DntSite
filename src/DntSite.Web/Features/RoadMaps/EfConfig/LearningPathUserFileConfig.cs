using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.EfConfig;

public class LearningPathUserFileConfig : IEntityTypeConfiguration<LearningPathUserFile>
{
    public void Configure(EntityTypeBuilder<LearningPathUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.LearningPathUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

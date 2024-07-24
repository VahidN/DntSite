using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.EfConfig;

public class LearningPathTagConfig : IEntityTypeConfiguration<LearningPathTag>
{
    public void Configure(EntityTypeBuilder<LearningPathTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.LearningPathTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

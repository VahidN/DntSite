using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyTagConfig : IEntityTypeConfiguration<SurveyTag>
{
    public void Configure(EntityTypeBuilder<SurveyTag> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(tag => tag.Name).HasMaxLength(450).IsRequired();
        builder.HasIndex(tag => tag.Name).IsUnique();

        builder.HasMany(entity => entity.AssociatedEntities).WithMany(@base => @base.Tags);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SurveyTags)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

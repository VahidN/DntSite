using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyConfig : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.Surveys)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

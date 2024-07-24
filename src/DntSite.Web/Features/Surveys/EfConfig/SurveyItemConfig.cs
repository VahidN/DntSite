using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyItemConfig : IEntityTypeConfiguration<SurveyItem>
{
    public void Configure(EntityTypeBuilder<SurveyItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(surveyItem => surveyItem.Survey)
            .WithMany(survey => survey.SurveyItems)
            .HasForeignKey(surveyItem => surveyItem.SurveyId)
            .IsRequired();

        builder.HasMany(surveyItem => surveyItem.Users).WithMany(user => user.SurveyItems);

        builder.Ignore(surveyItem => surveyItem.User);
        builder.Ignore(surveyItem => surveyItem.UserId);
    }
}

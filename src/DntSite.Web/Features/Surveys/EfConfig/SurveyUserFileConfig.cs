using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyUserFileConfig : IEntityTypeConfiguration<SurveyUserFile>
{
    public void Configure(EntityTypeBuilder<SurveyUserFile> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.Parent)
            .WithMany(file => file.UserFiles)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SurveyUserFiles)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

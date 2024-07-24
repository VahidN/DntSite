using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyUserFileVisitorConfig : IEntityTypeConfiguration<SurveyUserFileVisitor>
{
    public void Configure(EntityTypeBuilder<SurveyUserFileVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SurveyUserFileVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

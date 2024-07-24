using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyVisitorConfig : IEntityTypeConfiguration<SurveyVisitor>
{
    public void Configure(EntityTypeBuilder<SurveyVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SurveyVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

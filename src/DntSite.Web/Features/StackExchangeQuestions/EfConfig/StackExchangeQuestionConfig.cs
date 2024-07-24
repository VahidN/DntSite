using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.EfConfig;

public class StackExchangeQuestionConfig : IEntityTypeConfiguration<StackExchangeQuestion>
{
    public void Configure(EntityTypeBuilder<StackExchangeQuestion> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.StackExchangeQuestions)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(required: false);
    }
}

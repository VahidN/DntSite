using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.EfConfig;

public class StackExchangeQuestionVisitorConfig : IEntityTypeConfiguration<StackExchangeQuestionVisitor>
{
    public void Configure(EntityTypeBuilder<StackExchangeQuestionVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.StackExchangeQuestionVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.EfConfig;

public class StackExchangeQuestionCommentConfig : IEntityTypeConfiguration<StackExchangeQuestionComment>
{
    public void Configure(EntityTypeBuilder<StackExchangeQuestionComment> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(comment => comment.Body).IsRequired();

        builder.HasOne(entity => entity.Parent)
            .WithMany(@base => @base.Comments)
            .HasForeignKey(entity => entity.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.StackExchangeQuestionComments)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);

        // Self Referencing Entity
        builder.HasIndex(comment => comment.ReplyId);

        // By convention, a property whose CLR type can contain null will be configured as `optional`.
        builder.HasOne(comment => comment.Reply)
            .WithMany(entity => entity.Children)
            .HasForeignKey(comment => comment.ReplyId)
            .IsRequired(false);
    }
}

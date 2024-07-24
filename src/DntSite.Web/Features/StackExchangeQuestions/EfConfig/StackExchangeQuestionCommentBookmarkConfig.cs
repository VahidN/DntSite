using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.EfConfig;

public class StackExchangeQuestionCommentBookmarkConfig : IEntityTypeConfiguration<StackExchangeQuestionCommentBookmark>
{
    public void Configure(EntityTypeBuilder<StackExchangeQuestionCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.StackExchangeQuestionCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

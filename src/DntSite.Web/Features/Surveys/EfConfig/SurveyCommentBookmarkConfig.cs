using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyCommentBookmarkConfig : IEntityTypeConfiguration<SurveyCommentBookmark>
{
    public void Configure(EntityTypeBuilder<SurveyCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SurveyCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

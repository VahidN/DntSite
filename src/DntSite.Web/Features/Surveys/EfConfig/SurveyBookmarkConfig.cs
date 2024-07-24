using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.EfConfig;

public class SurveyBookmarkConfig : IEntityTypeConfiguration<SurveyBookmark>
{
    public void Configure(EntityTypeBuilder<SurveyBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SurveyBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}

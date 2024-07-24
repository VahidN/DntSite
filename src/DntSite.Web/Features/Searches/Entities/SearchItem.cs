using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Searches.Entities;

public class SearchItem : BaseInteractiveEntity<SearchItem, SearchItemVisitor, SearchItemBookmark, SearchItemReaction,
    SearchItemTag, SearchItemComment, SearchItemCommentVisitor, SearchItemCommentBookmark, SearchItemCommentReaction,
    SearchItemUserFile, SearchItemUserFileVisitor>
{
    [MaxLength] public required string Text { set; get; }
}

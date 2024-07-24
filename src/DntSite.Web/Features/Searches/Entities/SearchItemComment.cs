using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Searches.Entities;

public class SearchItemComment : BaseCommentsEntity<SearchItemComment, SearchItem, SearchItemCommentVisitor,
    SearchItemCommentBookmark, SearchItemCommentReaction>
{
}

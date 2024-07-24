using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Advertisements.Entities;

public class Advertisement : BaseInteractiveEntity<Advertisement, AdvertisementVisitor, AdvertisementBookmark,
    AdvertisementReaction, AdvertisementTag, AdvertisementComment, AdvertisementCommentVisitor,
    AdvertisementCommentBookmark, AdvertisementCommentReaction, AdvertisementUserFile, AdvertisementUserFileVisitor>
{
    public required string Title { set; get; }

    [MaxLength] public required string Body { set; get; }

    public DateTime? DueDate { set; get; }
}

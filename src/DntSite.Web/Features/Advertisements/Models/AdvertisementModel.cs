using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.Models;

public class AdvertisementModel
{
    public Advertisement? CurrentItem { set; get; }

    public Advertisement? NextItem { set; get; }

    public Advertisement? PreviousItem { set; get; }

    public IList<AdvertisementComment> CommentsList { set; get; } = [];
}

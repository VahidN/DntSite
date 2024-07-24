using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class AdvertisementDueDate
{
    [Parameter] [EditorRequired] public Advertisement? Advertisement { set; get; }
}

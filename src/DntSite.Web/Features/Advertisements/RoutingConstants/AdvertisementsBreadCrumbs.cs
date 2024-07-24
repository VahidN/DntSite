using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Advertisements.RoutingConstants;

public static class AdvertisementsBreadCrumbs
{
    public static readonly BreadCrumb JobOfferBreadCrumb = new()
    {
        Title = "ارسال آگهی",
        Url = AdvertisementsRoutingConstants.WriteAdvertisementJobOffer,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb AdvertisementsTagBreadCrumb = new()
    {
        Title = "گروه‌های آگهی‌ها",
        Url = AdvertisementsRoutingConstants.AdvertisementsTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb AdvertisementsWritersBreadCrumb = new()
    {
        Title = "نویسنده‌های آگهی‌ها",
        Url = AdvertisementsRoutingConstants.AdvertisementsWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb AdvertisementsCommentsBreadCrumb = new()
    {
        Title = "نظرات آگهی‌ها",
        Url = AdvertisementsRoutingConstants.AdvertisementsComments,
        GlyphIcon = DntBootstrapIcons.BiWechat
    };

    public static readonly BreadCrumb AdvertisementsBreadCrumb = new()
    {
        Title = "آگهی‌ها",
        Url = AdvertisementsRoutingConstants.Advertisements,
        GlyphIcon = DntBootstrapIcons.BiPeopleFill
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        JobOfferBreadCrumb, AdvertisementsWritersBreadCrumb, AdvertisementsTagBreadCrumb,
        AdvertisementsCommentsBreadCrumb, AdvertisementsBreadCrumb
    ];
}

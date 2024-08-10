namespace DntSite.Web.Features.Advertisements.RoutingConstants;

public static class AdvertisementsRoutingConstants
{
    public const string Announcements = "/announcements";

    public const string Advertisements = "/advertisements";
    public const string AdvertisementsPage = $"{Advertisements}/page/{{CurrentPage:int}}";

    public const string AdvertisementsFilterBase = $"{Advertisements}/filter";
    public const string AdvertisementsFilter = $"{AdvertisementsFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string AdvertisementsDetailsBase = $"{Advertisements}/details";
    public const string AdvertisementsDetails = $"{AdvertisementsDetailsBase}/{{AdvertisementId:int}}";
    public const string CommentsUrlTemplate = $"{AdvertisementsDetailsBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{AdvertisementsDetailsBase}/{{0}}";

    public const string AdvertisementsOldDetails = "/announcements/details/{AdvertisementId:int}";

    public const string AdvertisementsTag = "/advertisements-tag";
    public const string PostTagUrlTemplate = $"{AdvertisementsTag}/{{0}}";
    public const string AdvertisementsTagPage = $"{AdvertisementsTag}/page/{{CurrentPage:int}}";
    public const string AdvertisementsTagName = $"{AdvertisementsTag}/{{TagName}}";
    public const string AdvertisementsTagNamePage = $"{AdvertisementsTag}/{{TagName}}/page/{{CurrentPage:int}}";

    public const string AdvertisementsWriters = "/advertisements-writers";
    public const string AdvertisementsWritersPage = $"{AdvertisementsWriters}/page/{{CurrentPage:int}}";
    public const string AdvertisementsWritersName = $"{AdvertisementsWriters}/{{UserFriendlyName}}";

    public const string AdvertisementsWritersNamePage =
        $"{AdvertisementsWriters}/{{UserFriendlyName}}/page/{{CurrentPage:int}}";

    public const string AdvertisementsComments = "/advertisements-comments";
    public const string AdvertisementsCommentsPage = $"{AdvertisementsComments}/page/{{CurrentPage:int}}";
    public const string AdvertisementsCommentsName = $"{AdvertisementsComments}/{{UserFriendlyName}}";

    public const string AdvertisementsCommentsNamePage =
        $"{AdvertisementsComments}/{{UserFriendlyName}}/page/{{CurrentPage:int}}";

    public const string WriteAdvertisementBase = "/write-advertisement";
    public const string WriteAdvertisementTypeBase = $"{WriteAdvertisementBase}/type";
    public const string WriteAdvertisementJobOffer = $"{WriteAdvertisementTypeBase}/JobOffer";
    public const string WriteNewProjectAdvertisement = $"{WriteAdvertisementTypeBase}/NewProject";
    public const string WriteSpecialAdvertisement = $"{WriteAdvertisementTypeBase}/Special";
    public const string WriteAdvertisementType = $"{WriteAdvertisementTypeBase}/{{AdvertisementKind}}";
    public const string WriteAdvertisementEditBase = $"{WriteAdvertisementBase}/edit";

    public const string WriteAdvertisementEdit =
        $"{WriteAdvertisementEditBase}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string EditPostUrlTemplate = $"{WriteAdvertisementEditBase}/{{0}}";

    public const string WriteAdvertisementDeleteBase = $"{WriteAdvertisementBase}/delete";

    public const string WriteAdvertisementDelete =
        $"{WriteAdvertisementDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string DeletePostUrlTemplate = $"{WriteAdvertisementDeleteBase}/{{0}}";
}

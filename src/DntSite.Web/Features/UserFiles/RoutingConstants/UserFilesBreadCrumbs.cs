using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.UserFiles.RoutingConstants;

public static class UserFilesBreadCrumbs
{
    public static readonly BreadCrumb Files = new()
    {
        Title = "مدیریت فایل‌های ارسالی",
        Url = UserFilesRoutingConstants.FilesManagerFilesUrl,
        GlyphIcon = DntBootstrapIcons.BiFiles
    };

    public static readonly BreadCrumb Images = new()
    {
        Title = "مدیریت تصاویر ارسالی",
        Url = UserFilesRoutingConstants.FilesManagerImagesUrl,
        GlyphIcon = DntBootstrapIcons.BiFileImage
    };

    public static readonly BreadCrumb UserFilesBookmarksBreadCrumb = new()
    {
        Title = "علاقمندی‌های شخصی",
        Url = UserFilesRoutingConstants.FilesManagerBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [Files, Images];
}

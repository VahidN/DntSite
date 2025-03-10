using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.RoutingConstants;

namespace DntSite.Web.Features.AppConfigs.Services;

public class AppAntiXssService(
    IAntiXssService antiXssService,
    IAppFoldersService appFoldersService,
    IHttpContextAccessor httpContextAccessor) : IAppAntiXssService
{
    public string GetSanitizedHtml(string? html)
    {
        var httpContext = httpContextAccessor.HttpContext;

        var baseUri = httpContext?.GetBaseUri();
        var baseUrl = httpContext?.GetBaseUrl();

        var rules = new HtmlModificationRules
        {
            ConvertPToDiv = true,
            RemoveRelAndTargetFromInternalUrls = true,
            HostUri = baseUri,
            RemoveConsecutiveEmptyLines = true,
            MaxAllowedConsecutiveEmptyLines = 2
        };

        if (baseUrl.IsEmpty() || baseUri is null)
        {
            return antiXssService.GetSanitizedHtml(html, htmlModificationRules: rules);
        }

        return antiXssService.GetSanitizedHtml(html, remoteImagesOptions: new FixRemoteImagesOptions
        {
            OutputImageFolder = appFoldersService.ArticleImagesFolderPath,
            HostUri = baseUri,
            ImageUrlBuilder = savedFileName
                => baseUrl.CombineUrl(
                    $"{ApiUrlsRoutingConstants.File.HttpAny.Image}?name={Uri.EscapeDataString(savedFileName)}",
                    escapeRelativeUrl: false)
        }, htmlModificationRules: rules);
    }
}

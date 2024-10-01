using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Posts.RoutingConstants;

namespace DntSite.Web.Features.Seo.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]")]
public class OpenSearchController(
    IHttpRequestInfoService httpRequestInfoService,
    ICachedAppSettingsProvider appSettingsService) : ControllerBase
{
    [HttpGet(template: "[action]")]
    public async Task<IActionResult> RenderOpenSearch()
    {
        var appSettings = await appSettingsService.GetAppSettingsAsync();
        var shortName = appSettings?.BlogName ?? "DNT";
        var baseUrl = appSettings?.SiteRootUri ?? httpRequestInfoService.GetBaseUrl() ?? "/";

        var searchUrlTemplate =
            baseUrl.CombineUrl(
                $"{PostsRoutingConstants.PostsFilterFilterBase}/{Uri.EscapeDataString(stringToEscape: "title={{searchTerms}}")}");

        return new OpenSearchResult
        {
            ShortName = shortName,
            Description = $"{shortName} Contents Search",
            SearchForm = baseUrl,
            FavIconUrl = baseUrl.CombineUrl(relativeUrl: "favicon.ico"),
            SearchUrlTemplate = searchUrlTemplate
        };
    }
}

using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Services.Contracts;
using Microsoft.AspNetCore.OutputCaching;

namespace DntSite.Web.Features.Seo.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]")]
public class SitemapController(IBlogPostsService blogPostsService) : ControllerBase
{
    [OutputCache(Duration = TimeConstants.Minute * 15, PolicyName = AlwaysCachePolicy.Name)]
    [Microsoft.AspNetCore.Mvc.Route(template: "/sitemap.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/sitemap")]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public async Task<IActionResult> Get()
    {
        var items = await blogPostsService.GetLastBlogPostsIncludeAuthorTagsAsync(count: 15);

        var sitemapItems = items.Where(item => !IsPrivate(item))
            .Select(item => new SitemapItem
            {
                LastUpdatedTime = item.Audit.CreatedAt.ToDateTimeOffset(),
                Url = HttpContext.GetBaseUrl()
                    .CombineUrl(string.Create(CultureInfo.InvariantCulture, $"/post/{item.Id}"),
                        escapeRelativeUrl: false)
            })
            .ToList();

        return new SitemapResult(sitemapItems);
    }

    private static bool IsPrivate(BlogPost item) => item.NumberOfRequiredPoints is > 0;
}

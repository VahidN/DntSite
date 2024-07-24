using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Seo.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class SitemapController(IBlogPostsService blogPostsService) : ControllerBase
{
    public async Task<IActionResult> Get()
    {
        var items = await blogPostsService.GetLastBlogPostsIncludeAuthorTagsAsync(15);

        var sitemapItems = items.Where(item => !IsPrivate(item))
            .Select(item => new SitemapItem
            {
                LastUpdatedTime = item.Audit.CreatedAt.ToDateTimeOffset(),
                Url = HttpContext.GetBaseUrl().CombineUrl(Invariant($"/post/{item.Id}"))
            })
            .ToList();

        return new SitemapResult(sitemapItems);
    }

    private static bool IsPrivate(BlogPost item) => item.NumberOfRequiredPoints is > 0;
}

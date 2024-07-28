using DntSite.Web.Features.RssFeeds.Services.Contracts;
using Microsoft.AspNetCore.OutputCaching;

namespace DntSite.Web.Features.RssFeeds.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]/[action]")]
public class ProjectsFeedsController(IFeedsService feedsService) : ControllerBase
{
    private const int Min15 = TimeConstants.Minute * 15;

    [OutputCache(Duration = Min15)] public Task<IActionResult> Index() => ProjectsNews();

    [OutputCache(Duration = Min15)] public Task<IActionResult> Get() => ProjectsNews();

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> ProjectsNews() => new FeedResult(await feedsService.GetProjectsNewsAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> ProjectsFiles() => new FeedResult(await feedsService.GetProjectsFilesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> ProjectsIssues() => new FeedResult(await feedsService.GetProjectsIssuesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> ProjectsIssuesReplies()
        => new FeedResult(await feedsService.GetProjectsIssuesRepliesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> ProjectsFaqs() => new FeedResult(await feedsService.GetProjectsFaqsAsync());

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int}")]
    public async Task<IActionResult> ProjectFaqs(int? id)
    {
        var (items, _) = await feedsService.GetProjectFaqsAsync(id);

        if (items is null)
        {
            return BadRequest();
        }

        return new FeedResult(items);
    }

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int}")]
    public async Task<IActionResult> ProjectFiles(int? id)
    {
        var (items, _) = await feedsService.GetProjectFilesAsync(id);

        if (items is null)
        {
            return BadRequest();
        }

        return new FeedResult(items);
    }

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int}")]
    public async Task<IActionResult> ProjectIssues(int? id)
    {
        var (items, _) = await feedsService.GetProjectIssuesAsync(id);

        if (items is null)
        {
            return BadRequest();
        }

        return new FeedResult(items);
    }

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int}")]
    public async Task<IActionResult> ProjectIssuesReplies(int? id)
    {
        var (items, _) = await feedsService.GetProjectIssuesRepliesAsync(id);

        if (items is null)
        {
            return BadRequest();
        }

        return new FeedResult(items);
    }
}

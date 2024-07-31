using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.RssFeeds.Services.Contracts;
using Microsoft.AspNetCore.OutputCaching;

namespace DntSite.Web.Features.RssFeeds.Controllers;

[ApiController]
[AllowAnonymous]
[OutputCache(Duration = TimeConstants.Minute * 15, PolicyName = AlwaysCachePolicy.Name)]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]/[action]")]
public class ProjectsFeedsController(IFeedsService feedsService) : ControllerBase
{
    public Task<IActionResult> Index() => ProjectsNews();

    public Task<IActionResult> Get() => ProjectsNews();

    public async Task<IActionResult> ProjectsNews()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetProjectsNewsAsync());

    public async Task<IActionResult> ProjectsFiles()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetProjectsFilesAsync());

    public async Task<IActionResult> ProjectsIssues()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetProjectsIssuesAsync());

    public async Task<IActionResult> ProjectsIssuesReplies()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetProjectsIssuesRepliesAsync());

    public async Task<IActionResult> ProjectsFaqs()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetProjectsFaqsAsync());

    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int?}")]
    public async Task<IActionResult> ProjectFaqs(int? id = null)
    {
        var (items, _) = await feedsService.GetProjectFaqsAsync(id);

        if (items is null)
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(items);
    }

    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int?}")]
    public async Task<IActionResult> ProjectFiles(int? id = null)
    {
        var (items, _) = await feedsService.GetProjectFilesAsync(id);

        if (items is null)
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(items);
    }

    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int?}")]
    public async Task<IActionResult> ProjectIssues(int? id = null)
    {
        var (items, _) = await feedsService.GetProjectIssuesAsync(id);

        if (items is null)
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(items);
    }

    [Microsoft.AspNetCore.Mvc.Route(template: "{id:int?}")]
    public async Task<IActionResult> ProjectIssuesReplies(int? id = null)
    {
        var (items, _) = await feedsService.GetProjectIssuesRepliesAsync(id);

        if (items is null)
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(items);
    }
}

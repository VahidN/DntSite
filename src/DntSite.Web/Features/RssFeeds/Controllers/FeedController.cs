using DntSite.Web.Features.RssFeeds.Services.Contracts;
using Microsoft.AspNetCore.OutputCaching;

namespace DntSite.Web.Features.RssFeeds.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]/[action]")]
public class FeedController(IFeedsService feedsService) : ControllerBase
{
    private const int Min15 = TimeConstants.Minute * 15;

    [OutputCache(Duration = Min15)]
    [Microsoft.AspNetCore.Mvc.Route(template: "")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/[controller]")]
    public Task<IActionResult> Index() => Posts();

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Posts() => new FeedResult(await feedsService.GetPostsAsync());

    [OutputCache(Duration = Min15)]
    [Microsoft.AspNetCore.Mvc.Route(template: "/feeds/posts/{name}")]
    public async Task<IActionResult> UserPosts(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest();
        }

        return await Posts();
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Comments() => new FeedResult(await feedsService.GetCommentsAsync());

    [OutputCache(Duration = Min15)]
    [Microsoft.AspNetCore.Mvc.Route(template: "/feeds/comments/{name}")]
    public async Task<IActionResult> UserComments(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest();
        }

        return await Comments();
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> News() => new FeedResult(await feedsService.GetNewsAsync());

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id}")]
    public async Task<IActionResult> Tag(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        return new FeedResult(await feedsService.GetTagAsync(id));
    }

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id}")]
    public async Task<IActionResult> Author(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        return new FeedResult(await feedsService.GetAuthorAsync(id));
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> NewsComments() => new FeedResult(await feedsService.GetNewsCommentsAsync());

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id}")]
    public async Task<IActionResult> NewsAuthor(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        return new FeedResult(await feedsService.GetNewsAuthorAsync(id));
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> LatestChanges() => new FeedResult(await GetLatestChangesAsync());

    [OutputCache(Duration = Min15)]
    [Microsoft.AspNetCore.Mvc.Route(template: "/rss.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/atom.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/rss")]
    public Task<IActionResult> SiteFeed() => LatestChanges();

    private Task<FeedChannel> GetLatestChangesAsync() => feedsService.GetLatestChangesAsync();

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Courses() => new FeedResult(await feedsService.GetAllCoursesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> CoursesTopics() => new FeedResult(await feedsService.GetAllCoursesTopicsAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> CoursesComments()
        => new FeedResult(await feedsService.GetCourseTopicsRepliesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Surveys() => new FeedResult(await feedsService.GetAllVotesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Announcements() => new FeedResult(await feedsService.GetAllAdvertisementsAsync());
}

using DntSite.Web.Features.RssFeeds.Models;
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
    public async Task<IActionResult> Posts() => new FeedResult<WhatsNewItemModel>(await feedsService.GetPostsAsync());

    [OutputCache(Duration = Min15)]
    [Microsoft.AspNetCore.Mvc.Route(template: "/feeds/posts/{name?}")]
    public async Task<IActionResult> UserPosts(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return RedirectToActionPermanent(nameof(Posts));
        }

        return await Posts();
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Comments()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetCommentsAsync());

    [OutputCache(Duration = Min15)]
    [Microsoft.AspNetCore.Mvc.Route(template: "/feeds/comments/{name?}")]
    public async Task<IActionResult> UserComments(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return RedirectToActionPermanent(nameof(Comments));
        }

        return await Comments();
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> News() => new FeedResult<WhatsNewItemModel>(await feedsService.GetNewsAsync());

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id?}")]
    public async Task<IActionResult> Tag(string? id = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(await feedsService.GetTagAsync(id));
    }

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id?}")]
    public async Task<IActionResult> Author(string? id = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(await feedsService.GetAuthorAsync(id));
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> NewsComments()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetNewsCommentsAsync());

    [OutputCache(Duration = Min15, VaryByQueryKeys = ["*"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "{id?}")]
    public async Task<IActionResult> NewsAuthor(string? id = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(await feedsService.GetNewsAuthorAsync(id));
    }

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> LatestChanges()
        => new FeedResult<WhatsNewItemModel>(await GetLatestChangesAsync());

    [OutputCache(Duration = Min15)]
    [Microsoft.AspNetCore.Mvc.Route(template: "/rss.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/atom.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/rss")]
    public Task<IActionResult> SiteFeed() => LatestChanges();

    private Task<WhatsNewFeedChannel> GetLatestChangesAsync() => feedsService.GetLatestChangesAsync();

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Courses()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetAllCoursesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> CoursesTopics()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetAllCoursesTopicsAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> CoursesComments()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetCourseTopicsRepliesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Surveys()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetAllVotesAsync());

    [OutputCache(Duration = Min15)]
    public async Task<IActionResult> Announcements()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetAllAdvertisementsAsync());
}

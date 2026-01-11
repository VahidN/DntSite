using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.RssFeeds.Services.Contracts;
using Microsoft.AspNetCore.OutputCaching;

namespace DntSite.Web.Features.RssFeeds.Controllers;

[ApiController]
[AllowAnonymous]
[OutputCache(Duration = TimeConstants.Minute * 15, PolicyName = AlwaysCachePolicy.Name)]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]/[action]")]
public class FeedController(IFeedsService feedsService, ICachedAppSettingsProvider cachedAppSettingsProvider)
    : ControllerBase
{
    [Microsoft.AspNetCore.Mvc.Route(template: "")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/[controller]")]
    public Task<IActionResult> Index() => Posts();

    public async Task<IActionResult> Posts()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetPostsAsync(await ShowBriefDescriptionAsync()));

    [Microsoft.AspNetCore.Mvc.Route(template: "/feeds/posts/{name?}")]
    public async Task<IActionResult> UserPosts(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return RedirectToActionPermanent(nameof(Posts));
        }

        return await Posts();
    }

    public async Task<IActionResult> Comments()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetCommentsAsync(await ShowBriefDescriptionAsync()));

    [Microsoft.AspNetCore.Mvc.Route(template: "/feeds/comments/{name?}")]
    public async Task<IActionResult> UserComments(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return RedirectToActionPermanent(nameof(Comments));
        }

        return await Comments();
    }

    public async Task<IActionResult> News()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetNewsAsync(await ShowBriefDescriptionAsync()));

    [Microsoft.AspNetCore.Mvc.Route(template: "{id?}")]
    public async Task<IActionResult> Tag(string? id = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(await feedsService.GetTagAsync(await ShowBriefDescriptionAsync(), id));
    }

    [Microsoft.AspNetCore.Mvc.Route(template: "{id?}")]
    public async Task<IActionResult> Author(string? id = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(
            await feedsService.GetAuthorAsync(await ShowBriefDescriptionAsync(), id));
    }

    public async Task<IActionResult> NewsComments()
        => new FeedResult<WhatsNewItemModel>(
            await feedsService.GetNewsCommentsAsync(await ShowBriefDescriptionAsync()));

    [Microsoft.AspNetCore.Mvc.Route(template: "{id?}")]
    public async Task<IActionResult> NewsAuthor(string? id = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        return new FeedResult<WhatsNewItemModel>(
            await feedsService.GetNewsAuthorAsync(await ShowBriefDescriptionAsync(), id));
    }

    public async Task<IActionResult> LatestChanges()
        => new FeedResult<WhatsNewItemModel>(await GetLatestChangesAsync());

    [Microsoft.AspNetCore.Mvc.Route(template: "/rss.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/atom.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/rss")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/rss2.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/feed.xml")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/feed/rss")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/feed/atom")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/blog/feed")]
    [Microsoft.AspNetCore.Mvc.Route(template: "/blog/rss.xml")]
    public Task<IActionResult> SiteFeed() => LatestChanges();

    [Microsoft.AspNetCore.Mvc.Route(template: "/llms.txt")]
    public async Task<IActionResult> LlmsTxt() => new LlmsTxtResult<WhatsNewItemModel>(await GetLatestChangesAsync());

    [Microsoft.AspNetCore.Mvc.Route(template: "/llms-full.txt")]
    public async Task<IActionResult> LlmsFull()
        => new LlmsFullTxtResult<WhatsNewItemModel>(await GetLatestChangesAsync());

    private async Task<WhatsNewFeedChannel> GetLatestChangesAsync()
        => await feedsService.GetLatestChangesAsync(await ShowBriefDescriptionAsync());

    public async Task<IActionResult> Courses()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetAllCoursesAsync(await ShowBriefDescriptionAsync()));

    public async Task<IActionResult> CoursesTopics()
        => new FeedResult<WhatsNewItemModel>(
            await feedsService.GetAllCoursesTopicsAsync(await ShowBriefDescriptionAsync()));

    public async Task<IActionResult> CoursesComments()
        => new FeedResult<WhatsNewItemModel>(
            await feedsService.GetCourseTopicsRepliesAsync(await ShowBriefDescriptionAsync()));

    public async Task<IActionResult> Surveys()
        => new FeedResult<WhatsNewItemModel>(await feedsService.GetAllVotesAsync(await ShowBriefDescriptionAsync()));

    public async Task<IActionResult> Announcements()
        => new FeedResult<WhatsNewItemModel>(
            await feedsService.GetAllAdvertisementsAsync(await ShowBriefDescriptionAsync()));

    private async Task<bool> ShowBriefDescriptionAsync()
    {
        var appSetting = await cachedAppSettingsProvider.GetAppSettingsAsync();

        return appSetting.ShowRssBriefDescription;
    }
}

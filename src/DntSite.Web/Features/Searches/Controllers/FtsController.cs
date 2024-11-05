using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.Searches.ModelsMappings;
using DntSite.Web.Features.Searches.RoutingConstants;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "api/[controller]")]
public class FtsController(IFullTextSearchService fullTextSearchService, ISearchItemsService searchItemsService)
    : ControllerBase
{
    private const int ItemsPerPage = 12;

    private const string ItemNotFound =
        $"<span class='dropdown-item bg-light-subtle'><i class='{DntBootstrapIcons.BiXCircle} me-2'></i>اطلاعاتی یافت نشد</span>";

    [HttpGet(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public IActionResult Search([FromQuery] string? searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            return Ok(Array.Empty<string>());
        }

        var results = fullTextSearchService.FindPagedPosts(searchQuery, ItemsPerPage, pageNumber: 1, ItemsPerPage);

        if (results.TotalItems == 0)
        {
            return Ok(new[]
            {
                ItemNotFound
            });
        }

        var items = results.Data.OrderBy(result => result.ItemType.Value)
            .ThenByDescending(result => result.Score)
            .Select(result => result.MapToTemplatedResult(searchQuery))
            .ToList();

        items.Add(GetMoreItemsLink(searchQuery));

        return Ok(items);
    }

    [HttpPost(template: "[action]")]
    [HttpGet(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Log([FromBody] string? searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return BadRequest();
        }

        await searchItemsService.SaveSearchItemAsync(searchValue);

        return Ok();
    }

    private static string GetMoreItemsLink(string text)
        => $"""
            <a class='dropdown-item'
               rel='noopener noreferrer'
               href='{"/".CombineUrl(SearchesRoutingConstants.SearchResultsBase, escapeRelativeUrl: false).CombineUrl(text, escapeRelativeUrl: true)}'>
               بیشتر ...
               <i class='{DntBootstrapIcons.BiArrowLeftCircleFill} ms-1'></i>
            <a/>
            """;
}

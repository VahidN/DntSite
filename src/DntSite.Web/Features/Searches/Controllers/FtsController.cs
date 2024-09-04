using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.Searches.ModelsMappings;
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
        $"<span class='alert alert-warning'><i class='{DntBootstrapIcons.BiXCircle} me-2'></i>اطلاعاتی یافت نشد</span>";

    [HttpGet(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public IActionResult Search([FromQuery] string? searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            return Ok(Array.Empty<string>());
        }

        var results = fullTextSearchService.FindPagedPosts(searchQuery, ItemsPerPage, pageNumber: 1, ItemsPerPage);

        return results.TotalItems == 0
            ? Ok(new[]
            {
                ItemNotFound
            })
            : Ok(results.Data.OrderBy(result => result.ItemType.Value)
                .ThenByDescending(result => result.Score)
                .Select(result => result.MapToTemplatedResult(searchQuery))
                .ToList());
    }

    [HttpPost(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Log([FromBody] string? searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return BadRequest();
        }

        await searchItemsService.AddSearchItemAsync(searchValue);

        return Ok();
    }
}

using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Posts.Components;

public partial class PostsArchive
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<BlogPost>? _blogPosts;

    [InjectComponentScoped] internal IBlogPostsService BlogPostsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    [Parameter] [SupplyParameterFromQuery] public string? Term { get; set; }

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrWhiteSpace(Term))
        {
            ActivateOldSearchPattern();
        }
        else
        {
            await ShowBlogPostsListAsync(Filter);
            AddBreadCrumbs();
        }
    }

    private void ActivateOldSearchPattern()
    {
        var searchFilter = Uri.EscapeDataString($"(Title =* {Term})");
        ApplicationState.NavigateTo($"{PostsRoutingConstants.PostsFilterFilterBase}/{searchFilter}/page/1#main");
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..PostsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await SearchItemsService.AddSearchItemAsync(gridifyFilter);
        await ShowBlogPostsListAsync(gridifyFilter);
        StateHasChanged();
    }

    private async Task ShowBlogPostsListAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{PostsRoutingConstants.PostsFilterFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _blogPosts = await BlogPostsService.GetLastBlogPostsIncludeAuthorsTagsAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(BlogPost.Id)
        });
    }
}

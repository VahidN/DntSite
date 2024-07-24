using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;

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

    protected override async Task OnInitializedAsync()
    {
        await ShowBlogPostsListAsync(Filter);
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..PostsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
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

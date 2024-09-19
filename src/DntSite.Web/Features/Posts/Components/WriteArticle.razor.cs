using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.Components;

public partial class WriteArticle
{
    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [InjectComponentScoped] internal IBlogPostsService BlogPostsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteArticle))]
    public WriteArticleModel WriteArticleModel { get; set; } = new();

    [Inject] internal IMapper Mapper { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);

        await FillPossibleEditFormAsync();
        await PerformPossibleDeleteAsync();

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..PostsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        var post = await BlogPostsService.FindBlogPostIncludeTagsAsync(EditId.ToInt());

        if (post is null || !ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        WriteArticleModel = Mapper.Map<BlogPost, WriteArticleModel>(post);
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(DeleteId))
        {
            return;
        }

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        if (ApplicationState.CurrentUser?.IsAdmin == false)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        await BlogPostsService.PerformPossibleDeleteAsync(DeleteId.ToInt());
        ApplicationState.NavigateTo(PostsRoutingConstants.Posts);
    }

    private async Task PerformAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        await BlogPostsService.PerformEditAsync(EditId.ToInt(), WriteArticleModel, ApplicationState);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{PostsRoutingConstants.PostBase}/{EditId}"));
    }
}

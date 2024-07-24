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
    [Parameter] public int? EditId { set; get; }

    [Parameter] public int? DeleteId { set; get; }

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
        if (!EditId.HasValue)
        {
            return;
        }

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        var post = await BlogPostsService.FindBlogPostIncludeTagsAsync(EditId.Value);

        if (post is null || !ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        WriteArticleModel = Mapper.Map<BlogPost, WriteArticleModel>(post);
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (!DeleteId.HasValue)
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

        await BlogPostsService.PerformPossibleDeleteAsync(DeleteId.Value);
        ApplicationState.NavigateTo(PostsRoutingConstants.Root);
    }

    private async Task PerformAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        await BlogPostsService.PerformEditAsync(EditId, WriteArticleModel, ApplicationState);

        ApplicationState.NavigateTo(Invariant($"{PostsRoutingConstants.PostBase}/{EditId}"));
    }
}

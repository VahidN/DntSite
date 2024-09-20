using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.WebToolkit;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.ModelsMappings;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.Components;

public partial class Post
{
    private BlogPostModel? _blogPost;
    private string? _documentTypeIdHash;

    private List<BlogPostComment>? _postComments;

    private BlogPost? CurrentPost => _blogPost?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Body ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IBlogPostsService BlogPostsService { set; get; } = null!;

    [InjectComponentScoped] internal IBlogCommentsService BlogCommentsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? Id { set; get; }

    [Parameter] public string? Slug { set; get; }

    [Parameter] public string? PublishYear { set; get; }

    [Parameter] public string? PublishMonth { set; get; }

    [Parameter] public string? OldTitle { set; get; }

    private bool IsOldBloggerPostUrl => PublishYear.IsNumber() &&
                                        PublishMonth.IsNumber() && !string.IsNullOrWhiteSpace(OldTitle);

    private bool CanUserViewThisPost => ApplicationState.CurrentUser.CanUserViewThisPost(CurrentPost);

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        if (IsOldBloggerPostUrl)
        {
            await ManageOldBloggerLinksAsync();

            return;
        }

        AddBreadCrumbs();

        if (!Id.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _blogPost = await BlogPostsService.GetBlogPostLastAndNextPostIncludeAuthorTagsAsync(Id.Value);

        if (_blogPost.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await GetCommentsAsync(Id.Value);
        await UpdateStatAsync(Id.Value);
        SetSimilarPostsId();
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _blogPost?.CurrentItem?.MapToPostWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash;

    private async Task ManageOldBloggerLinksAsync()
    {
        var oldUrl = ApplicationState.AppSetting?.SiteRootUri.CombineUrl(PublishYear.ToEnglishNumbers())
            .CombineUrl(PublishMonth.ToEnglishNumbers())
            .CombineUrl($"{OldTitle}.html");

        var post = await BlogPostsService.FindBlogPostAsync(oldUrl.ToEnglishNumbers());

        if (post is not null)
        {
            ApplicationState.NavigateTo(
                PostsRoutingConstants.PostBase.CombineUrl(post.Id.ToString(CultureInfo.InvariantCulture)));
        }
        else
        {
            ApplicationState.NavigateToNotFoundPage();
        }
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..PostsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task GetCommentsAsync(int id)
        => _postComments = await BlogCommentsService.GetRootCommentsOfPostAsync(id);

    private async Task UpdateStatAsync(int id)
    {
        if (ApplicationState.HttpContext.IsPostRequest())
        {
            return;
        }

        await BlogPostsService.UpdateStatAsync(id, ApplicationState.NavigationManager.IsFromFeed());
    }

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await BlogCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await BlogCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");

                break;
            case CommentAction.SubmitNewComment:
                await BlogCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId, model.Comment ?? "",
                    ApplicationState.CurrentUser?.UserId ?? 0);

                break;
            case CommentAction.ReplyToComment:
            case CommentAction.Edit:
            case CommentAction.Cancel:
            default:
                break;
        }

        await GetCommentsAsync(model.FormPostId);
        StateHasChanged();
    }
}

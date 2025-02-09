using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.ModelsMappings;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.News.Components;

public partial class NewsArchiveDetails
{
    private string? _documentTypeIdHash;

    private NewsDetailsModel? _news;

    private List<DailyNewsItemComment>? _newsComments;

    [Parameter] public int? NewsId { set; get; }

    private DailyNewsItem? CurrentPost => _news?.CurrentNews;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.BriefDescription ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IDailyNewsItemsService DailyNewsItemsService { set; get; } = null!;

    [InjectComponentScoped] internal IDailyNewsItemCommentsService DailyNewsItemCommentsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private string DeleteScreenshotUrl
        => NewsId.HasValue && CanUserDeleteThisPost && !string.IsNullOrWhiteSpace(CurrentPost?.PageThumbnail)
            ? NewsRoutingConstants.WriteNewsDeleteDeleteScreenshotIdBase.CombineUrl(
                ProtectionProvider.Encrypt(NewsId.Value.ToString(CultureInfo.InvariantCulture)),
                escapeRelativeUrl: true)
            : "";

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (!NewsId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _news = await DailyNewsItemsService.GetNewsLastAndNextIncludeAuthorTagsAsync(NewsId.Value);

        if (_news.CurrentNews is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await GetCommentsAsync(NewsId.Value);
        SetSimilarPostsId();
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _news?.CurrentNews?.MapToNewsWhatsNewItemModel(siteRootUri: "", newsThumbImage: "")
            .DocumentTypeIdHash;

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..NewsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task GetCommentsAsync(int id)
        => _newsComments = await DailyNewsItemCommentsService.GetRootCommentsOfNewsAsync(id);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await DailyNewsItemCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await DailyNewsItemCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");

                break;
            case CommentAction.SubmitNewComment:
                await DailyNewsItemCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId,
                    model.Comment ?? "", ApplicationState.CurrentUser?.UserId ?? 0);

                break;
            case CommentAction.ReplyToComment:
            case CommentAction.Edit:
            case CommentAction.Cancel:
            default:
                return;
        }

        await GetCommentsAsync(model.FormPostId);
        StateHasChanged();
    }
}

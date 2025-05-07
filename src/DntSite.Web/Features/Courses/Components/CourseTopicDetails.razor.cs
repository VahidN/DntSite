using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.ModelsMappings;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Courses.Components;

public partial class CourseTopicDetails
{
    private CourseTopicModel? _courseTopic;

    private List<CourseTopicComment>? _courseTopicComments;

    private string? _documentTypeIdHash;

    [Parameter] public int? CourseId { set; get; }

    [Parameter] public Guid? DisplayId { set; get; }

    private CourseTopic? CurrentPost => _courseTopic?.ThisTopic;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Body ?? "").FirstOrDefault();

    [InjectComponentScoped] internal ICoursesService CoursesService { set; get; } = null!;

    [InjectComponentScoped] internal ICourseTopicsService CourseTopicsService { set; get; } = null!;

    [InjectComponentScoped] internal ICourseTopicCommentsService CourseTopicCommentsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private string CommentsUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{CoursesRoutingConstants.CoursesTopicBase}/{CourseId}/{DisplayId:D}#comments");

    private string PostUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{CoursesRoutingConstants.CoursesTopicBase}/{CourseId}/{DisplayId:D}");

    private string LastPostUrl => string.Create(CultureInfo.InvariantCulture,
        $"{CoursesRoutingConstants.CoursesTopicBase}/{CourseId}/{_courseTopic!.PreviousTopic?.DisplayId:D}");

    private string NextPostUrl => string.Create(CultureInfo.InvariantCulture,
        $"{CoursesRoutingConstants.CoursesTopicBase}/{CourseId}/{_courseTopic!.NextTopic?.DisplayId:D}");

    private string EditPostUrlTemplate => CoursesRoutingConstants.WriteCourseTopicEditBase
        .CombineUrl(CourseId?.ToString(CultureInfo.InvariantCulture), escapeRelativeUrl: false)
        .CombineUrl(EncryptedDisplayId, escapeRelativeUrl: true);

    private string DeletePostUrlTemplate => CoursesRoutingConstants.WriteCourseTopicDeleteBase
        .CombineUrl(CourseId?.ToString(CultureInfo.InvariantCulture), escapeRelativeUrl: false)
        .CombineUrl(EncryptedDisplayId, escapeRelativeUrl: true);

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    private string EncryptedDisplayId => ProtectionProvider.Encrypt(DisplayId?.ToString(format: "D")) ?? "";

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        if (!CourseId.HasValue || !DisplayId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        var checkAccessResult =
            await CoursesService.HasUserAccessToThisCourseForReadingAsync(CourseId.Value, ApplicationState.CurrentUser);

        if (checkAccessResult.Stat != OperationStat.Succeeded)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        _courseTopic = await CourseTopicsService.GetTopicAsync(DisplayId.Value);

        if (_courseTopic?.ThisTopic is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await CourseTopicsService.UpdateNumberOfViewsAsync(DisplayId.Value,
            ApplicationState.NavigationManager.IsFromFeed());

        await GetCommentsAsync(_courseTopic.ThisTopic.Id);

        SetSimilarPostsId();

        AddBreadCrumbs();
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _courseTopic?.ThisTopic?.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false).DocumentTypeIdHash;

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..CoursesBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = $"دوره «{CurrentPost?.Course.Title ?? ""}»",
                Url = string.Create(CultureInfo.InvariantCulture,
                    $"{CoursesRoutingConstants.CoursesDetailsBase}/{CourseId}"),
                GlyphIcon = DntBootstrapIcons.BiShare
            },
            new BreadCrumb
            {
                Title = "نظرات دوره",
                Url = string.Create(CultureInfo.InvariantCulture,
                    $"{CoursesRoutingConstants.CourseCommentsBase}/{CourseId}"),
                GlyphIcon = DntBootstrapIcons.BiShare
            }
        ]);

    private async Task GetCommentsAsync(int id)
        => _courseTopicComments = await CourseTopicCommentsService.GetRootCommentsOfTopicsAsync(id);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await CourseTopicCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await CourseTopicCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");

                break;
            case CommentAction.SubmitNewComment:
                await CourseTopicCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId,
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

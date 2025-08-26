using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.PrivateMessages.RoutingConstants;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.PrivateMessages.Components;

[Authorize]
public partial class MyPrivateMessage
{
    private PrivateMessage? _firstPrivateMessage;

    private List<PrivateMessageComment>? _privateMessageComments;

    private string PageTitle => $"پیام خصوصی: {_firstPrivateMessage?.Title}";

    [InjectComponentScoped] internal IPrivateMessagesService PrivateMessagesService { set; get; } = null!;

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? PrivateMessageId { set; get; }

    private bool CanUserDeleteThisPost => CanUserEditThisPost;

    private bool CanUserEditThisPost => ApplicationState.CanCurrentUserEditThisItem(_firstPrivateMessage?.UserId);

    [InjectComponentScoped] internal IPrivateMessageCommentsService PrivateMessageCommentsService { set; get; } = null!;

    private string EncryptedId => ProtectionProvider.Encrypt(PrivateMessageId) ?? "";

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(PrivateMessageId))
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        var id = PrivateMessageId.ToInt();
        await InitFirstPrivateMessageAsync(id);
        await GetCommentsAsync(id);
        await MarkMainMessageAsReadAsync(id);

        AddBreadCrumbs();
    }

    private async Task MarkMainMessageAsReadAsync(int id)
        => await PrivateMessagesService.TryMarkMainMessageAsReadAsync(id, ApplicationState.CurrentUser?.UserId);

    private async Task InitFirstPrivateMessageAsync(int id)
    {
        var firstPrivateMessage =
            await PrivateMessagesService.GetFirstAllowedPrivateMessageAsync(id, ApplicationState.CurrentUser?.UserId);

        if (firstPrivateMessage is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _firstPrivateMessage = firstPrivateMessage;
    }

    private async Task GetCommentsAsync(int id)
        => _privateMessageComments = await PrivateMessageCommentsService.GetRootCommentsOfPrivateMessageAsync(id);

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            PrivateMessagesBreadCrumbs.Users, PrivateMessagesBreadCrumbs.MyPrivateMessages, new BreadCrumb
            {
                Title = PageTitle,
                Url = PrivateMessagesRoutingConstants.MyPrivateMessageBase.CombineUrl(EncryptedId,
                    escapeRelativeUrl: true),
                GlyphIcon = DntBootstrapIcons.BiInbox
            }
        ]);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        var currentUserId = ApplicationState.CurrentUser?.UserId;

        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await PrivateMessageCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await PrivateMessageCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");
                await PrivateMessagesService.TryMarkMainMessageAsUnReadAsync(model.FormPostId, currentUserId);

                break;
            case CommentAction.SubmitNewComment:
                await PrivateMessageCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId,
                    model.Comment ?? "", ApplicationState.CurrentUser?.User);

                await PrivateMessagesService.TryMarkMainMessageAsReadAsync(model.FormPostId, currentUserId);

                await PrivateMessageCommentsService.SendReplyEmailsAsync(model.FormPostId,
                    ApplicationState.CurrentUser?.User, model.Comment ?? "");

                break;
            case CommentAction.Cancel:
            case CommentAction.ReplyToPost:
            case CommentAction.ReplyToComment:
            case CommentAction.Edit:
            default:
                return;
        }

        await GetCommentsAsync(model.FormPostId);
        StateHasChanged();
    }
}

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

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? PrivateMessageId { set; get; }

    private bool CanUserDeleteThisPost => CanUserEditThisPost;

    private bool CanUserEditThisPost => ApplicationState.CanCurrentUserEditThisItem(PrivateMessageId);

    [InjectComponentScoped] internal IPrivateMessageCommentsService PrivateMessageCommentsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!PrivateMessageId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await InitFirstPrivateMessageAsync(PrivateMessageId.Value);
        await GetCommentsAsync(PrivateMessageId.Value);
        await MarkMainMessageAsReadAsync(PrivateMessageId.Value);

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
                Url = Invariant($"/{PrivateMessagesRoutingConstants.MyPrivateMessageBase}/{PrivateMessageId}"),
                GlyphIcon = DntBootstrapIcons.BiInbox
            }
        ]);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        var currentUserId = ApplicationState.CurrentUser?.UserId;

        switch (model.CommentAction)
        {
            case CommentAction.ReplyToPost:
                break;
            case CommentAction.ReplyToComment:
                break;
            case CommentAction.Edit:
                break;
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
                break;
        }

        await GetCommentsAsync(model.FormPostId);
        StateHasChanged();
    }
}

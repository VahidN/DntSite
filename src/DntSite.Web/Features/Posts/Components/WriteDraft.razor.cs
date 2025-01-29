using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Posts.Components;

[Authorize]
public partial class WriteDraft
{
    [InjectComponentScoped] internal IStatService StatService { set; get; } = null!;

    [InjectComponentScoped] internal IBlogPostsEmailsService EmailsService { set; get; } = null!;

    [InjectComponentScoped] internal IBlogCommentsService BlogCommentsService { set; get; } = null!;

    [InjectComponentScoped] internal IBlogCommentsEmailsService BlogCommentsEmailsService { get; set; } = null!;

    [InjectComponentScoped] internal IBlogPostDraftsService BlogPostDraftsService { set; get; } = null!;

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm(FormName = nameof(WriteDraft))]
    public WriteDraftModel WriteDraftModel { get; set; } = new();

    [SupplyParameterFromForm(FormName = "ConvertToComment")]
    public ConvertToCommentModel ConvertToCommentModel { get; set; } = new();

    [SupplyParameterFromForm(FormName = "ConvertToLink")]
    public ConvertToLinkModel ConvertToLinkModel { get; set; } = new();

    public IList<string>? AutoCompleteDataList { get; set; }

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [InjectComponentScoped] internal IDailyNewsEmailsService DailyNewsEmailsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);
        AddBreadCrumbs();

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        await FillPossibleEditFormAsync();

        await PerformPossibleDeleteAsync();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..PostsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task PerformPossibleDeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(DeleteId))
        {
            return;
        }

        var draft = await GetUserDraftAsync(DeleteId.ToInt());
        await BlogPostDraftsService.MarkAsDeletedAsync(draft);

        await EmailsService.DeleteDraftSendEmailAsync(draft);

        ApplicationState.NavigateTo(PostsRoutingConstants.MyDrafts);
    }

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var draft = await GetUserDraftAsync(EditId.ToInt());

        if (draft is null)
        {
            return;
        }

        WriteDraftModel = Mapper.Map<BlogPostDraft, WriteDraftModel>(draft);
    }

    private async Task<BlogPostDraft?> GetUserDraftAsync(int id)
    {
        var draft = await BlogPostDraftsService.FindBlogPostDraftIncludeUserAsync(id);

        if (!ApplicationState.CurrentUser.CanUserEditThisDraft(draft))
        {
            ApplicationState.NavigateToNotFoundPage();

            return null;
        }

        return draft;
    }

    private async Task PerformAsync()
    {
        if (!string.IsNullOrWhiteSpace(EditId))
        {
            await UpdateDraftAsync();
        }
        else
        {
            await AddDraftAsync();
        }
    }

    private async Task UpdateDraftAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var draft = await GetUserDraftAsync(EditId.ToInt());

        if (draft is null)
        {
            return;
        }

        await BlogPostDraftsService.UpdateBlogPostDraftAsync(WriteDraftModel, draft);

        await EmailsService.WriteDraftSendEmailAsync(draft);

        if (draft.UserId is not null)
        {
            await StatService.UpdateNumberOfDraftsStatAsync(draft.UserId.Value);
        }

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{PostsRoutingConstants.ShowDraftBase}/{draft.Id}"));
    }

    private async Task AddDraftAsync()
    {
        if (!string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var result = await BlogPostDraftsService.AddBlogPostDraftAsync(WriteDraftModel);

        await EmailsService.WriteDraftSendEmailAsync(result);

        if (result.UserId is not null)
        {
            await StatService.UpdateNumberOfDraftsStatAsync(result.UserId.Value);
        }

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{PostsRoutingConstants.ShowDraftBase}/{result.Id}"));
    }

    private async Task PerformConvertToLinkAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        if (ApplicationState.CurrentUser?.IsAdmin == false)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        var draft = await GetUserDraftAsync(EditId.ToInt());

        if (draft is null)
        {
            return;
        }

        var operationResult = await BlogPostDraftsService.ConvertDraftToLinkAsync(new DailyNewsItemModel
        {
            Url = ConvertToLinkModel.Url,
            DescriptionText = draft.Body,
            Title = draft.Title,
            Tags = draft.Tags
        }, draft.Id);

        if (!operationResult.IsSuccess)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", operationResult.Message);

            return;
        }

        await DailyNewsEmailsService.ConvertedDailyNewsItemsSendEmailAsync(draft.Id, draft.Title, draft.UserId ?? 0);

        await BlogPostDraftsService.DeleteDraftAsync(draft);

        ApplicationState.NavigateTo(NewsRoutingConstants.NewsDetailsBase.CombineUrl(
            operationResult.Result?.Id.ToString(CultureInfo.InvariantCulture), escapeRelativeUrl: false));
    }

    private async Task PerformConvertToCommentAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        if (ApplicationState.CurrentUser?.IsAdmin == false)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        var draft = await GetUserDraftAsync(EditId.ToInt());

        if (draft is null)
        {
            return;
        }

        var blogPost = await BlogCommentsService.FindCommentPostAsync(ConvertToCommentModel.ReplyPostId);

        if (blogPost is null || blogPost.IsDeleted)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", message: "شماره مطلب وارد شده وجود خارجی ندارد.");

            return;
        }

        await BlogCommentsService.AddReplyAsync(replyId: null, ConvertToCommentModel.ReplyPostId,
            $"<b>یک نکته‌ی تکمیلی: {draft.Title}</b></br></br>{draft.Body}", draft.UserId ?? 0);

        await BlogCommentsEmailsService.ConvertedToReplySendEmailAsync(ConvertToCommentModel.ReplyPostId, draft.Title,
            draft.UserId ?? 0);

        await BlogPostDraftsService.DeleteDraftAsync(draft);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{PostsRoutingConstants.PostBase}/{ConvertToCommentModel.ReplyPostId}"));
    }
}

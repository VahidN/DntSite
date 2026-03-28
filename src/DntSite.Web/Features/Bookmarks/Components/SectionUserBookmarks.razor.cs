using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Bookmarks.Models;
using DntSite.Web.Features.Bookmarks.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Bookmarks.Components;

public partial class SectionUserBookmarks<TBookmarkEntity, TForeignKeyEntity>
    where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>, new()
    where TForeignKeyEntity : BaseAuditedInteractiveEntity
{
    private PagedResultModel<TBookmarkEntity>? _pagedResultModel;

    [InjectComponentScoped] internal IBookmarksService BookmarksService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [Parameter] public RenderFragment<TBookmarkEntity>? ItemTemplate { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ManagePostedFormAsync();

        _pagedResultModel =
            await BookmarksService.GetUserBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(
                ApplicationState.CurrentUser?.UserId, CurrentPage ??= 1, ItemsPerPage);
    }

    private async Task ManagePostedFormAsync()
    {
        var httpRequest = ApplicationState.HttpContext.Request;

        if (httpRequest.HasFormContentType &&
            httpRequest.Form.TryGetValue(nameof(DntBookmark<,>.FormId), out var formIdStrVal) &&
            httpRequest.Form.TryGetValue(nameof(DntBookmark<,>.ActionType), out var actionTypeStrVal))
        {
            var formId = formIdStrVal.ToString().ToInt();
            var actionType = actionTypeStrVal.ToString().ToEnum<BookmarkActionType>();

            if (actionType == BookmarkActionType.Add)
            {
                await BookmarksService.SavePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(formId, actionType,
                    ApplicationState.CurrentUser?.UserId);
            }
        }
    }
}

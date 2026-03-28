using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Bookmarks.Models;
using DntSite.Web.Features.Bookmarks.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Bookmarks.Components;

public partial class DntBookmark<TBookmarkEntity, TForeignKeyEntity>
    where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>, new()
    where TForeignKeyEntity : BaseAuditedInteractiveEntity
{
    private bool _isAdded;
    private bool _isDeleted;

    private string FormName => string.Create(CultureInfo.InvariantCulture,
        $"Bookmark_{typeof(TBookmarkEntity).Name}_{typeof(TForeignKeyEntity).Name}_{Id}");

    private bool ShowBookmarksForm => ApplicationState.CurrentUser?.IsAuthenticated == true;

    [SupplyParameterFromForm] public int FormId { set; get; }

    [SupplyParameterFromForm] public BookmarkActionType ActionType { set; get; }

    [Parameter] [EditorRequired] public int Id { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    [InjectComponentScoped] internal IBookmarksService BookmarksService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public ICollection<TBookmarkEntity>? Bookmarks { set; get; }

    private bool IsUserReacted => Bookmarks?.Any(x => x.UserId == ApplicationState.CurrentUser?.UserId) == true;

    private bool ShowAddBookmarkButton => !_isAdded && (BookmarksUsersCount == 0 || !IsUserReacted || _isDeleted);

    private int BookmarksUsersCount => Bookmarks?.Count ?? 0;

    private async Task OnValidSubmitAsync()
    {
        var userId = ApplicationState.CurrentUser?.UserId;

        if (userId is null)
        {
            ApplicationState.NavigateTo(UserProfilesRoutingConstants.Login);

            return;
        }

        var success =
            await BookmarksService.SavePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(FormId, ActionType,
                userId.Value);

        _isDeleted = success && ActionType == BookmarkActionType.Cancel;
        _isAdded = success && ActionType == BookmarkActionType.Add;
    }
}

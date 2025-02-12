using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Stats.Components;

public partial class DntReactions<TReactionEntity, TForeignKeyEntity>
    where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>, new()
    where TForeignKeyEntity : BaseAuditedInteractiveEntity
{
    private bool _isLoadingList;
    private ShowReactionsModel? _reactionsInfo;
    private IList<User?>? _thumbsDownUsers;
    private IList<User?>? _thumbsUpUsers;

    [MemberNotNullWhen(returnValue: true, nameof(_thumbsUpUsers))]
    private bool HasThumbsUpUsers => _thumbsUpUsers is { Count: > 0 };

    [MemberNotNullWhen(returnValue: true, nameof(_thumbsDownUsers))]
    private bool HasThumbsDownUsers => _thumbsDownUsers is { Count: > 0 };

    private string FormName => string.Create(CultureInfo.InvariantCulture,
        $"Reaction_{typeof(TReactionEntity).Name}_{typeof(TForeignKeyEntity).Name}_{Id}");

    private string ThumbsUpId { get; } = Guid.NewGuid().ToString(format: "N");

    private string ThumbsDownId { get; } = Guid.NewGuid().ToString(format: "N");

    private int ThumbsUpUsersCount => _reactionsInfo?.ThumbsUpUsersCount ?? 0;

    private int ThumbsDownUsersCount => _reactionsInfo?.ThumbsDownUsersCount ?? 0;

    private int TotalReactionsCount => ThumbsDownUsersCount + ThumbsUpUsersCount;

    private bool AreReactionsDisabled => _reactionsInfo?.AreReactionsDisabled == true;

    private bool IsCurrentUserReacted => _reactionsInfo?.IsCurrentUserReacted == true;

    [SupplyParameterFromForm] public ReactionType Reaction { set; get; }

    [SupplyParameterFromForm] public int FormId { set; get; }

    [Parameter] [EditorRequired] public int Id { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    [InjectComponentScoped] internal IUserRatingsService UserRatingsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public required ICollection<TReactionEntity> Reactions { set; get; }

    protected override void OnParametersSet() => _reactionsInfo = GetReactionsInfo();

    private async Task OnValidSubmitAsync()
    {
        if (Reaction == ReactionType.ShowList)
        {
            await ShowUpdatedReactionsAsync(new ReactionModel
            {
                FormId = FormId,
                Reaction = Reaction
            });
        }
        else
        {
            await HandleUserReactionAsync(new ReactionModel
            {
                FormId = FormId,
                Reaction = Reaction
            });
        }
    }

    private async Task ShowUpdatedReactionsAsync(ReactionModel userFormReaction)
    {
        try
        {
            _isLoadingList = true;
            (_thumbsDownUsers, _thumbsUpUsers, _reactionsInfo) = await HandleShowReactionsListAsync(userFormReaction);
        }
        finally
        {
            _isLoadingList = false;
        }
    }

    private Task<ShowReactionsUsersListModel> HandleShowReactionsListAsync(ReactionModel arg)
        => UserRatingsService.GetReactionsUsersListAsync<TReactionEntity, TForeignKeyEntity>(arg.FormId,
            ApplicationState);

    private async Task HandleUserReactionAsync(ReactionModel arg)
    {
        var userId = ApplicationState.CurrentUser?.UserId;

        if (userId is null)
        {
            ApplicationState.NavigateTo(UserProfilesRoutingConstants.Login);

            return;
        }

        await UserRatingsService.SaveRatingAsync<TReactionEntity, TForeignKeyEntity>(arg.FormId, arg.Reaction,
            userId.Value);

        await ShowUpdatedReactionsAsync(new ReactionModel
        {
            Reaction = ReactionType.ShowList,
            FormId = arg.FormId
        });
    }

    private ShowReactionsModel GetReactionsInfo()
        => UserRatingsService.GetReactionsInfo<TReactionEntity, TForeignKeyEntity>(ApplicationState, Reactions);
}

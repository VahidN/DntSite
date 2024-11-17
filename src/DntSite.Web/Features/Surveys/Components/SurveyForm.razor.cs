using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Components;

public partial class SurveyForm
{
    private bool _canUserVote;

    private bool IsFormReadonly => IsSurveyExpired || !_canUserVote;

    private string FormName => string.Create(CultureInfo.InvariantCulture, $"SurveyForm_{Survey?.Id}");

    [InjectComponentScoped] internal IVotesService VotesService { set; get; } = null!;

    [SupplyParameterFromForm] public int FormId { set; get; }

    [SupplyParameterFromForm] public IList<int>? SelectedValues { set; get; }

    [Parameter] [EditorRequired] public Survey? Survey { set; get; }

    [Parameter]
    [EditorRequired]
    public Func<(int FormId, IList<int> SurveyItemIds), Task>? HandleVoteAction { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool IsSurveyExpired => Survey?.DueDate is not null && Survey.DueDate.Value <= DateTime.UtcNow;

    protected override async Task OnInitializedAsync() => _canUserVote = await CanUserVoteAsync();

    private async Task<bool> CanUserVoteAsync()
        => ApplicationState.CurrentUser?.IsAuthenticated != false &&
           await VotesService.CanUserVoteAsync(Survey, ApplicationState.CurrentUser?.User);

    private async Task OnValidSubmitAsync()
    {
        if (HandleVoteAction is null)
        {
            throw new InvalidOperationException($"{nameof(HandleVoteAction)} is null.");
        }

        await HandleVoteAction.Invoke((FormId, SelectedValues ?? []));
    }
}

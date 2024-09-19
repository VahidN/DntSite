using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;

namespace DntSite.Web.Features.Backlogs.Components;

public partial class BacklogActionForm
{
    [MemberNotNullWhen(returnValue: true, nameof(Model))]
    private bool IsTaken => Model?.TakenByUser is not null;

    private string FormName => string.Create(CultureInfo.InvariantCulture, $"BacklogStat_{Model?.Id}");

    private bool IsTakenByCurrentUser => ApplicationState.CurrentUser?.IsAdmin == true ||
                                         (Model?.TakenByUser is not null && Model.TakenByUser.Id ==
                                             ApplicationState.CurrentUser?.UserId);

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public ManageBacklogModel? Model { set; get; }

    [SupplyParameterFromForm] public int ConvertedBlogPostId { set; get; }

    [SupplyParameterFromForm] public int DaysEstimate { set; get; }

    [SupplyParameterFromForm] public BacklogAction BacklogActionValue { set; get; }

    [InjectComponentScoped] internal IBacklogsService BacklogsService { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        ConvertedBlogPostId = Model?.ConvertedBlogPostId ?? 0;
        DaysEstimate = Model?.DaysEstimate ?? 0;
    }

    private async Task PerformAsync()
    {
        if (Model is null || ApplicationState.CurrentUser?.IsAuthenticated == false)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", message: "اطلاعات وارد شده ناقص هستند.");

            return;
        }

        Model.DaysEstimate = DaysEstimate;
        Model.ConvertedBlogPostId = ConvertedBlogPostId;

        var currentUser = ApplicationState.CurrentUser;
        var siteRootUri = ApplicationState.AppSetting?.SiteRootUri;

        var operationResult = BacklogActionValue switch
        {
            BacklogAction.Update or BacklogAction.IsDone => await BacklogsService.DoneBacklogAsync(Model, currentUser,
                siteRootUri),
            BacklogAction.Cancel => await BacklogsService.CancelBacklogAsync(Model, currentUser, siteRootUri),
            BacklogAction.Take => await BacklogsService.TakeBacklogAsync(Model, currentUser, siteRootUri),
            _ => throw new InvalidOperationException()
        };

        if (operationResult.Stat == OperationStat.Succeeded)
        {
            ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
                $"{BacklogsRoutingConstants.BacklogsDetailsBase}/{Model.Id}"));
        }
        else
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", operationResult.Message);
        }
    }
}

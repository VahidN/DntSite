using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class Activation
{
    private string? _alertMessage;
    private AlertType _alertType = AlertType.Info;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IUserProfilesManagerService UsersService { set; get; } = null!;

    [Parameter] public string? Data { set; get; }

    protected override async Task OnInitializedAsync()
    {
        ApplicationState.DoNotLogPageReferrer = true;

        if (string.IsNullOrWhiteSpace(Data))
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        var operationResult = await UsersService.ActivateEmailAsync(Data);
        _alertMessage = operationResult.Message;
        _alertType = operationResult.Stat == OperationStat.Succeeded ? AlertType.Success : AlertType.Danger;
    }
}

using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class EditProfileOptions
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [EditorRequired] [Parameter] public required string EncryptedUserId { set; get; }

    [EditorRequired] [Parameter] public int UserId { set; get; }

    private bool IsVisible => ApplicationState.CurrentUser?.IsAuthenticated == true &&
                              (ApplicationState.IsCurrentUserAdmin || ApplicationState.CurrentUser?.UserId == UserId);
}
